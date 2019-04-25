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
        Vector2 moveVector = Vector2.zero;
        if (MoveHorizontalAxis != "")
        {
            moveVector.x = Input.GetAxis(MoveHorizontalAxis);
        }
        if (MoveVerticalAxis != "")
        {
            moveVector.y = Input.GetAxis(MoveVerticalAxis);
        }

        return moveVector;
    }

    public Vector2 GetLookVector()
    {
        return new Vector2(Input.GetAxis(LookHorizontalAxis), Input.GetAxis(LookVerticalAxis));
    }

    public float GetLeftTrigger()
    {
        if(LeftTriggerAxis == "")
        {
            return 0.0f;
        }
        return Mathf.Abs(Input.GetAxis(LeftTriggerAxis));
    }

    public float GetRightTrigger()
    {
        if (RightTriggerAxis == "")
        {
            return 0.0f;
        }
        return Mathf.Abs(Input.GetAxis(RightTriggerAxis));
    }

    public Vector2 GetDPadInput()
    {
        Vector2 dPadInput = Vector2.zero;
        if (DPadHorizontalAxis != "")
        {
            dPadInput.x = Input.GetAxis(DPadHorizontalAxis);
        }
        if (DPadVerticalAxis != "")
        {
            dPadInput.y = Input.GetAxis(DPadVerticalAxis);
        }
        return dPadInput;
    }

    public bool GetUltimateInput()
    {
        return Input.GetButtonDown(UltimateButton);
    }

    public bool GetInteractInput()
    {
        return Input.GetButton(InteractButton);
    }

    public bool GetJumpInput()
    {
        return Input.GetButton(JumpButton);
    }

    public bool GetBooInput()
    {
        return Input.GetButtonDown(BooButton);
    }

    public bool GetStartInput()
    {
        if(StartButton == "")
        {
            return false;
        }
        return Input.GetButtonDown(StartButton);
    }

    public bool GetSelectInput()
    {
        if(SelectButton == "")
        {
            return false;
        }
        return Input.GetButtonDown(SelectButton);
    }

    public override string ToString()
    {
        return "PlayerInput " + PlayerNumber +
            "\n{\n\tMoveHorizontalAxis: " + MoveHorizontalAxis +
            "\n\tMoveVerticalAxis: " + MoveVerticalAxis +
            "\n\tLookHorizontalAxis: " + LookHorizontalAxis +
            "\n\tLookVerticalAxis: " + LookVerticalAxis +
            "\n\tLeftTriggerAxis: " + LeftTriggerAxis +
            "\n\tRightTriggerAxis: " + RightTriggerAxis +
            "\n\tDPadHorizontalAxis: " + DPadHorizontalAxis +
            "\n\tDPadVerticalAxis: " + DPadVerticalAxis +
            "\n\tUltimateButton: " + UltimateButton +
            "\n\tInteractButton: " + InteractButton +
            "\n\tJumpButton: " + JumpButton +
            "\n\tBooButton: " + BooButton +
            "\n\tStartButton: " + StartButton +
            "\n\tSelectButton: " + SelectButton + "\n}";
    }
}
