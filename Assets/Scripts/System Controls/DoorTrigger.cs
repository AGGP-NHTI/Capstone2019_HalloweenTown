using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    float radius = 3.0f;
    public float CandyCollectionCoolDown = 3.0f;
    public int candyCount;

    public ParticleSystem system;
    AudioSource trickOtreat;

    void Start()
    {
        trickOtreat = GetComponent<AudioSource>();
    }

    public virtual void RecieveInteract(Pawn source, Interactable myInteractable)
    {
        //Make it so that the player can't interact and get more candy until they wait for a bit.
        myInteractable.PawnsThatCantInteract.Add(source);
        StartCoroutine(WaitingforCandy(source, myInteractable));
        
        //Give candy
        if(source.MyCandy)
        {
            system.Play();
            trickOtreat.Play();
            if(source.myMask.hasMask)
            {
                candyCount = Random.Range(6,8);
                source.MyCandy.AddCandy(candyCount);
            }
            else
            {
                candyCount = Random.Range(2,4);
                source.MyCandy.AddCandy(candyCount);
            }            
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);//for testing
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player")
            {
                script = hitColliders[i].gameObject.GetComponent<Candy>();
                script.showXForCandy = true;
                if (script != null)
                {
                    if (dictionary.ContainsKey(script) == false)
                    {
                        if (script.actionButton == true)
                        {
                            script.candy++;
                            dictionary.Add(script, StartCoroutine(WaitingforCandy(script)));
                        }
                        script.actionButton = false;
                        
                    }

                    /*Debug.Log(hitColliders[i].gameObject.name);
                    if (script.waitforcandy == false)
                    {
                        script.candy++;
                        script.WaitForCandy();
                    }*//*
                }
                script.showXForCandy = false;
            }
           
            i++;
        }

    }*/

    IEnumerator WaitingforCandy(Pawn interacter, Interactable myInteractable)
    {
        yield return new WaitForSeconds(CandyCollectionCoolDown);
        myInteractable.PawnsThatCantInteract.Remove(interacter);
        //Debug.Log(Time.time);
    }
}