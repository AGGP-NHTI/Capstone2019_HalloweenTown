﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class MoveScript : MonoBehaviour
{
    public Animator anim;
    Pawn pawn;

    #region Pawn Properties
    public Transform movementRelativeTransform;

    //Mobility properties
    public bool allowSprinting = true;
    public bool allowJumping = true;
    public bool allowCrouching = true;
    public float moveSpeed = 5.0f;
    [Range(0.0f, 1.0f)]
    public float groundedInertia = 0.5f;
    [Range(0.0f, 1.0f)]
    public float aerialInertia = 0.5f;
    public float sprintMultiplier = 2.0f;
    public float crouchMultiplier = 0.5f;
    public float crouchRate = 0.2f;
    public float jumpForce = 3.0f;
    public float maxGroundAngle = 45;
    public float coyoteTimeDuration = 0.1f;
    public float gravity = 20.0f;
    [HideInInspector]
    public bool letBeGrounded = true;

    //Audio sources
    public AudioSource feetAudio;

    //Audio properties
    public AudioClip[] footstepSound;
    public float minFootstepVelocity = 0.01f;
    public float minFootstepBreak = 0.1f;
    public float maxFootstepBreak = 2.0f;
    #endregion

    #region Pawn Member Variables
    //General components needed to be tracked
    protected Rigidbody _rb;
    protected CapsuleCollider _col;

    //Internal booleans
    protected bool _isCrouching = false;
    protected bool _isSprinting = false;
    protected bool _isJumping = false;

    //Grounded-related variables
    [SerializeField]protected bool _isGrounded = false;
    [SerializeField]protected bool _shouldBeGrounded = false;
    protected Vector3 _groundContactNormal;
    protected float _remainingCoyoteTime;

    //Movement value storage
    protected float _forwardVelocity = 1.0f;
    protected float _strafeVelocity = 1.0f;

    //Crouching-related variables
    protected float _playerHeight;
    protected float _playerInitialScale;
    protected float _crouchPercent = 0.0f;

    //Audio related
    protected bool _footstepAudioCoroutineIsActive = false;
    #endregion

    protected virtual void Start()
    {
        pawn = GetComponent<Pawn>();
        //Grab initial scale to use in crouching later on
        _playerInitialScale = transform.localScale.y;

        //Get Rigidbody component
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.useGravity = false;

        //Grab the main collider of the object (intended to be a CapsuleCollider possibly on a child object) and use it's height as the player's height (used in crouching)
        _col = gameObject.GetComponentInChildren<CapsuleCollider>();
        _playerHeight = _col.height;
    }

    protected virtual void FixedUpdate()
    {
        _rb.AddForce(new Vector3(0.0f, -gravity * _rb.mass, 0.0f));

        CheckIfGrounded();
        if (_rb.velocity.sqrMagnitude > minFootstepVelocity && !_footstepAudioCoroutineIsActive && _isGrounded)
        {
            StartCoroutine(HandleFootstepAudio());

        }
        if (anim != null)
        {
            Vector3 curVel = _rb.velocity;
            curVel.y = 0.0f;
            if (curVel.magnitude > 1f)
            {
                anim.SetBool("runBool", true);
            }
            else
            {
                anim.SetBool("runBool", false);
            }

        }
        UpdateMoveVelocity();
        HandleCrouching();
    }

    #region Pawn's Controller Inputs

    public virtual void MoveHorizontal(float value)
    {
        _strafeVelocity = value;
    }

    public virtual void MoveVertical(float value)
    {
        _forwardVelocity = value;
    }

    //Make the player jump
    public virtual void Jump(bool value)
    {
        if (value && allowJumping && _isGrounded)
        {
            _isJumping = true;
        }
    }

    //Toggles the players' sprint on
    public virtual void Sprint(bool value)
    {
        if (value && allowSprinting && !_isCrouching)
        {
            _isSprinting = true;
        }
        else
        {
            _isSprinting = false;
        }
    }

    //Crouches the player when held
    public virtual void Crouch(bool value)
    {
        if (!allowCrouching) { return; }
        //If currently crouching and the crouch button isn't being held, try to stand up
        //Else crouch.

        if (_isCrouching && !value)
        {
            //Prepare data for use in CheckCapsule()
            Vector3 p1 = _col.transform.position;
            Vector3 p2 = p1 + (Vector3.up * _playerHeight * 0.524f);
            float checkRadius = _col.radius * 0.9f;
            int layermask = 1 << LayerMask.NameToLayer("Player");
            layermask = ~layermask;

            //Check to see if the player has enough room to stand up
            bool didCollide = Physics.CheckCapsule(p1, p2, checkRadius, layermask, QueryTriggerInteraction.Ignore);
            //If there's nothing in their way, let the player stop crouching
            if (!didCollide)
            {
                _isCrouching = false;
            }
        }
        else if (value)
        {
            _isCrouching = true;
        }
    }
    #endregion

    #region Movement Related Methods
    protected virtual void UpdateMoveVelocity()
    {
        //Initialize moveVelocity to zero. 
        Vector3 desiredVelocity = Vector3.zero;

        //Modify input data to remove issue of faster movement on non-axes
        Vector2 inputVector = GetProperInputVector();

        //Apply sprint effects if trying to sprint forwards.
        if (_isSprinting && _forwardVelocity > 0.0f)
        {
            inputVector.x *= sprintMultiplier;
        }

        //Combine the vectors of transform.forward and tranform.right to find the desired move vector.
        //Use modified input data stored in _forwardVelocity and _strafeVelocity as the scalars for these vectors, respectively.
        if (movementRelativeTransform)
        {
            Vector3 correctForward = movementRelativeTransform.forward;
            correctForward.y = 0.0f;
            correctForward.Normalize();
            Vector3 correctRight = movementRelativeTransform.right;
            correctRight.y = 0.0f;
            correctRight.Normalize();

            desiredVelocity = correctForward * inputVector.x + correctRight * inputVector.y;
        }
        else
        {
            desiredVelocity = transform.forward * inputVector.x + transform.right * inputVector.y;
        }
        desiredVelocity.y = 0.0f;

        //Scale velocity by moveSpeed
        desiredVelocity *= moveSpeed;

        //Scale velocity by crouch multiplier if the player is crouching
        if (_isCrouching)
        {
            desiredVelocity *= crouchMultiplier;
        }

        if (_shouldBeGrounded && !_isJumping)
        {
            desiredVelocity.y = _rb.velocity.y;
            desiredVelocity = Vector3.ProjectOnPlane(desiredVelocity, _groundContactNormal);
            Vector3 newVelocity = Vector3.Lerp(desiredVelocity, _rb.velocity, groundedInertia);
            _rb.velocity = newVelocity;
        }
        else
        {
            if (inputVector.sqrMagnitude > float.Epsilon)
            {
                desiredVelocity.y = _rb.velocity.y;
                _rb.velocity = Vector3.Lerp(desiredVelocity, _rb.velocity, aerialInertia);
            }
            if (_isJumping)
            {
                //anim.GetBool("jumpBool");

                
                    //anim.GetCurrentAnimatorClipInfo(0).Length == 0
                if (anim && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    anim.SetBool("jumpBool", true);
                }
                _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
                _isJumping = false;
                _isGrounded = false;
            }
            else if(anim)
            {
                anim.SetBool("jumpBool", false);
            }
        }
        //Debug.DrawRay(transform.position, desiredVelocity, Color.cyan, 1.0f);
        //Debug.DrawRay(transform.position, _rb.velocity, Color.green, 1.0f);
    }

    //Adjusts player height to reflect crouch state
    protected virtual void HandleCrouching()
    {
        float playerHeightScale = Mathf.Lerp(_playerInitialScale, _playerInitialScale * 0.5f, _crouchPercent);
        transform.localScale = new Vector3(1.0f, playerHeightScale, 1.0f);

        if (_isCrouching && _crouchPercent < 1.0f)
        {
            _crouchPercent += Time.fixedDeltaTime * crouchRate;
        }
        else if (!_isCrouching && _crouchPercent > 0.0f)
        {
            _crouchPercent -= Time.fixedDeltaTime * crouchRate;
        }
    }
    #endregion

    #region Helper Functions

    private void OnDrawGizmos()
    {
        if(!_col) { return; }

        Vector3 checkPos = _col.transform.position + _col.center;
        float checkDist = (_col.height / 2 - _col.radius) * 1.01f * transform.localScale.y;
        float r = _col.radius;

        Gizmos.DrawWireSphere(checkPos, r);
        Gizmos.DrawWireSphere(checkPos + Vector3.down * checkDist, r);
    }

    protected virtual void CheckIfGrounded()
    {
        //Prepare data for use in CheckSphere()
        Vector3 checkPos = _col.transform.position + _col.center;
        float checkDist = (_col.height / 2 - _col.radius) * 1.01f * transform.localScale.y;

        //If the player's feet are touching something, player is grounded
        RaycastHit hitInfo;
        _shouldBeGrounded = Physics.SphereCast(checkPos, _col.radius, Vector3.down, out hitInfo, checkDist, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        
        if (!letBeGrounded)
        {
            _shouldBeGrounded = false;
        }
        
        if (_shouldBeGrounded)
        {
            //Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.yellow, 1.0f);

            //If ground is too steep (and also not classified as stairs) then the player isn't actually grounded
            _groundContactNormal = hitInfo.normal;
            if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Stairs"))
            {
                if (Vector3.Angle(Vector3.up, _groundContactNormal) > maxGroundAngle)
                {
                    _shouldBeGrounded = false;
                }
            }
        }

        //Check to see if we should start coyote time
        if (_isGrounded && !_shouldBeGrounded && _remainingCoyoteTime <= 0.0f)
        {
            //Start coyote time
            StartCoroutine(CoyoteTimeTimer());
        }
        else if (!_isGrounded)
        {
            _isGrounded = _shouldBeGrounded;
        }
    }

    protected virtual IEnumerator CoyoteTimeTimer()
    {
        _remainingCoyoteTime = coyoteTimeDuration;
        
        while (!_shouldBeGrounded && _remainingCoyoteTime > 0.0f)
        {
            yield return null;
            _remainingCoyoteTime -= Time.deltaTime;
        }
        _remainingCoyoteTime = 0.0f;
        _isGrounded = _shouldBeGrounded;
    }

    protected virtual Vector2 GetProperInputVector()
    {
        Vector2 inputVector = new Vector2(_forwardVelocity, _strafeVelocity);
        Vector2 maxedVector = Vector2.one;

        //Find maximum value 
        if (Mathf.Abs(_forwardVelocity) > Mathf.Abs(_strafeVelocity))
        {
            maxedVector.Set(1.0f, _strafeVelocity / _forwardVelocity);
            if (_forwardVelocity < 0.0f)
            {
                maxedVector.x = -1.0f;
            }
        }
        else if (Mathf.Abs(_forwardVelocity) < Mathf.Abs(_strafeVelocity))
        {
            maxedVector.Set(_forwardVelocity / _strafeVelocity, 1.0f);
            if (_strafeVelocity < 0.0f)
            {
                maxedVector.y = -1.0f;
            }
        }

        inputVector /= maxedVector.magnitude;

        return new Vector3(inputVector.x, inputVector.y);
    }
    #endregion

    #region Audio
    //Make the sound play frequently, with a faster velocity meaning a higher frequency. Also the player must be grounded.
    protected virtual IEnumerator HandleFootstepAudio()
    {
        _footstepAudioCoroutineIsActive = true;
        float activeTimer = minFootstepBreak + 1;
        float maximumSquareVelocity = moveSpeed * moveSpeed * sprintMultiplier * sprintMultiplier;
        float timeUntilNextSound;

        if(feetAudio)
        {
            do
            {
                timeUntilNextSound = Mathf.Lerp(maxFootstepBreak, minFootstepBreak, _rb.velocity.sqrMagnitude / maximumSquareVelocity);
                if (activeTimer >= timeUntilNextSound)
                {
                    if (footstepSound.Length != 0)
                    {
                        feetAudio.clip = SelectClipFrom(footstepSound);
                    }
                    feetAudio.Play();
                    activeTimer = 0.0f;
                }
                yield return null;
                activeTimer += Time.deltaTime;
            } while (_rb.velocity.sqrMagnitude > minFootstepVelocity && _isGrounded);
        }
       
        _footstepAudioCoroutineIsActive = false;
    }

    AudioClip SelectClipFrom(AudioClip[] arr)
    {
        if (arr.Length == 1)
        {
            return arr[0];
        }

        int index = (int)Random.Range(0, arr.Length - 1);
        return arr[index];
    }
    #endregion
}