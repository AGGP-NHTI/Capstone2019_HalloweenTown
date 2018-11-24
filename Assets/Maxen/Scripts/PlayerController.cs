using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputObject playerInput;
    [SerializeField] protected Pawn _controlledPawn;
    private void Start()
    {
        if(_controlledPawn)
        {
            OnGainControl();
        }
    }

    public Pawn ControlledPawn
    {
        get { return _controlledPawn; }
        set
        {
            if(value != _controlledPawn)
            {
                OnLoseControl();
                _controlledPawn = value;
                if(value)
                {
                    OnGainControl();
                }
            }
        }
    }
    public bool IsControllingPawn { get { return _controlledPawn; } }

    protected virtual void Update ()
    {
        if(ControlledPawn)
        {
            HandleInput();
        }
	}

    #region Control Change Events
    protected virtual void OnGainControl()
    {
        ControlledPawn.PassFire1(true);
        if (SplitScreenManager.Instance && _controlledPawn.MyCamera)
        {
            SplitScreenManager.Instance.PlayerCameras.Add( _controlledPawn.MyCamera);
        }
    }

    protected virtual void OnLoseControl()
    {
        ControlledPawn.PassFire1(false);
        if (SplitScreenManager.Instance && _controlledPawn.MyCamera)
        {
            SplitScreenManager.Instance.PlayerCameras.Remove(_controlledPawn.MyCamera);
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
