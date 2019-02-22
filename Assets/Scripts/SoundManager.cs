using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip boo;
    public AudioClip oof;
    public AudioClip witchScream;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Oof()
    {
        audioSource.clip = oof;
        audioSource.Play();
    }

    public void Boo()
    {
        audioSource.clip = boo;
        audioSource.Play();
    }

    public void WitchUltScream()
    {
        audioSource.clip = witchScream;
        audioSource.Play();
    }
}
