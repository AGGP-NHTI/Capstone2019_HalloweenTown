using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggControlScript : MonoBehaviour {

    public float speed;
    // Use this for initialization
	void Start () {
       // GetComponent<Rigidbody>().AddForce(transform.forward * force);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * speed * transform.forward;
	}
}
