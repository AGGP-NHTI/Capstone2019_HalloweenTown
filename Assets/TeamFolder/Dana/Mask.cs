using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour {

    bool hasMask = false;
    HealthBar hbar;
    PlayerController pc;

	void Start () {
        hbar = GetComponent<HealthBar>();
        pc = GetComponent<PlayerController>();
	}
	
	
	void Update () {

        if (pc.playerInput.GetSelectInput())
        {
            //gameObject.AddComponent(GhostMask)
        }

        if (hasMask)
        {
            
            
        }
	}

    void UpdateHealth()
    {
        hbar.health += 50;
        hasMask = true;
    }

    List<InputObject> GetInputs()
    {
        List<InputObject> inputObject = new List<InputObject>();


        return inputObject;
    }
}
