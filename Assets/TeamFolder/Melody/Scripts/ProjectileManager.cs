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

    public GameObject thrownObject;
    public float eggDamage = 10f;

    public bool showParabola = false;
    public float eggMagnitude = 100f;
    public float paperMagnitude = 500f;
    public float eggStep = 0.2f;
    public float paperStep = 0.4f;
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
        if (selectedWeaponIndex == 0)//egg
        {
            pawn.myParabola.parabolaMagnitude = eggMagnitude;
            pawn.myParabola.parabolaStepSize = eggStep;
        }
        else if (selectedWeaponIndex == 1)//paper
        {
            pawn.myParabola.parabolaMagnitude = paperMagnitude;
            pawn.myParabola.parabolaStepSize = paperStep;
        }

        if (showParabola)
        {    
            pawn.myParabola.ln.enabled = true;
            pawn.myParabola.DrawLine();
       }
       else
        {
            pawn.myParabola.ln.enabled = false;
        }

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
           // Debug.Log("inventory has no projectile" + inventory.hasProjectile(weapon));
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
            thrownObject = Instantiate(weaponList[selectedWeaponIndex], leftSpawn.position, model.rotation);
            Debug.Log(thrownObject.gameObject.name);
            thrownObject.GetComponent<Projectile>().owner = gameObject;
            inventory.subtractFromInventory(weaponList[selectedWeaponIndex]);
            
            if(thrownObject.GetComponent<ToiletPaper>())
            {
                //pawn.myParabola.DrawLine();
                thrownObject.GetComponent<ToiletPaper>().moveSpeed = rb.velocity;
                pawn.myParabola.parabolaMagnitude = paperMagnitude;
                thrownObject.GetComponent<ToiletPaper>().parabolaPoints.Clear();
                thrownObject.GetComponent<ToiletPaper>().parabolaPoints = pawn.myParabola.DrawLine();

            }
            else if(thrownObject.GetComponent<Egg>())
            {
                Egg.moveSpeed = rb.velocity;
                pawn.myParabola.parabolaMagnitude = eggMagnitude;
                thrownObject.GetComponent<Egg>().damage = eggDamage;
                thrownObject.GetComponent<Egg>().parabolaPoints = pawn.myParabola.DrawLine();
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
            inventory.UpdateDisplay();
            previousDPadY = currentDPadY;
    }
    //This is a mess
    /*private void updateLine()
    {
      

        Transform model = pawn.myMask.currentModel.transform;
        Transform leftSpawn = pawn.barrel.transform;
       

        float gravity = Physics.gravity.y;

        Vector3 velocityXZ = (leftSpawn.transform.forward * Egg.throwForce) +Egg.moveSpeed;
        Vector3 velocityY = (leftSpawn.transform.up * 2);
        Vector3 initVelocity = (velocityY + velocityXZ) * -Mathf.Sign(gravity);
        PlotTrajectory(leftSpawn.position, initVelocity, 10, 30);


        var t = Time.time;
        int sides = 30;
        float height = 1.0f;
         //float timeTotal = Mathf.Sqrt(-2* height/Physics.gravity) + Mathf.Sqrt(2* )

        for (int i = 0; i < sides; i++)
        {
           //float simulationTime = i / (float)sides * timeTotal
            
            // lr.SetPosition(i, new Vector3(i * 0.5f, Mathf.Sin(i + t), 0.0f));
        }


        lr.SetPosition(0, pawn.barrel.transform.position);
        lr.SetPosition(1, pawn.barrel.transform.forward * 10);
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;    
    }
    public void PlotTrajectory(Vector3 start, Vector3 startVelocity, float timeStep, float maxTime)
    {
        Vector3 previous = start;
        for (int i = 1; ;i++)
        {
            float t = timeStep * i;
            if (t > maxTime) break;
            Vector3 pos = PlotTrajectoryAtTime(start, startVelocity, t);
            if (Physics.Linecast(previous, pos)) break;
            Debug.DrawLine(previous, pos, Color.red);
            previous = pos;
        }

    }*/
}
