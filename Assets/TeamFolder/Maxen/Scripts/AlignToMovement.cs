using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToMovement : MonoBehaviour
{
    public Rigidbody TrackedRigidBody;
    public bool TrackX = true;
    public bool TrackY = true;
    public bool TrackZ = true;

    void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        if(!TrackedRigidBody) { return; }

        Vector3 newDirection = TrackedRigidBody.velocity.normalized;
        if (!TrackX) { newDirection.x = 0.0f; }
        if (!TrackY) { newDirection.y = 0.0f; }
        if (!TrackZ) { newDirection.z = 0.0f; }
        transform.forward = Vector3.Slerp(transform.forward, newDirection, 0.25f);
    }
}
