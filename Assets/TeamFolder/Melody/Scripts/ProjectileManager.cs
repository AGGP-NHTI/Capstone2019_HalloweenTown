using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour {

   WeaponInventory inventory;
    float previousRightTrigger;
    float currentRightTrigger;
    float deadZone;
   
    public List<GameObject> weaponList;
    public int selectedWeaponIndex;
    public Transform leftSpawn;
    public Vector3 offset;
    public Transform model;
    float currentDPadY;
    float previousDPadY;
    public bool canThrow = true;
    public bool werewolfUlt = false;

    Pawn pawn;
    Rigidbody rb;
    void Start() {

        pawn = GetComponent<Pawn>();
        rb = GetComponent<Rigidbody>();

        inventory = GetComponent<WeaponInventory>();
        //leftSpawn = transform.Find("Model/LeftArm");
        
        selectedWeaponIndex = 0;
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

    public void throwObject(float value)
    {
        //Debug.Log("in throw object");
        //Debug.Log(string.Format("in throw egg. Value: {0}", value));
        currentRightTrigger = value;
        //isthereathingtothrow
        GameObject weapon = weaponList[selectedWeaponIndex];
        if (!inventory.hasProjectile(weapon))
        {
            //Debug.Log("inventory has projectile" + inventory.hasProjectile(weapon));
            return;
        }
        if(currentRightTrigger > deadZone && previousRightTrigger <= deadZone && canThrow)
        {
            Debug.Log("in egg");
            if (!leftSpawn)
            {
                Debug.LogWarning(name + "is trying to throw projectile no leftSpawn component assigned!");
                return;
            }
            if(!model)
            {
                Debug.LogWarning(name + "is trying to throw projectile but no model component not found!");
                return;
            }
            GameObject thrownObject = Instantiate(weaponList[selectedWeaponIndex], leftSpawn.position + leftSpawn.transform.forward, model.rotation);
            Debug.Log(thrownObject.gameObject.name);
            thrownObject.GetComponent<Projectile>().owner = gameObject;
            inventory.subtractFromInventory(weaponList[selectedWeaponIndex]);
            
            if(thrownObject.GetComponent<ToiletPaper>())
            {
                thrownObject.GetComponent<ToiletPaper>().moveSpeed += rb.velocity;


            }
            else if(thrownObject.GetComponent<Egg>())
            {
                thrownObject.GetComponent<Egg>().moveSpeed += rb.velocity;
                if(werewolfUlt)
                {
                    thrownObject.GetComponent<Egg>().damage *= 2;
                }
            }
            
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
