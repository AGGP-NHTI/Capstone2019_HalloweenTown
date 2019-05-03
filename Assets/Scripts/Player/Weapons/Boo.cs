using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour {
    
    public int radiusOfBoo = 60;
    public float damage = 10f;
    public bool canBoo = true;
    Pawn pawn;
    SoundManager soundManager;
    ParticleManager particleManager;
    bool werewolfUlt = false;
    bool vampireUlt = false;
    float booRange = 1.0f;


    private void Start()
    {
        soundManager = GetComponent<SoundManager>();

        particleManager = GetComponent<ParticleManager>();

        pawn = GetComponent<Pawn>();
    }

    public void WerewolfUlt()
    {
        booRange = 10f;
        werewolfUlt = true;
    }

    public void WerewolfUltDone()
    {
        booRange = 1.0f;
        werewolfUlt = false;
    }

    public void VampireUlt()
    {
        booRange = 10f;
        vampireUlt = true;
    }

    public void VampireUltDone()
    {
        booRange = 1.0f;
        vampireUlt = false;
    }

    public void GoBoo(bool value)
    {
        if (value && canBoo)
        {
           // if (!soundManager.audioSource.isPlaying)
           // {
            Collider[] hitColliders = Physics.OverlapSphere(pawn.barrel.transform.position, booRange);
                
            for(int i = 0; i < hitColliders.Length; i++)
            {
                if(hitColliders[i].tag == "Player")
                {
                    if(hitColliders[i].transform.position != transform.position)
                    {
                        Pawn hitPlayerPawn = hitColliders[i].GetComponent<Pawn>();

                        if (werewolfUlt)
                        {
                            particleManager.wolfPart();
                            soundManager.WerewolfAttack();
                            hitPlayerPawn.myHealth.HitOof();
                            hitPlayerPawn.myHealth.TakeDamage(40f);
                            if(hitPlayerPawn.myMask.hasMask)
                            {
                                hitPlayerPawn.MyCandy.DropCandy(20);//if player doesn't have a mask, this already happends
                            }                            
                        }
                        else if(vampireUlt)
                        {                            
                            int candySuck = 20;
                            float healthSuck = 20f;
                            soundManager.VampireSlurp();
                            particleManager.vampPart();
                            if (hitPlayerPawn.myMask.hasMask)
                            {                                
                                if (hitPlayerPawn.myHealth.health < healthSuck)
                                {
                                    pawn.myHealth.health += hitPlayerPawn.myHealth.health;
                                    hitPlayerPawn.myHealth.health = 0;
                                }
                                else
                                {
                                    pawn.myHealth.health += healthSuck;
                                    hitPlayerPawn.myHealth.health -= healthSuck;
                                }
                            }

                            if(hitPlayerPawn.MyCandy.candy < candySuck)
                            {
                                pawn.MyCandy.candy += hitPlayerPawn.MyCandy.candy;
                                hitPlayerPawn.MyCandy.candy = 0;
                            }
                            else
                            {
                                pawn.MyCandy.candy += candySuck;
                                hitPlayerPawn.MyCandy.candy -= candySuck;
                            }
                        }
                        else
                        {
                            GameObject otherModel = hitColliders[i].GetComponent<Pawn>().myMask.currentModel;

                            float difference = otherModel.transform.rotation.eulerAngles.y - pawn.myMask.currentModel.transform.rotation.eulerAngles.y;

                            if (Mathf.Sign(difference) == -1)
                            {
                                difference *= -1;
                            }

                            if (difference <= radiusOfBoo)
                            {
                                if (hitColliders[i].GetComponent<Pawn>().myStun.stun == null)
                                {//Debug.Log("Got Booed");

                                    pawn._anim.SetTrigger("booTrigger");
                                    soundManager.Boo();
                                    particleManager.booPart();//shoots out boo and bat particles

                                    hitColliders[i].GetComponent<ParticleManager>().batPart();//stun particles circling bats
                                    hitColliders[i].GetComponent<ParticleManager>().dropPart();//drop candy particles

                                    HealthBar hb = hitColliders[i].GetComponent<HealthBar>();
                                    //hb.TakeDamage(damage);//for testing
                                    //StartCoroutine(hitColliders[i].GetComponent<Pawn>().myStun.suspendMovement(5f));
                                    hitColliders[i].GetComponent<Pawn>().myStun.StunPlayer(5f);
                                    hb.HitOof();

                                    pawn.myMask.SuccesfulBoo();
                                }
                            }



                            canBoo = false;
                            StartCoroutine(WaitingToBoo());

                        }
                    }
                }
            }                

           /* if (!werewolfUlt || !vampireUlt)
            {
                soundManager.Boo();
                particleManager.booPart();//shoots out boo and bat particles
                canBoo = false;
                StartCoroutine(WaitingToBoo());
            }*/
           // }            
        }
    }

    IEnumerator WaitingToBoo()
    {

        yield return new WaitForSeconds(3);
        canBoo = true;
    }
}
