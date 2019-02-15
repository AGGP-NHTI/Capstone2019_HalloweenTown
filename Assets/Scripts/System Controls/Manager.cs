using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject RoundModePrefab;

    public List<InputObject> inputObject;
    /*[HideInInspector]*/ public List<bool> joinedGame = new List<bool>(new bool[] {false, false, false, false});
    /*[HideInInspector]*/ public List<bool> readyUp = new List<bool>(new bool[] { false, false, false, false });
    public float CountDownDuration { get; private set; }
    public bool RoundReadyToStart { get; private set; }
    
    Coroutine startGameTimer;

    protected bool lookForJoining = false;
	void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}
	
	void Update ()
    {
        if (lookForJoining)
        {
            bool allPlayersReady = true;
            bool onePlayerInGame = false;

            for (int i = 0; i < inputObject.Count; i++)
            {         
                //test if trying to Joined Game                
                if (inputObject[i].GetStartInput())
                {                    
                    joinedGame[i] = true;
                    Debug.Log("Player " + i + " joined the game!");
                }
                
                //test if trying to Ready Up
                if(inputObject[i].GetJumpInput() && joinedGame[i])
                {
                    readyUp[i] = true;
                    Debug.Log("Player " + i + " is ready!");
                }

                //test if trying to Unready/Unjoin
                if(inputObject[i].GetBooInput())
                {
                    if(readyUp[i])
                    {
                        readyUp[i] = false;
                    }
                    else if(joinedGame[i])
                    {
                        joinedGame[i] = false;
                    }
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
            else if(startGameTimer != null)
            {
                StopCoroutine(startGameTimer);

                startGameTimer = null;
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
        lookForJoining = false;
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

    public void StartLookingForPlayers()
    {
        lookForJoining = true;
        joinedGame = new List<bool>(new bool[] { false, false, false, false });
        readyUp = new List<bool>(new bool[] { false, false, false, false });
    }

    public void StopLookingForPlayers()
    {
        lookForJoining = false;

        if (startGameTimer != null)
        {
            StopCoroutine(startGameTimer);

            startGameTimer = null;
        }
    }
}
