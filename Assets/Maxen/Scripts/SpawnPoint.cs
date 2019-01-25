using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public static List<SpawnPoint> ActiveSpawns = new List<SpawnPoint>();

    protected bool isValid;
    public float spawnValidityCheckRadius = 2.0f;
    public LayerMask spawnValidityCheckLayerMask;

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
        if (ActiveSpawns.Count <= 0)
        {
            return null;
        }

        List<SpawnPoint> validSpawns = new List<SpawnPoint>();
        foreach(SpawnPoint sp in ActiveSpawns)
        {
            if(sp.CheckValidity())
            {
                validSpawns.Add(sp);
            }
        }

        if(validSpawns.Count <= 0)
        {
            return GetRandomSpawn();
        }

        return validSpawns[Random.Range(0, validSpawns.Count)];
    }

    protected virtual void Awake()
    {
        ActiveSpawns.Add(this);
    }

    private void OnEnable()
    {
        if(!ActiveSpawns.Contains(this))
        {
            ActiveSpawns.Add(this);
        }
    }

    private void OnDisable()
    {
        ActiveSpawns.Remove(this);
    }

    protected virtual void OnDestroy()
    {
        ActiveSpawns.Remove(this);
    }

    public virtual bool CheckValidity()
    {
        isValid = !Physics.CheckSphere(transform.position, spawnValidityCheckRadius, spawnValidityCheckLayerMask, QueryTriggerInteraction.UseGlobal);

        return isValid;
    }

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