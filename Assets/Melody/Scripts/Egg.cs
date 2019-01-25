using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

    WeaponInventory inventory;
    // Use this for initialization
    void Start() {
        inventory = GetComponent<WeaponInventory>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void throwEgg(float value)
    {
        if ( value > 0 && inventory.numberEggs > 0)
        {
            //spawn egg

            //decrement inventory.numbereggs
            inventory.numberEggs--;
            inventory.UpdateEggCountDisplay();
        }
        
    }
}
