using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperController : MonoBehaviour {

    Rigidbody rb;
    public float throwForce;

    Vector3 throwAngle;
    // Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        
        throwAngle = (transform.forward + transform.up).normalized * throwForce;
        
        rb.AddForce(throwAngle);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
