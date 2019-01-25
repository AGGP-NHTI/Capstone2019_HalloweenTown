using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Input Object", menuName = "InputObject")]

public class InputObject : ScriptableObject
{
    public uint PlayerNumber;

    public string MoveHorizontalAxis = "Horizontal";
    public string MoveVerticalAxis = "Vertical";
    public string LookHorizontalAxis = "Mouse X";
    public string LookVerticalAxis = "Mouse Y";
    public string LeftTriggerAxis;
    public string RightTriggerAxis;
    public string DPadHorizontalAxis;
    public string DPadVerticalAxis;
    public string UltimateButton;
    public string InteractButton;
    public string JumpButton = "Jump";
    public string BooButton;
    public string StartButton;
    public string SelectButton;

    public Vector2 GetMoveVector()
    {
        return new Vector2(Input.GetAxis(MoveHorizontalAxis), Input.GetAxis(MoveVerticalAxis));
    }

    public Vector2 GetLookVector()
    {
        return new Vector2(Input.GetAxis(LookHorizontalAxis), Input.GetAxis(LookVerticalAxis));
    }

    public bool GetLeftTrigger()
    {
        //NOT YET IMPLEMENTED
        return false;
    }

    public bool GetRightTrigger()
    {
        //NOT YET IMPLEMENTED
        return false;
    }

    public Vector2 GetDPadInput()
    {
        //NOT YET IMPLEMENTED
        return Vector2.zero;
    }

    public bool GetUltimateInput()
    {
        //NOT YET IMPLEMENTED
        return false;
    }

    public bool GetInteractInput()
    {
        //NOT YET IMPLEMENTED
        return false;
    }

    public bool GetJumpInput()
    {
        return Input.GetButton(JumpButton);
    }

    public bool GetBooInput()
    {
        //NOT YET IMPLEMENTED
        return false;
    }

    public bool GetStartInput()
    {
        //NOT YET IMPLEMENTED
        return false;
    }

    public bool GetSelectInput()
    {
        //NOT YET IMPLEMENTED
        return false;
    }
}
