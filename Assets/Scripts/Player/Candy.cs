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
    public bool showXForCandy;
    public Text xForCandyText;

    void Start()
    {
        if (testcandy)
        {
            testcandy.text = "Candy: " + candy.ToString();
        }
        actionButton = false;
        showXForCandy = false;
    }


    void Update()
    {
        if(testcandy)
        {
            testcandy.text = "Candy: " + candy.ToString();
        }

        if(xForCandyText)
        {
            if (showXForCandy)
            {
                xForCandyText.enabled = true;
            }
            else
            {
                xForCandyText.enabled = false;
            }
        }
    }
    
}