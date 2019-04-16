using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementImage : MonoBehaviour {

    public Pawn player;

    int myPlayerScore;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        myPlayerScore = Candy.Scoreboard[player.MyController];

        foreach (KeyValuePair<PlayerController, int> candyTotal in Candy.Scoreboard)
        {  
        }

        

    }
}
