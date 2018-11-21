using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputObject playerInput;
    public Pawn ControlledPawn;                 //This is the main Pawn var for this script, use this one.
    protected Pawn _previouslyControlledPawn;   //This one is used to check and update the ControlChangeEvents. DO NOT use this one.
    public bool IsControllingPawn { get { return ControlledPawn; } }

    protected virtual void Update ()
    {
        CheckControlChangeEvent();

        if(ControlledPawn)
        {
            HandleInput();
        }
	}

    #region Control Change Events
    protected virtual void CheckControlChangeEvent()
    {
        if (ControlledPawn && !_previouslyControlledPawn)
        {
            OnGainControl();
            _previouslyControlledPawn = ControlledPawn;
        }
        else if (!ControlledPawn && _previouslyControlledPawn)
        {
            OnLoseControl();
            _previouslyControlledPawn = null;
        }
    }

    protected virtual void OnGainControl()
    {
        ControlledPawn.PassFire1(true);
        if (SplitScreenManager.Instance && ControlledPawn.MyCamera)
        {
            SplitScreenManager.Instance.PlayerCameras.Add( ControlledPawn.MyCamera);
        }
    }

    protected virtual void OnLoseControl()
    {
        _previouslyControlledPawn.PassFire1(false);
        if(SplitScreenManager.Instance && _previouslyControlledPawn.MyCamera)
        {
            SplitScreenManager.Instance.PlayerCameras.Remove(ControlledPawn.MyCamera);
        }
    }
    #endregion

    #region Input
    protected virtual void HandleInput()
    {
        if(!(ControlledPawn && playerInput)) { return; }

        PassMoveInput(playerInput.GetMoveVector());
        PassLookInput(playerInput.GetLookVector());
        PassJumpInput(playerInput.GetJumpInput());
    }

    protected virtual void PassMoveInput(Vector2 value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassMoveInput(value);
    }

    protected virtual void PassLookInput(Vector2 value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassLookInput(value);
    }

    protected virtual void PassJumpInput(bool value)
    {
        ControlledPawn.PassJumpInput(value);
    }
    #endregion
}
