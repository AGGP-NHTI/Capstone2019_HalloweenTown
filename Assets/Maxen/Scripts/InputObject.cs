using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Input Object", menuName = "InputObject")]

public class InputObject : ScriptableObject
{
    public string MoveHorizontalAxis = "Horizontal";
    public string MoveVerticalAxis = "Vertical";
    public string LookHorizontalAxis = "Mouse X";
    public string LookVerticalAxis = "Mouse Y";
    public string JumpButton = "Jump";

    public Vector2 GetMoveVector()
    {
        return new Vector2(Input.GetAxis(MoveHorizontalAxis), Input.GetAxis(MoveVerticalAxis));
    }

    public Vector2 GetLookVector()
    {
        return new Vector2(Input.GetAxis(LookHorizontalAxis), Input.GetAxis(LookVerticalAxis));
    }

    public bool GetJumpInput()
    {
        return Input.GetButton(JumpButton);
    }
}
