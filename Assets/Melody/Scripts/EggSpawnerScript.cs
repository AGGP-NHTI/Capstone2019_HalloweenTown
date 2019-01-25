using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawnerScript : MonoBehaviour {
    int spawnCounterStart = 60;
    int spawnCounterCurrent;
    float timer = 0.0f;
    int waitingTime = 3;
    public GameObject prefab;
    // Use this for initialization
	void Start () {
       
       spawnCounterCurrent = spawnCounterStart;
    }

    // Update is called once per frame
    void Update()
    {


        timer += Time.deltaTime;
        if (timer > waitingTime)
        {
            GameObject.Instantiate(prefab, transform.position, transform.rotation);
            timer = 0;
        }
    }
}
