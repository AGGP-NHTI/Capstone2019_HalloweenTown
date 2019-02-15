
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileCollisionManager : MonoBehaviour {
    HealthBar myHealthBar;
    MoveScript myMoveScript;
    ProjectileManager myProjecctileManager;
    public float stunTime = 5.0f;
    public float iFrameTimer = 8.0f;
    public bool canBeHit = true;
    Stun myStun;
	// Use this for initialization
	void Start () {
        myMoveScript = GetComponent<MoveScript>();
        myHealthBar = GetComponent<HealthBar>();
        myProjecctileManager = GetComponent<ProjectileManager>();
        myStun = GetComponent<Stun>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}/*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Egg"))
        {
            if (canBeHit)
            {
                Debug.Log("I've been hit by an egg!");
                float d = collision.gameObject.GetComponent<Egg>().damage;
                myHealthBar.TakeDamage(d);
            }

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Toilet Paper"))
        {
            if (canBeHit)
            {
                Debug.Log("I've been hit by an Toilet Paper!");
                //disable movement
                StartCoroutine(myStun.suspendMovement(stunTime));
            }
            Destroy(collision.gameObject);
        }
        //else Destroy(gameObject);
    }*/
    private IEnumerator suspendMovement1()
    {
        bool couldBeHit = canBeHit;
        bool canThrowBeforeHit = myProjecctileManager.canThrow;
        myProjecctileManager.canThrow = false;
        canBeHit = false;
        float timer = 0f;
        float rate = 1 / stunTime;

        bool stun = true;
        bool sprintingBeforeHit = myMoveScript.allowSprinting;
        bool jumpingBeforeHit = myMoveScript.allowJumping;
        bool crouchingBeforeHit = myMoveScript.allowCrouching;
        float moveSpeedBeforeHit = myMoveScript.moveSpeed;
        

        myMoveScript.allowSprinting = false;
        myMoveScript.allowJumping = false;
        myMoveScript.allowCrouching = false;
        myMoveScript.moveSpeed = 0.0f;

        while (stun)
        {

            timer += Time.deltaTime;
            if (timer > stunTime) stun = false;
            yield return 0;
        }
        myMoveScript.allowSprinting = sprintingBeforeHit;
        myMoveScript.allowJumping = jumpingBeforeHit;
        myMoveScript.allowCrouching = crouchingBeforeHit;
        myMoveScript.moveSpeed = moveSpeedBeforeHit ;
        canBeHit = couldBeHit;
        myProjecctileManager.canThrow = canThrowBeforeHit;
    }

    private IEnumerator makeInvicible()
    {
        float timer = 0f;
        bool invincible = true;
        while(invincible)
        {

            timer += Time.deltaTime;
            if (timer > iFrameTimer) invincible = false;
            yield return 0;

        }

    }
}
