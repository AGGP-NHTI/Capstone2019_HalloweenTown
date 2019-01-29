using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour {
        
    public GameObject playercontroller;
    public GameObject playerPrefab;
    public List<InputObject> io;
    List<bool> joinedGame = new List<bool>();
    List<bool> readyUp = new List<bool>();
   // public Text countDown;
    
    Coroutine timer;

    bool startGame = true;
	void Start () {
        //sets list of bools to false
        
        for(int i = 0; i < 4; i++)
        {
            joinedGame.Add(false);
            readyUp.Add(false);
        }
		
	}
	
	void Update () {
        if (startGame)
        {
            bool allPlayersReady = true;
            bool onePlayerInGame = false;

            for (int i = 0; i < io.Count; i++)
            {         
                //test if Joined Game                
                if (io[i].GetStartInput())
                {
                    
                    joinedGame[i] = true;
                    Debug.Log("Player " + i + " joined the game!");
                }
                
                //test if Ready Up
                if(io[i].GetJumpInput() && joinedGame[i])
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
            Debug.Log("oneplayer " + onePlayerInGame.ToString());
            if(allPlayersReady && onePlayerInGame)
            {
                
                if (timer == null)
                {
                    Debug.Log("timer");
                    timer = StartCoroutine(BeginGameCountDown());
                }
            }
            
        }
        
	}

    IEnumerator BeginGameCountDown()
    {
        startGame = false;

        float duration = 5f; 
        float endTime = 0;
        while (duration>= endTime)
        {
            //countDown.text = Mathf.Round(duration).ToString();
            duration -= Time.deltaTime;
            yield return null;
        }
        
        //countDown.text = Mathf.Round(Time.time).ToString();
        
        SpawnPlayers();
        
    }

    void SpawnPlayers()
    {
        //Instantiate(playercontroller, Vector3.zero, Quaternion.identity);
        //SpawnPoint.ActiveSpawns[0].SpawnPlayer;
        for(int i = 0; i< io.Count; i++)
        {
            if(joinedGame[i] && readyUp[i])
            {
                GameObject spawnedBoy = Instantiate(playercontroller, Vector3.zero, Quaternion.identity);
                spawnedBoy.GetComponent<PlayerController>().playerInput = io[i];

                SpawnPoint.GetRandomValidSpawn().SpawnPlayer(spawnedBoy.GetComponent<PlayerController>(), playerPrefab);
            }

        }

        SplitScreenManager.Instance.ConfigureScreenSpace();
    }
}
