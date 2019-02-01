using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperController : Projectile {

    
    public float throwForce;

    Vector3 throwAngle;
    // Use this for initialization
	void Start () {
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
