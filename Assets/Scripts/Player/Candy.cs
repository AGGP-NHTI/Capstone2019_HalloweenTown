﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candy : MonoBehaviour
{
    public static Dictionary<PlayerController, int> Scoreboard;

    public int candy = 0;
    public Text candyText;
    float radius = 3.0f;
    public bool waitforcandy = false;
    public bool actionButton;
    public bool showXForCandy; 
    public GameObject candyPrefab;
    protected PlayerController _myController;

    PlaceImage placement;

    void Start()
    {
        placement = gameObject.GetComponentInChildren<PlaceImage>();

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

    public void CandySuck(int candySuck)
    {

    }

    public void DropCandy(int numCandy = 0)
    {
        if (numCandy == 0)
        {
            if (candy > 20)
            {
                numCandy = Random.Range(5, 20);
            }
            else if (candy > 0)
            {
                numCandy = Random.Range(1, candy);
            }
        }
        else
        {
            if(candy < numCandy)// can set amount of candy player drops
            {
                numCandy = candy;
            }
        }

        if (candy > 0)
        {
            for (int i = 0; i < numCandy; i++)
            {
                Vector3 vel = Random.onUnitSphere;
                vel.y = Mathf.Abs(vel.y);
                Vector3 pos = new Vector3(0f, 1f, 0f) + vel;
                vel *= 5;

                GameObject candy;

                candy = Instantiate(candyPrefab, transform.position + pos, transform.rotation);
                candy.GetComponent<Rigidbody>().velocity = vel;
            }
        }

        AddCandy(numCandy * -1);
    }

    public void AddCandy(int amount)
    {
        candy += amount;
        Scoreboard[_myController] = candy;
    }

    public void SetPlayerController(PlayerController pc)
    {
        if(Scoreboard == null)
        {
            Scoreboard = new Dictionary<PlayerController, int>();
        }

        _myController = pc;
        if(Scoreboard.ContainsKey(pc))
        {
            Scoreboard[pc] = candy;
            
        }
        else
        {
            Scoreboard.Add(pc, candy);
            
        }
    }
}