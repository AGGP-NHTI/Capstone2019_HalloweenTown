using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy_Test : MonoBehaviour {

    public GameObject dropCandy;

    public Transform player;

    public int numCandy = 3;


    // Use this for initialization
    void Start () {

        

        for (int i = 0; i < numCandy; i++)
        {
            Instantiate(dropCandy, player.transform.position, Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
