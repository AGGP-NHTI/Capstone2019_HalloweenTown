﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMask : BaseMask{

    //Mask myMask;
    Color color;
    
    void Start () {
       // myMask = GetComponent<Mask>();
        color = pawn.myMask.currentModel.GetComponent<Renderer>().material.color;        
    }
	

    public override void Ult()
    {        
        color.a = 0.5f;
        pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
        pawn.myHealth.ghostUlt = true;
        if (wait == null)
        {
            wait = StartCoroutine("BeginGameCountDown");
            pawn.myHealth.ghostUlt = false;
        }
    }

    public override void UltFinished()
    {
        color.a = 1f;
        pawn.myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
    }
    
}
