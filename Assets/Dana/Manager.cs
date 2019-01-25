using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {
        
    Pawn pawn;
    //public List<GameObject> spawnpts;
    public List<InputObject> io;
    List<bool> joinedGame = new List<bool>();
    List<bool> readyUp = new List<bool>();


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
        yield return new WaitForSeconds(5);
        startGame = false;        
    }
}
