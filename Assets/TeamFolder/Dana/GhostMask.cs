using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMask : BaseMask{ //needs AI invincibility
    
    Color color;
    GameObject mask;
    protected int defaultLayer;

    void Start () {
        base.Start();
        mask = pawn.myMask.currentModel;
        //color = mask.GetComponent<MeshRenderer>().material.color;
        ultMultiplier = 10f;
    }

    public override void Ult()
    {               
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            pawn.GhostUlt = true;
            pawn.soundMan.GhostUltScream();
            //color.a = 0.5f;
            //pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
            //pawn.myHealth.ghostUlt = true;

            defaultLayer = pawn.myMask.currentModel.layer;
            pawn.myMask.currentModel.layer = pawn.MyLayer;

            ulttimerCoroutine = StartCoroutine("UltTimer");
            
        }
    }

    public override void UltFinished()
    {
        pawn.GhostUlt = false;
        pawn.myHealth.ghostUlt = false;
        //color.a = 1f;
        //pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;

        pawn.myMask.currentModel.layer = defaultLayer;
    }
    
}
