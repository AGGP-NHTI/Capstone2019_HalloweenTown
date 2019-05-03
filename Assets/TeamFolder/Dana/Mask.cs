using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
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

    public BaseMask equipedMask;
    Pawn pawn;
    ImageChange maskSprite;
    Vector3 savedPos;

    void Start()
    {
        
        pawn = gameObject.GetComponent<Pawn>();
        maskSprite = gameObject.GetComponentInChildren<ImageChange>();
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

        if (interactButton)
        {
            interactButton = false;
            UpdateHealth();

            if(hasMask)
            {                
                if(equipedMask.ulttimerCoroutine!=null)
                {
                    StopCoroutine(equipedMask.ulttimerCoroutine);
                }
                
                equipedMask.UltFinished();
                RemoveMask();
            }
            GetMask();            
        }
        
        if (pawn.myHealth.health <= 0 && hasMask)
        {
            if (equipedMask.ulttimerCoroutine != null)
            {
                StopCoroutine(equipedMask.ulttimerCoroutine);
            }
            hasMask = false;            
            Vector3 pos;
            //pos = currentModel.transform.position;
            /* if (gameObject.GetComponent<GhostMask>())
             {
                 pos = currentModel.transform.position;
                 pos.y = savedPos.y;
             }
             else
             {
                 pos = currentModel.transform.position;
             }*/

            Quaternion rot = currentModel.transform.rotation;
            Destroy(equipedMask);
            Destroy(currentModel);
            GameObject mask = PhotonNetwork.Instantiate(playerPref.name, gameObject.transform.position, gameObject.transform.rotation);
            //mask.transform.position = pos;
            mask.transform.rotation = rot;
            AlignToMovement al = mask.GetComponent<AlignToMovement>();
            al.TrackedRigidBody = gameObject.GetComponent<Rigidbody>();
            pawn.MyAlignToMovement = al;

            maskSprite.whiteCircle();
            currentModel = mask;
            pawn.MyMoveScript.anim = mask.GetComponent<Animator>();
            pawn.ModelChange();
            
            equipedMask = null;
        }
    }    

    public void SuccesfulBoo()//ult mulitplier; makes ult wait time shorter for each successful boo
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
        pawn.myHealth.health = 100;
    }

    void RemoveMask()
    {
        Destroy(equipedMask);
        maskSprite.whiteCircle();
        equipedMask = null;
    }
    
    void GetMask()
    {
            hasMask = true;
            GameObject mask = currentModel;
            //Vector3 pos = currentModel.transform.position;
            Quaternion rot = currentModel.transform.rotation;

            switch (scriptName)
            {
                case "Ghost Mask":
                    Destroy(currentModel);
                    mask = PhotonNetwork.Instantiate(ghostPref.name, gameObject.transform.position, gameObject.transform.rotation);
                maskSprite.ghost();
                    currentModel = mask;
                    equipedMask = gameObject.AddComponent<GhostMask>();
                    
                    break;
                case "Witch Mask":
                    equipedMask = gameObject.AddComponent<WitchMask>();
                    Destroy(currentModel);
                    mask = PhotonNetwork.Instantiate(witchPref.name, gameObject.transform.position, gameObject.transform.rotation);
                maskSprite.witch();

                    break;
                case "Werewolf Mask":
                    equipedMask = gameObject.AddComponent<WerewolfMask>();
                    Destroy(currentModel);
                    mask = PhotonNetwork.Instantiate(werewolfPref.name, gameObject.transform.position, gameObject.transform.rotation);
                maskSprite.werewolf();
                    break;
                case "Vampire Mask":
                    equipedMask = gameObject.AddComponent<VampireMask>();
                    Destroy(currentModel);
                    mask = PhotonNetwork.Instantiate(vampirePref.name, gameObject.transform.position, gameObject.transform.rotation);
                maskSprite.vampire();
                    break;

                default:
                    Debug.Log("Error with mask");
                    break;
            }

            //mask.transform.position = pos;
            mask.transform.rotation = rot;
            gameObject.transform.parent = mask.transform;
            AlignToMovement al = mask.GetComponent<AlignToMovement>();
            al.TrackedRigidBody = gameObject.GetComponent<Rigidbody>();
            pawn.MyAlignToMovement = al;
            currentModel = mask;

            pawn.ModelChange();       
    }
    
}
