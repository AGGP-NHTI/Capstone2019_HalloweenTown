using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtempo : MonoBehaviour {

	public AudioSource asource;
    public float pitch = 0.001f;
	void Start () {
        StartCoroutine(pitchChange());
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator pitchChange()
    {
        while(asource.pitch <2)
        {
            asource.pitch += Time.deltaTime * pitch;
            yield return null;
        }
    }
}
