using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Image ultBar;

    BaseMask equipedMask;
    Pawn pawn;
    HealthBar hbar;

    Vector3 savedPos;

    void Start()
    {
        pawn = gameObject.GetComponent<Pawn>();
        hbar = GetComponent<HealthBar>();
        savedPos = currentModel.transform.position;
    }

    void Update()
    {
        if (equipedMask == null)
        {
            ultBar.fillAmount = 0;
        }
        else
        {
            ultBar.fillAmount = equipedMask.ultTimeFloat / 100;
        }
        
        
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
            Vector3 pos;
            if (gameObject.GetComponent<GhostMask>())
            {
                pos = savedPos;
            }
            else
            {
                pos = currentModel.transform.position;
            }
            
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

    public void SuccesfulBoo()
    {
        float booUltAdd = 20f;
        if(equipedMask != null && equipedMask.isUlting == false)
        {
            if(equipedMask.ultTimeFloat >= 100-booUltAdd)
            {
                equipedMask.ultTimeFloat = 100f;
            }
            else
            {
                equipedMask.ultTimeFloat += booUltAdd;
            }
        }
    }

    void UpdateHealth()
    {
        hbar.health = 100;
    }

    void GetMask()
    {
        if (!hasMask)
        {
            hasMask = true;
            GameObject mask = currentModel;
            Vector3 pos = currentModel.transform.position;
            Quaternion rot = currentModel.transform.rotation;

            switch (scriptName)
            {
                case "Ghost Mask":
                    Destroy(currentModel);
                    mask = Instantiate(ghostPref, gameObject.transform);
                    currentModel = mask;
                    equipedMask = gameObject.AddComponent<GhostMask>();

                    break;
                case "Witch Mask":
                    equipedMask = gameObject.AddComponent<WitchMask>();
                    Destroy(currentModel);
                    mask = Instantiate(witchPref, gameObject.transform);
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

    
}
