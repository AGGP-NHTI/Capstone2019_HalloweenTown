using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Use this for initialization
    float destroyAfterElapsedTime = 10.0f;
    protected float elapsedTime = 0.0f;
    protected Rigidbody rb;
    public GameObject owner;

    Pawn pawn;
    
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
        if(owner)
        {
            Debug.Log("owner: " + owner.name);
        }
    }    
}
