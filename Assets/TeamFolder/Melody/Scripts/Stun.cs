using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour {
    ProjectileManager myProjectileManager;
    MoveScript myMoveScript;
    PlayerProjectileCollisionManager myCollisionManager;
    Boo myBoo;
    public bool stunned = false;

    Pawn pawn;

   public Coroutine stun;
    
    // Use this for initialization
    void Start () {
		myMoveScript = GetComponent<MoveScript>();
        myProjectileManager = GetComponent<ProjectileManager>();
        myCollisionManager = GetComponent<PlayerProjectileCollisionManager>();
        pawn = GetComponent<Pawn>();
        myBoo = GetComponent<Boo>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StunPlayer(float stunTime)
    {
        if(stun == null)
        {
            stun = StartCoroutine(suspendMovement(stunTime));
        }        
    }
    

    public IEnumerator suspendMovement(float duration)
    {
        pawn.MyCandy.DropCandy();
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
        bool couldBoo = myBoo.canBoo;

        //disable
        myMoveScript.allowSprinting = false;
        myMoveScript.allowJumping = false;
        myMoveScript.allowCrouching = false;
        myMoveScript.moveSpeed = 0.0f;
        myProjectileManager.canThrow = false;
        myCollisionManager.canBeHit = false;
        myBoo.canBoo = false;
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
        myBoo.canBoo = couldBoo;
        stunned = false;
        Debug.Log("done being stunned");

        stun = null;
    }
}
