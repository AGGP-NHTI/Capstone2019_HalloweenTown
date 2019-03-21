using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public bool GhostUlt = false;

    [HideInInspector]
    public PlayerController MyController;

    private void Awake()
    {
        barrel = myMask.currentModel.GetComponent<GetBarrel>().barrel;
    }
    protected virtual void Start ()
    {
        myMask = GetComponent<Mask>();
        myProjectileManager = GetComponent<ProjectileManager>();
        
    }
    
    public void ModelChange()
    {
        barrel = myMask.currentModel.GetComponent<GetBarrel>().barrel;
        myParticle.booParticles = myMask.currentModel.GetComponent<GetBarrel>().boopartical;
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
                MyCameraManager.SetVirtualCamera(1);
                if(MyAlignToMovement)
                {
                    MyAlignToMovement.useOverrideForward = true;
                    MyAlignToMovement.overrideForward = MyCamera.transform.forward;
                }
            }
            else
            {
                MyCameraManager.SetVirtualCamera(0);
                if(MyAlignToMovement)
                {
                    MyAlignToMovement.useOverrideForward = false;
                }
            }
        }
        
    }

    public virtual void PassRightTriggerInput(float value)
    {
        if (value > 0.0f)
        {
            Debug.Log(name + " right trigger: " + value);
        }
        if (!myProjectileManager)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Projectile Manager component assigned!");
            return;
        }

        myProjectileManager.throwObject(value);
    }

    public virtual void PassDPadInput(Vector2 value)
    {
        if (value != Vector2.zero)
        {
            Debug.Log(name + " dPad: " + value);
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

        if(value)
        {
            MyInteractManager.TryToInteract();
        }

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
