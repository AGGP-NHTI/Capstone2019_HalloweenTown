using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Pawn : MonoBehaviour
{
    public MoveScript MyMoveScript;
    public LookScript MyLookScript;
    public Camera MyCamera;
    public Candy MyCandy;
    public Boo MyBoo;
    public ProjectileManager myProjectileManager;
    public InteractionManager MyInteractManager;
    public Mask myMask;
    public HealthBar myHealth;
    public Stun myStun;
    public GameObject barrel;
    public SoundManager soundMan;
    public ParticleManager myParticle;
    public PlayerCamManager MyCameraManager;
    public AlignToMovement MyAlignToMovement;
    public LayerMask MyLayer;
    public Color color;
    public SkinnedMeshRenderer outlineMesh;
    public SkinnedMeshRenderer solidMesh;
    public ParabolaTemp myParabola;
    public bool GhostUlt = false;
    public Animator _anim;
    public GameObject LhandBagSpawn;
    public PlayerBagSpawner bagSpawner;
    public BarrelController myBarrelController;
    [HideInInspector]
    public PlayerController MyController;

    public MonoBehaviour[] MonobehavioursToToggleOnControlChange;
    public GameObject[] GameObjectsToToggleOnControlChange;

    private void Awake()
    {
        //myMask.currentModel.GetComponent<GetBarrel>().witchBroom.SetActive(false);
        barrel = myMask.currentModel.GetComponent<GetBarrel>().barrel;
        LhandBagSpawn = myMask.currentModel.GetComponent<GetBarrel>().Lhand;
        _anim = myMask.currentModel.GetComponent<Animator>();
    }
    protected virtual void Start ()
    {
        myMask = GetComponent<Mask>();
        myProjectileManager = GetComponent<ProjectileManager>();
        //Debug.Log(outlineMesh.materials[0].name.ToString());
       outlineMesh.materials[0].SetColor("_OutlineColor", color);
        solidMesh.material.SetColor("_OutlineColor", color);
        myParabola = gameObject.GetComponent<ParabolaTemp>();
    }
    
    public void ModelChange()
    {
        myBarrelController.DefaultAnchor = myMask.currentModel.GetComponent<GetBarrel>().barrel.transform;
        LhandBagSpawn = myMask.currentModel.GetComponent<GetBarrel>().Lhand;
        //bagSpawner.myBag.transform.SetParent(LhandBagSpawn.transform, false);
        gameObject.GetPhotonView().RPC("NetworkSetParent", RpcTarget.AllBuffered);
        _anim = myMask.currentModel.GetComponent<Animator>();

        myParticle.booParticles = myMask.currentModel.GetComponent<GetBarrel>().boopartical;
        MyMoveScript.anim = myMask.currentModel.GetComponent<Animator>();

        outlineMesh = myMask.currentModel.GetComponent<GetBarrel>().outlineMesh;
        solidMesh = myMask.currentModel.GetComponent<GetBarrel>().solidMesh;

        outlineMesh.materials[0].SetColor("_OutlineColor", color);
        solidMesh.material.SetColor("_OutlineColor", color);
    }

    public virtual void ToggleComponents()
    {
        Debug.Log("Toggling");
        foreach(GameObject obj in GameObjectsToToggleOnControlChange)
        {
            Debug.Log("toggling " + obj.name + " from " + obj.activeSelf);
            obj.SetActive(!obj.activeSelf);
            Debug.Log("toggling " + obj.name + " to " + obj.activeSelf);
        }
        foreach (MonoBehaviour obj in MonobehavioursToToggleOnControlChange)
        {
            Debug.Log("toggling " + obj.name + " from " + obj.enabled);
            obj.enabled = !obj.enabled;
            Debug.Log("toggling " + obj.name + " to " + obj.enabled);
        }

    }

    #region Input
    public virtual void PassMoveInput(Vector2 value)
    {
        if(!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed movement input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.MoveHorizontal(value.x);
        MyMoveScript.MoveVertical(value.y);
    }

    public virtual void PassLookInput(Vector2 value)
    {
        if(!MyLookScript)
        {
            Debug.LogWarning(name + " is trying to be passed look input when it has no LookScript component assigned!");
            return;
        }

        MyLookScript.MouseInput = value;
    }

    public virtual void PassLeftTriggerInput(float value)
    {
        
        if(MyCameraManager)
        {
            if (value > 0.0f)
            {
                myProjectileManager.showParabola = true;
                MyCameraManager.SetVirtualCamera(1);
                _anim.SetBool("aimBool", true);
                if(MyAlignToMovement)
                {
                    MyAlignToMovement.useOverrideForward = true;
                    MyAlignToMovement.overrideForward = MyCamera.transform.forward;
                }
                if(myBarrelController)
                {
                    myBarrelController.IsAiming = true;
                }
            }
            else
            {
                _anim.SetBool("aimBool", false);
                myProjectileManager.showParabola = false;
                MyCameraManager.SetVirtualCamera(0);
                if(MyAlignToMovement)
                {
                    MyAlignToMovement.useOverrideForward = false;
                }
                if (myBarrelController)
                {
                    myBarrelController.IsAiming = false;
                }
            }
        }
        
    }

    public virtual void PassRightTriggerInput(float value)
    {
        if (value > 0.0f)
        {
            //Debug.Log(name + " right trigger: " + value);
        }
        if (!myProjectileManager)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Projectile Manager component assigned!");
            return;
        }

        myProjectileManager.throwObject(value);
        _anim.SetTrigger("throwTrigger");
    }

    public virtual void PassDPadInput(Vector2 value)
    {
        if (value != Vector2.zero)
        {
            //Debug.Log(name + " dPad: " + value);
        }

        if (!myProjectileManager)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Egg component assigned!");
            return;
        }
        myProjectileManager.cycleWeapon(value);
    }

    public virtual void PassUltimateInput(bool value)
    {
        myMask.ultButton = value;
        if (value)
        {
           // Debug.Log(name + " ultimate!");            
        }
    }

    //x button
    public virtual void PassInteractInput(bool value)
    {
        if(!MyInteractManager)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no InteractionManager component assigned!");
            return;
        }

        MyInteractManager.TryToInteract(value);

        /*if(!MyCandy)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Candy component assigned!");
            return;
        }
        
        MyCandy.actionButton = value;

        if (value)
        {
            Debug.Log(name + " interact!");
        }*/
    }

    public virtual void PassJumpInput(bool value)
    {
        if (!MyMoveScript)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no MoveScript component assigned!");
            return;
        }

        MyMoveScript.Jump(value);
    }

    public virtual void PassBooInput(bool value)
    {
        if (!MyBoo)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Boo component assigned!");
            return;

        }
        MyBoo.GoBoo(value);
    }
    #endregion
}
