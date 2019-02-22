using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMask : BaseMask{

    //Mask myMask;
    Color color;
    public GameObject mask;


    void Start () {
        base.Start();
        mask = pawn.myMask.currentModel;
        color = mask.GetComponent<MeshRenderer>().material.color;
        // myMask = GetComponent<Mask>();
        //color = pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color;



    }
	

    public override void Ult()
    {        
        color.a = 0.5f;
        pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
        pawn.myHealth.ghostUlt = true;
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            ulttimerCoroutine = StartCoroutine("UltTimer");
            pawn.myHealth.ghostUlt = false;
        }
    }

    public override void UltFinished()
    {        
        color.a = 1f;
        pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
    }
    
}
