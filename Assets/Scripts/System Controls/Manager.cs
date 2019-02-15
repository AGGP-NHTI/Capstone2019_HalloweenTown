﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject RoundModePrefab;
    
    public List<InputObject> inputObject;
    [HideInInspector] public List<bool> joinedGame = new List<bool>();
    [HideInInspector] public List<bool> readyUp = new List<bool>();
    public float CountDownDuration { get; private set; }
    public bool RoundReadyToStart { get; private set; }
    
   // public Text countDown;
    Coroutine startGameTimer;

    protected bool startGame = true;
	void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //sets list of bools to false        
        for (int i = 0; i < 4; i++)
        {
            joinedGame.Add(false);
            readyUp.Add(false);
        }
	}
	
	void Update ()
    {
        if (startGame)
        {
            bool allPlayersReady = true;
            bool onePlayerInGame = false;

            for (int i = 0; i < inputObject.Count; i++)
            {         
                //test if Joined Game                
                if (inputObject[i].GetStartInput())
                {                    
                    joinedGame[i] = true;
                    if(startGameTimer != null)
                    {
                        StopCoroutine(startGameTimer);
                        
                        startGameTimer = null;
                    }
                    Debug.Log("Player " + i + " joined the game!");
                }
                
                //test if Ready Up
                if(inputObject[i].GetJumpInput() && joinedGame[i])
                {
                    readyUp[i] = true;
                    Debug.Log("Player " + i + " is ready!");
                }                
                
                if (joinedGame[i])
                {
                    onePlayerInGame = true;
                    if (!readyUp[i])
                    {
                        allPlayersReady = false;
                    }
                }                
            }

            //Debug.Log("oneplayer " + onePlayerInGame.ToString());
            RoundReadyToStart = allPlayersReady && onePlayerInGame;
            if (RoundReadyToStart)
            {                
                if (startGameTimer == null)
                {
                    Debug.Log("timer");
                    startGameTimer = StartCoroutine(BeginGameCountDown());
                }
            }
        }        
	}

    IEnumerator BeginGameCountDown()
    {      
        CountDownDuration = 5f; 
        float endTime = 0;
        while (CountDownDuration >= endTime)
        {
            //countDown.text = Mathf.Round(duration).ToString();
            Debug.Log(Mathf.Round(CountDownDuration).ToString());
            CountDownDuration -= Time.deltaTime;
            yield return null;
        }
        //countDown.text = Mathf.Round(Time.time).ToString();
        startGame = false;
        //This is when the game actually begins.
        if(RoundModePrefab)
        {
            GameObject spawnedObj = Instantiate(RoundModePrefab);
            RoundManager roundMode = spawnedObj.GetComponent<RoundManager>();

            if(roundMode)
            {
                List<InputObject> participatingPlayers = new List<InputObject>();
                for (int i = 0; i < inputObject.Count; i++)
                {
                    if (joinedGame[i] && readyUp[i])
                    {
                        participatingPlayers.Add(inputObject[i]);
                    }
                }
                roundMode.StartRound(participatingPlayers);
            }
            else
            {
                Debug.LogWarning(spawnedObj + " prefab has no RoundManager component!");
            }
        }
        else
        {
            Debug.LogWarning(name + " has no RoundModePrefab!");
        }
    }
}
