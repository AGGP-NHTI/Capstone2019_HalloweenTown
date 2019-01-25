using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

    WeaponInventory inventory;
    float previousRightTrigger;
    float currentRightTrigger;
    float deadZone;
    public GameObject eggPrefab;
    public Transform leftSpawn;
    Transform model;
    // Use this for initialization
    void Start() {
        inventory = GetComponent<WeaponInventory>();
        previousRightTrigger = currentRightTrigger = 0;
        deadZone = 0;
        foreach(Transform c in transform)
        {
            if (c.name == "Model")
            {
                model = c;
            }
        }
      
        //leftSpawn = ge
    }

    // Update is called once per frame
    void Update() {

    }

    public void throwEgg(float value)
    {
        Debug.Log(string.Format("in throw egg. Value: {0}", value));
        currentRightTrigger = value;
        if(currentRightTrigger > deadZone && previousRightTrigger <= deadZone && inventory.numberEggs > 0)
        {
            if(!leftSpawn)
            {
                Debug.LogWarning(name + "is trying to throw egg no leftSpawn component assigned!");
                return;
            }
            if(!model)
            {
                Debug.LogWarning(name + "is trying to throw egg no model component not found!");
                return;
            }
            GameObject.Instantiate(eggPrefab, leftSpawn.position, model.rotation);
            inventory.numberEggs--;
            inventory.UpdateEggCountDisplay();
            
        }
        previousRightTrigger = currentRightTrigger;
        
    }
}
