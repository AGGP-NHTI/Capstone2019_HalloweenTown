using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public struct PlayerInfo
    {
        public string playerLayer;
        public Color32 outlineColor;

        public PlayerInfo(string layer, Color32 outline)
        {
            playerLayer = layer;
            outlineColor = outline;
        }
    }

    public static Dictionary<uint, PlayerInfo> PlayerInfoHolder = new Dictionary<uint, PlayerInfo>()
        {
            { 1, new PlayerInfo("Player1", new Color32(248,104,0,255)) },
            { 2, new PlayerInfo("Player2", new Color32(31,168,0,255)) },
            { 3, new PlayerInfo("Player3", new Color32(119,26,168,255)) },
            { 4, new PlayerInfo("Player4", new Color32(0,51,248,255)) }
        };

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
        DontDestroyOnLoad(this);
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

        _controlledPawn.MyController = this;

        if (_controlledPawn.MyCandy)
        {
            _controlledPawn.MyCandy.SetPlayerController(this);
        }

        int playerLayer = LayerMask.NameToLayer(PlayerInfoHolder[PlayerNumber].playerLayer);
        _controlledPawn.MyLayer = playerLayer;
        if(_controlledPawn.myBarrelController)
        {
            _controlledPawn.myBarrelController.Barrel.gameObject.layer = playerLayer;
        }
        if(_controlledPawn.MyCamera)
        {
            _controlledPawn.MyCamera.cullingMask |= (1 << playerLayer);
        }
        if(_controlledPawn.MyCameraManager)
        {
            _controlledPawn.MyCameraManager.SetCameraLayer(playerLayer);
        }

        Debug.Log("Player " + PlayerNumber + PlayerInfoHolder[PlayerNumber].outlineColor);
        _controlledPawn.color = PlayerInfoHolder[PlayerNumber].outlineColor;

        if (SplitScreenManager.Instance && _controlledPawn.MyCamera)
        {
            SplitScreenManager.Instance.PlayerCameras.Add(_controlledPawn.MyCamera);
            Debug.Log("Adding Camera" + PlayerNumber);
        }
    }

    protected virtual void OnLoseControl()
    {
        if (!_controlledPawn) { return; }

        if(_controlledPawn.MyController == this)
        {
            _controlledPawn.MyController = null;
        }

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
