using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour {
    
    public int radiusOfBoo = 60;
    public float damage = 10f;
    bool canBoo = true;
    Pawn pawn;
    SoundManager soundManager;



    ParticleManager particleManager;

    private void Start()
    {
        soundManager = GetComponent<SoundManager>();

        particleManager = GetComponent<ParticleManager>();

        pawn = GetComponent<Pawn>();
    }

    public void GoBoo(bool value)
    {
        if (value && canBoo)
        {
            Debug.Log("Boo");
            if (!soundManager.audioSource.isPlaying)
            {
                Collider[] hitColliders = Physics.OverlapSphere(pawn.barrel.transform.position, 1.0f);
                
                for(int i = 0; i < hitColliders.Length; i++)
                {
                    if(hitColliders[i].tag == "Player")
                    {
                        if(hitColliders[i].transform.position != transform.position)
                        {
                            GameObject otherModel = hitColliders[i].GetComponent<Pawn>().myMask.currentModel;
                            float difference = otherModel.transform.rotation.eulerAngles.y - pawn.myMask.currentModel.transform.rotation.eulerAngles.y;

                            //checks if the difference is a negative value
                            if(Mathf.Sign(difference) == -1)
                            {
                                difference *= -1;
                            }

                            if (difference <= radiusOfBoo)
                            {
                                Debug.Log("Got Booed");
                                hitColliders[i].GetComponent<ParticleManager>().batPart();//stun particles circling bats
                                hitColliders[i].GetComponent<ParticleManager>().dropPart();//drop candy particles
                                HealthBar hb = hitColliders[i].GetComponent<HealthBar>();
                                //hb.TakeDamage(damage);//for testing
                                StartCoroutine(hitColliders[i].GetComponent<Pawn>().myStun.suspendMovement(5f));
                                hb.Hit();//makes oof sound
                                hitColliders[i].GetComponent<Pawn>().MyCandy.DropCandy();
                                pawn.myMask.SuccesfulBoo();
                            }                            
                        }
                    }
                }
                soundManager.Boo();
                particleManager.booPart();//shoots out boo and bat particles
                canBoo = false;
                StartCoroutine(WaitingToBoo());
            }            
        }
    }

    IEnumerator WaitingToBoo()
    {
        yield return new WaitForSeconds(3);
        canBoo = true;
    }
}
