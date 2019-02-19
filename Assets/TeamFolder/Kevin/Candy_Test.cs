using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy_Test : MonoBehaviour {

    public GameObject dropCandy;

    public Transform player;

    public ParticleSystem system;

    // Use this for initialization
    void Start () {

        DropCandy(30);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DropCandy(int numCandy)
    {
        for (int i = 0; i < numCandy; i++)
        {
            system.Play();

            Vector3 pos = new Vector3(0f, 2f, 0f);

            GameObject candy;

            candy = Instantiate(dropCandy, transform.position + pos, transform.rotation);

            
        }

    }
}
