using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggControlScript : MonoBehaviour {

    public float speed = 2;
    public float arcAfterElapsedTime = 5;
    float elapsedTime = 0;
    // Use this for initialization
	void Start () {
       // GetComponent<Rigidbody>().AddForce(transform.forward * force);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Time.deltaTime * speed * transform.forward;
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= arcAfterElapsedTime)
        {
            transform.position -= new Vector3(0, .02f, 0);
        }
	}
}
