﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

    public WeaponInventory inventory;
    float previousRightTrigger;
    float currentRightTrigger;
    float deadZone;
   
    public List<GameObject> weaponList;
    public int selectedWeaponIndex;
    public Transform leftSpawn;
    public Vector3 offset;
    Transform model;
    float currentDPadY;
    float previousDPadY;
    // Use this for initialization
    void Start() {
        //these warnings
      
       
        
        selectedWeaponIndex = 1;
        previousDPadY = currentDPadY = 0;
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
        //Debug.Log(string.Format("in throw egg. Value: {0}", value));
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
            GameObject.Instantiate(weaponList[selectedWeaponIndex], leftSpawn.position + leftSpawn.transform.forward, model.rotation);
            inventory.numberEggs--;
            inventory.UpdateEggCountDisplay();
            
        } 
        previousRightTrigger = currentRightTrigger;
        
    }
    public void cycleWeapon(Vector2 value)
    {
        
        currentDPadY = value.y;
        if (currentDPadY < -0.5f  && previousDPadY >= -0.5f)
        {
            if(selectedWeaponIndex == 0)
            {
                selectedWeaponIndex = weaponList.Count - 1;
            }
            else
            {
                selectedWeaponIndex -= 1;
            }
        }
        if (currentDPadY > 0.5f && previousDPadY <= 0.5f)
        {
            if (selectedWeaponIndex == weaponList.Count - 1)
            {
                selectedWeaponIndex = 0;
            }
            else
            {
                selectedWeaponIndex += 1;
            }
        }

            previousDPadY = currentDPadY;
    }
}
