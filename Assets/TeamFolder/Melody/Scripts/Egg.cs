using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Projectile {

   
    public float throwForce = 20;
    public float arcAfterElapsedTime = 5;
    public float damage = 10.0f;
    // Use this for initialization
	void Start () {
        base.Start();
        rb.AddForce(transform.forward * throwForce);
	}

    // Update is called once per frame
    new void Update () {

        base.Update();
        if (elapsedTime >= arcAfterElapsedTime)
        {
            rb.useGravity = true;
        }
        
        
	}
    new void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
