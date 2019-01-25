using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour {
    public AudioSource boo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoBoo(bool value)
    {
        if (value)
        {
            Debug.Log("Boo");
            if(!boo.isPlaying)
            {
                boo.Play();
            }
        }
    }
}
