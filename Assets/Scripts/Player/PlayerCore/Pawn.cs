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
    public Egg myEgg;
    
    [HideInInspector]
    public PlayerController MyController;

    protected virtual void Start ()
    {
		
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
        if (!myEgg)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Egg component assigned!");
            return;
        }
        myEgg.throwEgg(value);

    }

    public virtual void PassDPadInput(Vector2 value)
    {
        if (value != Vector2.zero)
        {
            Debug.Log(name + " dPad: " + value);
        }

        if (!myEgg)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Egg component assigned!");
            return;
        }
        myEgg.cycleWeapon(value);
    }

    public virtual void PassUltimateInput(bool value)
    {
        if(value)
        {
            Debug.Log(name + " ultimate!");
        }
    }

    //x button
    public virtual void PassInteractInput(bool value)
    {
        if(!MyCandy)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Candy component assigned!");
            return;
        }
        
        MyCandy.actionButton = value;

        if (value)
        {
            Debug.Log(name + " interact!");
        }
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
