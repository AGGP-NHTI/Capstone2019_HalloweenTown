using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaper : Projectile {

    
    public float throwForce;
    public Vector3 moveSpeed;

    Vector3 throwAngle;
    float stunTime = 5f;
    // Use this for initialization
    void Start () {
        base.Start();
        throwAngle = (transform.forward  + transform.up).normalized * throwForce;
        throwAngle += moveSpeed;
        rb.velocity = throwAngle;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {            
            GameObject player = collision.gameObject;            
            Stun stun = player.GetComponent<Stun>();

            if (stun.stunned == false)
            {
                stun.StunPlayer(stunTime);
                    //StartCoroutine(stun.suspendMovement(stunTime));
            }           
        }
        Destroy(gameObject);
    }
}
