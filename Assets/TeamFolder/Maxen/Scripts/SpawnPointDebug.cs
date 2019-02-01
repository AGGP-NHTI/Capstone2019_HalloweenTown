using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointDebug : MonoBehaviour {

    public bool saySpawnsInScene = false;

    public bool doSpawn = false;
    public PlayerController pc;
    public GameObject playerPrefab;
    public SpawnPoint spawn;

	void Update ()
    {
        if(saySpawnsInScene)
        {
            string msg = "Spawns in scene:\n";
            foreach(SpawnPoint sp in SpawnPoint.ActiveSpawns)
            {
                msg += sp.name + ", ";
            }
            msg = msg.Substring(0, msg.Length - 2) + ".";
            Debug.Log(msg);

            saySpawnsInScene = false;
        }

        if(doSpawn)
        {
            SpawnFella();

            doSpawn = false;
        }
    }

    void SpawnFella()
    {
        if(spawn)
        {
            Debug.Log("Spawning " + playerPrefab + " for " + pc + " at " + spawn + ".");
            spawn.SpawnPlayer(pc, playerPrefab);
        }
        else
        {
            SpawnPoint randomSpawn = SpawnPoint.GetRandomSpawn();
            if(randomSpawn)
            {
                Debug.Log("Spawning " + playerPrefab + " for " + pc + " at " + randomSpawn + ".");
                randomSpawn.SpawnPlayer(pc, playerPrefab);
            }
            else
            {
                Debug.Log("Could not spawn " + playerPrefab + " because there were no spawnpoints in the scene.");
            }
        }
    }
}
