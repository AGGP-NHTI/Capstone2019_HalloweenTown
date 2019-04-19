using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomsHere : MonoBehaviour {

    public Animation momVan;
    public Animation gateOpen;

    // Use this for initialization
    void Start () {
        momVan.Play();
        gateOpen.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
