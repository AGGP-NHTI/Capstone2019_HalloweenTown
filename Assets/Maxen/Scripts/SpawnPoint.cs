using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public static List<SpawnPoint> ActiveSpawns = new List<SpawnPoint>();

    protected bool isValid;

    public static SpawnPoint GetRandomSpawn()
    {
        if (ActiveSpawns.Count <= 0)
        {
            return null;
        }

        return ActiveSpawns[Random.Range(0, ActiveSpawns.Count)];
    }

    public static SpawnPoint GetRandomValidSpawn()
    {
        return null;
    }

    protected virtual void Awake()
    {
        ActiveSpawns.Add(this);
    }

    protected virtual void OnDestroy()
    {
        ActiveSpawns.Remove(this);
    }

    /*public virtual bool CheckValidity()
    {
        Physics.OverlapSphere(transform.position);
    }*/

    public virtual void SpawnPlayer(PlayerController pc, GameObject pawnPrefab)
    {
        GameObject spawnedGameObject = Instantiate(pawnPrefab, transform.position, transform.rotation);
        isValid = false;

        Pawn spawnedPawn = spawnedGameObject.GetComponent<Pawn>();
        if (spawnedPawn)
        {
            pc.ControlledPawn = spawnedPawn;
        }
        else
        {
            Debug.LogWarning(name + " attempted to spawn prefab " + pawnPrefab.name + " that has no Pawn component!");
        }
    }
}
