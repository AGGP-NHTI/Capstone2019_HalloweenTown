using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandmaSpawn : MonoBehaviour {



    public GameObject grandma;
    public static int? spawnIndex = null;
    public bool isSpawn;

    static List<GrandmaSpawn> spawns = new List<GrandmaSpawn>();

    public void addSelf()
    {
        spawns.Add(this);
    }

    public void Awake()
    {
        addSelf();
    }

    public static void selectSpawn()
    {
        if(GrandmaSpawn.spawnIndex != null)
        {
            return;
        }
        Debug.Log("selecting spawnIndex");
        spawnIndex = Random.Range(0, spawns.Count - 1);
        Debug.Log(string.Format("spawns.count - 1: {0}; spawnIndex: {1}",spawns.Count - 1, spawnIndex));
        spawns[(int)spawnIndex].isSpawn = true;
    }

    void Start ()
    {
        selectSpawn();
        if (isSpawn)
        {
            Instantiate(grandma, transform.position, transform.rotation);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	   	
	}


}
