using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candy : MonoBehaviour {

    public int candy = 0;
    public Text testcandy;
    public GameObject trigger;
    float radius = 3.0f;
    bool waitforcandy = false;
	void Start () {
        testcandy.text = "Candy: " + candy.ToString();
    }
	
	
	void Update () {

        Collider[] hitColliders = Physics.OverlapSphere(trigger.transform.position, radius);//for testing
        int i = 0;
        if (waitforcandy == false)
        {
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].gameObject == gameObject)
                {
                    waitforcandy = true;
                    
                    StartCoroutine(WaitingforCandy());

                    candy++;
                    testcandy.text = "Candy: " + candy.ToString();
                }
                i++;
            }
        }        
    }

    IEnumerator WaitingforCandy()
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(5);
        waitforcandy = false;
        Debug.Log(Time.time);
        /*Debug.Log("time " + time);
        if(time > 0)
        {
            time--;
            WaitingforCandy(time);
        }
        else
        {
            waitforcandy = false;
        }*/
    }
}
