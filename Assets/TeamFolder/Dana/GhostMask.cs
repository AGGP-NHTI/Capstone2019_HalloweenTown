using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMask : MonoBehaviour{

    Mask myMask;
	void Start () {
        myMask = GetComponent<Mask>();
    }
	
	
	void Update () {

        if (myMask.ultButton && myMask.hasMask)
        {
            Color color = myMask.currentModel.GetComponent<Renderer>().material.color;
            color.a = 0.5f;
            myMask.currentModel.GetComponent<MeshRenderer>().material.color = color;
        }

    }
}
