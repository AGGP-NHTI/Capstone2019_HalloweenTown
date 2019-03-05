using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour {
    ProjectileManager myProjectileManager;
    MoveScript myMoveScript;
    PlayerProjectileCollisionManager myCollisionManager;
    public bool stunned = false;
    
    // Use this for initialization
    void Start () {
		myMoveScript = GetComponent<MoveScript>();
        myProjectileManager = GetComponent<ProjectileManager>();
        myCollisionManager = GetComponent<PlayerProjectileCollisionManager>();
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    

    public IEnumerator suspendMovement(float duration)
    {
        
        float timer = 0f;
        float rate = 1 / duration;
        stunned = true;
        //store original values
        bool couldBeHit = myCollisionManager.canBeHit;
        bool canThrowBeforeHit = myProjectileManager.canThrow;
        bool sprintingBeforeHit = myMoveScript.allowSprinting;
        bool jumpingBeforeHit = myMoveScript.allowJumping;
        bool crouchingBeforeHit = myMoveScript.allowCrouching;
        float moveSpeedBeforeHit = myMoveScript.moveSpeed;

        //disable
        myMoveScript.allowSprinting = false;
        myMoveScript.allowJumping = false;
        myMoveScript.allowCrouching = false;
        myMoveScript.moveSpeed = 0.0f;
        myProjectileManager.canThrow = false;
        myCollisionManager.canBeHit = false;

        //count
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //enable
        myMoveScript.allowSprinting = sprintingBeforeHit;
        myMoveScript.allowJumping = jumpingBeforeHit;
        myMoveScript.allowCrouching = crouchingBeforeHit;
        myMoveScript.moveSpeed = moveSpeedBeforeHit;
        myCollisionManager.canBeHit = couldBeHit;
        myProjectileManager.canThrow = canThrowBeforeHit;
        stunned = false;
        Debug.Log("done being stunned");
    }
}
