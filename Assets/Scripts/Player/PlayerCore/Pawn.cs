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
    Mask myMask;
    
    [HideInInspector]
    public PlayerController MyController;

    protected virtual void Start ()
    {
        myMask = GetComponent<Mask>();
	}
	
	protected virtual void Update ()
    {
		
	}

    #region Input
    public virtual void PassLockScreen(bool value)
    {
        if (MyLookScript)
        {
            MyLookScript.lockState = value;
        }
    }

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
        if(value > 0.0f)
        {
            Debug.Log(name + " left trigger: " + value);
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
        myProjectileManager.throwEgg(value);

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
        if(value)
        {
            Debug.Log(name + " ultimate!");
            myMask.ultButton = true;
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
            MyInteractManager.TryToInteract(this);
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
