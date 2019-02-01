using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMask : Mask{

    PlayerController pc;
	
	void Start () {
        pc = GetComponentInParent<PlayerController>();
    }
	
	
	void Update () {
                
		
	}
}
