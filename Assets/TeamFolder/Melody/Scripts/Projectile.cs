using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Use this for initialization
    float destroyAfterElapsedTime = 10.0f;
    protected float elapsedTime = 0.0f;
    protected Rigidbody rb;
    protected void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected void Update () {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= destroyAfterElapsedTime)
        {
            Destroy(gameObject);
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        

        if (collision.gameObject.CompareTag("Player")) Debug.Log("colidded with player " );
        
        else Destroy(gameObject);
        
    }
}
