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
    //public Transform leftSpawn;
    public Vector3 offset;
    float currentDPadY;
    float previousDPadY;
    public bool canThrow = true;
    public bool werewolfUlt = false;
    public LineRenderer lr;
    public bool debugLineOn = false; 
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
        
        //set up debug line Renderer
        if (GetComponent<LineRenderer>() && debugLineOn)
        {
            lr = GetComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.forward * 10);
        }
        //leftSpawn = ge
    }

    // Update is called once per frame
    void Update() {
        if (debugLineOn) updateLine();
    }

    public void throwObject(float value)
    {
        if(!pawn)
        {
            Debug.Log("No assigned pawn for " + name +"'s projectile manager");
            return;
        }
        if (!pawn.barrel)
        {
            Debug.Log("No assigned barrel  for pawn " + name);
            return;
        }
        Transform leftSpawn = pawn.barrel.transform;

        if(!pawn.myMask)
        {
            Debug.Log("No myMask for pawn " + name);
            return;
        }
        if (!pawn.myMask.currentModel)
        {
            Debug.Log("No currentModel for myMask " + name);
            return;
        }
        Transform model = pawn.myMask.currentModel.transform;

        //Debug.Log("in throw object");
        //Debug.Log(string.Format("in throw egg. Value: {0}", value));
        currentRightTrigger = value;
        //isthereathingtothrow
        GameObject weapon = weaponList[selectedWeaponIndex];
        if (!inventory.hasProjectile(weapon))
        {
            Debug.Log("inventory has no projectile" + inventory.hasProjectile(weapon));
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

    private void updateLine()
    {
        
        var t = Time.time;
        int sides = 30;
        for (int i = 0; i < sides; i++)
        {
           // lr.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f));
        }


        lr.SetPosition(0, pawn.barrel.transform.position);
        lr.SetPosition(1, pawn.barrel.transform.forward * 10);
    }
}
