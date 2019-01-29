using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggControlScript : MonoBehaviour {

    public float speed = 10;
    public float throwForce = 20;
    public float arcAfterElapsedTime = 5;
    public float destroyAfterElapsedTime = 25; 
    float elapsedTime = 0;
    Rigidbody rb;
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce);
	}
	
	// Update is called once per frame
	void Update () {
        
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= arcAfterElapsedTime)
        {
            rb.useGravity = true;
        }
        //failsafe
        if (elapsedTime >= destroyAfterElapsedTime)
        {
            Destroy(gameObject);
        }
	}
    private void OnCollisionEnter(Collision collision)
    { 
        Destroy(gameObject);
    }
}
