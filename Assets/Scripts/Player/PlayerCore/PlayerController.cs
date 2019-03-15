using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputObject playerInput;
    public uint PlayerNumber
    {
        get
        {
            if(playerInput)
            {
                return playerInput.PlayerNumber;
            }
            else
            {
                return 0;
            }
        }
    }

    [SerializeField] protected Pawn _controlledPawn;
    public Pawn ControlledPawn
    {
        get { return _controlledPawn; }
        set
        {
            if (value != _controlledPawn)
            {
                OnLoseControl();
                _controlledPawn = value;
                if (value)
                {
                    OnGainControl();
                }
            }
        }
    }
    public bool IsControllingPawn { get { return _controlledPawn; } }

    private void Start()
    {
        if (_controlledPawn)
        {
            OnGainControl();
        }
    }

    protected virtual void Update()
    {
        if (ControlledPawn)
        {
            HandleInput();
        }
    }

    #region Control Change Events
    protected virtual void OnGainControl()
    {
        if (!_controlledPawn) { return; }

        _controlledPawn.PassLockScreen(true);
        if(_controlledPawn.MyCandy)
        {
            _controlledPawn.MyCandy.SetPlayerController(this);
        }

        if (SplitScreenManager.Instance && _controlledPawn.MyCamera)
        {
            SplitScreenManager.Instance.PlayerCameras.Add(_controlledPawn.MyCamera);
            Debug.Log("Adding Camera" + PlayerNumber);
        }
    }

    protected virtual void OnLoseControl()
    {
        if (!_controlledPawn) { return; }

        ControlledPawn.PassLockScreen(false);
        /*if (_controlledPawn.MyCandy)
        {
            _controlledPawn.MyCandy.SetPlayerController(this);
        }*/

        if (SplitScreenManager.Instance && _controlledPawn.MyCamera)
        {
            SplitScreenManager.Instance.PlayerCameras.Remove(_controlledPawn.MyCamera);
        }
    }
    #endregion

    #region Input
    protected virtual void HandleInput()
    {
        if (!(ControlledPawn && playerInput)) { return; }

        //Some inputs should not be processed if the game is "paused"
        if(Time.timeScale != 0.0f)
        {
            PassMoveInput(playerInput.GetMoveVector());
            PassLookInput(playerInput.GetLookVector());
            PassLeftTriggerInput(playerInput.GetLeftTrigger());
            PassRightTriggerInput(playerInput.GetRightTrigger());
            PassDPadInput(playerInput.GetDPadInput());
            PassUltimateInput(playerInput.GetUltimateInput());
            PassInteractInput(playerInput.GetInteractInput());
            PassJumpInput(playerInput.GetJumpInput());
            PassBooInput(playerInput.GetBooInput());
        }

        //These inputs should always be processed, even if the game is paused.
        PassStart(playerInput.GetStartInput());
        PassSelect(playerInput.GetSelectInput());
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

    protected virtual void PassLeftTriggerInput(float value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassLeftTriggerInput(value);
    }

    protected virtual void PassRightTriggerInput(float value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassRightTriggerInput(value);
    }

    protected virtual void PassDPadInput(Vector2 value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassDPadInput(value);
    }

    protected virtual void PassUltimateInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassUltimateInput(value);
    }

    protected virtual void PassInteractInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassInteractInput(value);
    }

    protected virtual void PassJumpInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassJumpInput(value);
    }

    protected virtual void PassBooInput(bool value)
    {
        if (!ControlledPawn) { return; }

        ControlledPawn.PassBooInput(value);
    }

    protected virtual void PassStart(bool value)
    {
        //Might not connect to pawn, not sure.
        //Will need to display on the camera of this pawn though, so maybe.
        if (value)
        {
            Debug.Log(name + " START");
        }
    }

    protected virtual void PassSelect(bool value)
    {
        //Might not connect to pawn, not sure.
        //Will need to display on the camera of this pawn though, so maybe.
        if (value)
        {
            Debug.Log(name + " SELECT");
        }
    }
    #endregion
}
