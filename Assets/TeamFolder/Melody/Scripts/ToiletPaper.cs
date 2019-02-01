﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaper : Projectile {

    
    public float throwForce = 400;

    Vector3 throwAngle;
    // Use this for initialization
	new void Start () {
        base.Start();
        
        throwAngle = (transform.forward + transform.up).normalized * throwForce;
        
        rb.AddForce(throwAngle);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    new void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
