using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candy : MonoBehaviour
{

    public int candy = 0;
    public Text testcandy;
    float radius = 3.0f;
    public bool waitforcandy = false;
    public bool actionButton;
    bool test = true;

    void Start()
    {
        testcandy.text = "Candy: " + candy.ToString();
        actionButton = false;
    }


    void Update()
    {
        testcandy.text = "Candy: " + candy.ToString();
        /*
        if(waitforcandy)
        {
            
                testcandy.text = "Candy: " + candy.ToString();
                StartCoroutine(WaitingforCandy());
                test = false;
            
            
        }
        /*if (waitforcandy == false)
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
        } */
    }
    
}