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
    public bool xForCandy;
    public Text xforcandy;

    void Start()
    {
        testcandy.text = "Candy: " + candy.ToString();
        actionButton = false;
        xForCandy = false;
    }


    void Update()
    {
        testcandy.text = "Candy: " + candy.ToString();

        if (xForCandy)
        {
            xforcandy.enabled = true;
        }
        else
        {
            xforcandy.enabled = false;
        }
    }
    
}