using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMask : BaseMask{

    Mask myMask;
    Color color;
    
    void Start () {
        myMask = GetComponent<Mask>();
        color = myMask.currentModel.GetComponent<Renderer>().material.color;
    }
	

    public override void Ult()
    {        
        color.a = 0.5f;
        myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
        if (wait == null)
        {
            wait = StartCoroutine("BeginGameCountDown");            
        }
    }

    public override void UltFinished()
    {
        color.a = 1f;
        myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
    }
    
}
