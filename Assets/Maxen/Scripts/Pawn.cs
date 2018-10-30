using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    public MoveScript MyMoveScript;
    public LookScript MyLookScript;

    [HideInInspector]
    public PlayerController MyController;

    protected virtual void Start ()
    {
		
	}
	
	protected virtual void Update ()
    {
		
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

    public virtual void PassFire1(bool value)
    {
        if(MyLookScript)
        {
            MyLookScript.lockState = value;
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
}
