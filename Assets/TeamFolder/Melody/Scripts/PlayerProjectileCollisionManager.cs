using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileCollisionManager : MonoBehaviour {
    public HealthBar myHealthBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Egg"))
        {
            Debug.Log("I've been hit by an egg!");
            float d = collision.gameObject.GetComponent<Egg>().damage;
            myHealthBar.TakeDamage(d);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Toilet Paper"))
        {
            Debug.Log("I've been hit by an Toilet Paper!");
            Destroy(collision.gameObject);
        }
        //else Destroy(gameObject);
    }
}
