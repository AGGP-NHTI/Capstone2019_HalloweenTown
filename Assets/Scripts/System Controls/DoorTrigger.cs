using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    float radius = 3.0f;
    public float CandyCollectionCoolDown = 3.0f;
<<<<<<< HEAD
    public int candyCount = 2;
=======
>>>>>>> e9fc774c89c28581da1979284394cc998d544e45

    void Start()
    {

    }

    public virtual void RecieveInteract(Pawn source, Interactable myInteractable)
    {
        //Make it so that the player can't interact and get more candy until they wait for a bit.
        myInteractable.PawnsThatCantInteract.Add(source);
        StartCoroutine(WaitingforCandy(source, myInteractable));
        
        //Give candy
        if(source.MyCandy)
        {
            source.MyCandy.candy++;
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