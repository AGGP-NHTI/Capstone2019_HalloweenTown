using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candy : MonoBehaviour
{

    public int candy = 0;
    public Text candyText;
    float radius = 3.0f;
    public bool waitforcandy = false;
    public bool actionButton;
    public bool showXForCandy;

    void Start()
    {
        if (candyText)
        {
            candyText.text = " " + candy.ToString();
        }
        actionButton = false;
        showXForCandy = false;
    }


    void Update()
    {
        if(candyText)
        {
            candyText.text = " " + candy.ToString();
        }
    }
    
}