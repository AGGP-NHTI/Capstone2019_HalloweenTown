using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    float startHealth;
    public bool hasMask = false;
    public bool ultButton = false;
    public bool interactButton;
    Pawn pawn;
    HealthBar hbar;
    public GameObject playerPref;
    public GameObject ghostPref;
    public GameObject currentModel;
    public string scriptName;
    GameObject mask;

    BaseMask equipedMask;

    void Start()
    {
        pawn = gameObject.GetComponent<Pawn>();
        hbar = GetComponent<HealthBar>(); 
    }


    void Update()
    {
        if (ultButton && hasMask)
        {
            equipedMask.Ult();
        }

        if (interactButton && hasMask == false)
        {
            interactButton = false;
            GetMask();
            UpdateHealth();
        }        
        
        if (hbar.health <= 0 && hasMask)
        {
            hasMask = false;
            Debug.Log("dead");
            Vector3 pos = currentModel.transform.position;
            Quaternion rot = currentModel.transform.rotation;
            Destroy(currentModel);
            GameObject mask = Instantiate(playerPref, gameObject.transform);
            mask.transform.position = pos;
            mask.transform.rotation = rot;
            AlignToMovement al = mask.GetComponent<AlignToMovement>();
            al.TrackedRigidBody = gameObject.GetComponent<Rigidbody>();

            currentModel = mask;
            
        }
    }    

    void UpdateHealth()
    {
        hbar.health = 100;
    }

    void GetMask()
    {
        hasMask = true;        

        Vector3 pos = currentModel.transform.position;
        Quaternion rot = currentModel.transform.rotation;

        switch(scriptName)
        {
            case "Ghost Mask":
                equipedMask = gameObject.AddComponent<GhostMask>();
                Destroy(currentModel);
                mask = Instantiate(ghostPref, gameObject.transform);
                break;
            case "Werewolf Mask":
                break;
            default:
                Debug.Log("Error with mask");
                break;
        }  

        mask.transform.position = pos;
        mask.transform.rotation = rot;
        gameObject.transform.parent = mask.transform;
        AlignToMovement al = mask.GetComponent<AlignToMovement>();
        al.TrackedRigidBody = gameObject.GetComponent<Rigidbody>();
        currentModel = mask;

        pawn.MyBoo.ModelChange();
    }

    
}
