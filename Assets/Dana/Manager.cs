using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public InputObject io;
    Pawn pawn;
    public List<PlayerController> pc;
    public List<GameObject> spawnpts;

    bool startGame = true;
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
        if (startGame)
        {
            for (int i = 0; i <= pc.Count; i++)
            {
                //pc[i].playerInput.
            }
        }
	}
}
