using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_ActivateOrderly : MonoBehaviour {

    public GameObject[] things;

	void Start ()
    {
		foreach(GameObject thing in things)
        {
            thing.SetActive(true);
        }
	}
}
