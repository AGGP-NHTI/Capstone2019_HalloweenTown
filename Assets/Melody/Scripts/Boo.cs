using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour {
    public AudioSource boo;
    bool canBoo = true;
    public GameObject barrel;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void GoBoo(bool value)
    {
        if (value && canBoo)
        {
            Debug.Log("Boo");
            if(!boo.isPlaying)
            {
                Collider[] hitColliders = Physics.OverlapSphere(barrel.transform.position, 1.0f);
                
                for(int i = 0; i < hitColliders.Length; i++)
                {
                    if(hitColliders[i].tag == "Player")
                    {
                        if(hitColliders[i].transform.position != transform.position)
                        {
                            Debug.Log("Hit Player");
                        }
                    }
                }

                boo.Play();
            }
            canBoo = false;
            StartCoroutine(WaitingToBoo());
        }
    }

    IEnumerator WaitingToBoo()
    {
        yield return new WaitForSeconds(3);
        canBoo = true;
        //Debug.Log(Time.time);
    }
}
