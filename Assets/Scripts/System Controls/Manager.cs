using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public RoundManager roundMode;
    public GameObject playercontroller;
    public GameObject playerPrefab;
    public List<InputObject> inputObject;
    List<bool> joinedGame = new List<bool>();
    List<bool> readyUp = new List<bool>();
    [HideInInspector]public List<PlayerController> activePlayers;
   // public Text countDown;
    Coroutine startGameTimer;

    bool startGame = true;
	void Start ()
    {
        //sets list of bools to false        
        for(int i = 0; i < 4; i++)
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
            if(allPlayersReady && onePlayerInGame)
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
        float duration = 5f; 
        float endTime = 0;
        while (duration>= endTime)
        {
            //countDown.text = Mathf.Round(duration).ToString();
            Debug.Log(Mathf.Round(duration).ToString());
            duration -= Time.deltaTime;
            yield return null;
        }
        //countDown.text = Mathf.Round(Time.time).ToString();
        startGame = false;
        SpawnPlayers();        
    }

    void SpawnPlayers()
    {
        activePlayers = new List<PlayerController>();

        for(int i = 0; i< inputObject.Count; i++)
        {
            if(joinedGame[i] && readyUp[i])
            {
                GameObject spawnedBoy = Instantiate(playercontroller, Vector3.zero, Quaternion.identity);
                PlayerController pc = spawnedBoy.GetComponent<PlayerController>();
                pc.playerInput = inputObject[i];

                SpawnPoint.GetRandomValidSpawn().SpawnPlayer(pc, playerPrefab);

                activePlayers.Add(pc);
            }
        }

        SplitScreenManager.Instance.ConfigureScreenSpace();
    }
}
