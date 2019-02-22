using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // Use this for initialization
    float destroyAfterElapsedTime = 10.0f;
    protected float elapsedTime = 0.0f;
    protected Rigidbody rb;
    public GameObject owner;
    public Vector3 velocityY = Vector3.zero;
    public Vector3 velocityXZ = Vector3.zero;

    Pawn pawn;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void Start () {

       
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
    protected void addVelocity()
    {
        Debug.Log("velocityy: " + velocityY);
        rb.velocity = velocityY + velocityXZ;
    }
}
