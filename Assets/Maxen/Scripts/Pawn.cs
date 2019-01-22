using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public MoveScript MyMoveScript;
    public LookScript MyLookScript;
    public Camera MyCamera;
    public Candy MyCandy;

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

    public virtual void PassLeftTriggerInput(bool value)
    {

    }

    public virtual void PassRightTriggerInput(bool value)
    {

    }

    public virtual void PassDPadInput(Vector2 value)
    {

    }

    public virtual void PassUltimateInput(bool value)
    {

    }

    //x button
    public virtual void PassInteractInput(bool value)
    {
        if(!MyCandy)
        {
            Debug.LogWarning(name + " is trying to be passed input when it has no Candy component assigned!");
        }

        if(value)
        {
            MyCandy.IWantCandy();
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

    }
    #endregion
}
