using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMask : BaseMask{

    //Mask myMask;
    Color color;
    public GameObject mask;
    bool up = true;
    public float yvalue;
    public LayerMask test;

    void Start () {
        base.Start();
        mask = pawn.myMask.currentModel;
        color = mask.GetComponent<MeshRenderer>().material.color;
        // myMask = GetComponent<Mask>();
        //color = pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color;
        yvalue = pawn.myMask.currentModel.transform.position.y;


    }

    void Update()
    {       
       /* if(pawn.myMask.currentModel.transform.position.y >= yvalue + 0.5f)
        {
            up = false;
        }
        else if(pawn.myMask.currentModel.transform.position.y <= yvalue)
        {
            up = true;
        }
        
        if(up)
        {
            // yvalue += Time.deltaTime;
            float newvalue = pawn.myMask.currentModel.transform.position.y + Time.deltaTime/2;
            pawn.myMask.currentModel.transform.position = new Vector3(pawn.myMask.currentModel.transform.position.x, newvalue, pawn.myMask.currentModel.transform.position.z);
        }
        else
        {
            float newvalue = pawn.myMask.currentModel.transform.position.y - Time.deltaTime/2;
            pawn.myMask.currentModel.transform.position = new Vector3(pawn.myMask.currentModel.transform.position.x, newvalue, pawn.myMask.currentModel.transform.position.z);
        }     */
            

    }


    public override void Ult()
    {               
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            pawn.soundMan.GhostUltScream();
            color.a = 0.5f;
            pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
            //pawn.myHealth.ghostUlt = true;

            GameObject[] playerCamera = GameObject.FindGameObjectsWithTag("MainCamera");

            foreach(GameObject pc in playerCamera)
            {
                if(!pc.transform.IsChildOf(gameObject.transform))
                {
                    pc.GetComponent<Camera>().cullingMask &=  ~(1 << LayerMask.NameToLayer("Ghost"));
                }
            }

            ulttimerCoroutine = StartCoroutine("UltTimer");
            
        }
    }

    public override void UltFinished()
    {
        pawn.myHealth.ghostUlt = false;
        color.a = 1f;
        pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
        GameObject[] playerCamera = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject pc in playerCamera)
        {
            if (!pc.transform.IsChildOf(gameObject.transform))
            {
                pc.GetComponent<Camera>().cullingMask |= 1 << LayerMask.NameToLayer("Ghost");
            }
        }
    }
    
}
