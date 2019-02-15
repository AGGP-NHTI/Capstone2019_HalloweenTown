﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public bool hasMask = false;
    public bool ultButton = false;
    public bool interactButton;
    public string scriptName;

    public GameObject currentModel;
    public GameObject playerPref;
    public GameObject ghostPref;
    public GameObject witchPref;
    public GameObject werewolfPref;
    public GameObject vampirePref;
    GameObject mask;

    BaseMask equipedMask;
    Pawn pawn;
    HealthBar hbar;

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
            case "Witch Mask":
                equipedMask = gameObject.AddComponent<WitchMask>();
                Destroy(currentModel);
                mask = Instantiate(ghostPref, gameObject.transform);
                break;
            case "Werewolf Mask":
                equipedMask = gameObject.AddComponent<WerewolfMask>();
                Destroy(currentModel);
                mask = Instantiate(werewolfPref, gameObject.transform);
                break;
            case "Vampire Mask":
                equipedMask = gameObject.AddComponent<VampireMask>();
                Destroy(currentModel);
                mask = Instantiate(vampirePref, gameObject.transform);
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

        pawn.ModelChange();
    }

    
}
