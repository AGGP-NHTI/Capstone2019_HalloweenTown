using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ConsoleCommandParser : MonoBehaviour
{
    UnityEngine.UI.InputField _field;
    private void Awake()
    {
        _field = GetComponent<UnityEngine.UI.InputField>();
    }

    private void Update()
    {
        if (gameObject.activeSelf && _field)
        {
            if (!_field.IsActive())
            {
                SetFocus();
            }
        }
    }

    //Main Parser
    public void ParseCommand(string cmd)
    {
        if (cmd.EndsWith("`"))
        {
            _field.text = "";
            return;
        }
        cmd = cmd.ToLower();
        if (waitForResponseDelegate != null)
        {
            OnResponse(cmd);
        }
        else
        {
            string[] splitCmd = cmd.Split(' ');

            switch (splitCmd[0])
            {
                case "round":
                    {
                        Round(splitCmd);
                        break;
                    }
                case "console":
                    {
                        Console(splitCmd);
                        break;
                    }
                case "player":
                    {
                        Player(splitCmd);
                        break;
                    }
                case "getcontrollers":
                    {
                        string[] names = Input.GetJoystickNames();
                        string msg = "ConnectedControllers:\n{";
                        foreach(string controller in names)
                        {
                            msg += "\n\t" + controller;
                        }
                        msg += "\n}";
                        BuildConsole.WriteLine(msg);
                        break;
                    }
                case "quit":
                    {
                        BuildConsole.WriteLine("Are you sure you want to quit? Y / N");
                        waitForResponseMessage = "y";
                        waitForResponseDelegate = Application.Quit;
                        break;
                    }
                case "cursor":
                    {
                        Cursor(splitCmd);
                        break;
                    }

                case "display":
                    {
                        Display(splitCmd);
                        break;
                    }
                default:
                    {
                        BuildConsole.WriteLine("Invalid command");
                        break;
                    }
            }
        }

        _field.text = "";
        SetFocus();
    }

    #region Command specific methods
    [Header("For command \"round\"")]
    public GameObject RoundManagerPrefab;
    public GameObject NetworkedRoundManagerPrefab;
    protected void Round(string[] keywords)
    {
        RoundManager activeRM = FindObjectOfType<RoundManager>();
        if(keywords.Length < 2)
        {
            BuildConsole.WriteLine("Insufficient parameters");
            return;
        }

        switch(keywords[1])
        {
            case "time":
                {
                    if (keywords[2] == "get")
                    {
                        if(activeRM)
                        {
                            BuildConsole.WriteLine("Elapsed time: " + activeRM.roundElapsedTime);
                        }
                        else
                        {
                            BuildConsole.WriteLine("Round not active");
                        }
                        break;
                    }
                    else
                    {
                        float value;
                        if(float.TryParse(keywords[2], out value))
                        {
                            if (activeRM)
                            {
                                activeRM.roundRunningEndTime = value;
                                BuildConsole.WriteLine("Round time set to " + value);
                            }
                            else
                            {
                                if (RoundManagerPrefab)
                                {
                                    RoundManager rm = RoundManagerPrefab.GetComponent<RoundManager>();
                                    if (rm)
                                    {
                                        rm.roundRunningEndTime = value;
                                        BuildConsole.WriteLine("Round time set to " + value);
                                    }
                                }
                                if(NetworkedRoundManagerPrefab)
                                {
                                    RoundManager rm = NetworkedRoundManagerPrefab.GetComponent<RoundManager>();
                                    if (rm)
                                    {
                                        rm.roundRunningEndTime = value;
                                        BuildConsole.WriteLine("Round time set to " + value);
                                    }
                                }
                            }
                        }
                        else
                        {
                            BuildConsole.WriteLine("Expecting \'get\' or number as parameters, found neither");
                        }
                    }
                    break;
                }
            case "phase":
                {
                    if(!activeRM)
                    {
                        BuildConsole.WriteLine("Round not active");
                    }

                    if (keywords.Length < 2)
                    {
                        BuildConsole.WriteLine("Insufficient parameters");
                        return;
                    }

                    switch (keywords[2])
                    {
                        case "pre_game":
                            {
                                activeRM.currentPhase = RoundManager.RoundPhase.PRE_GAME;
                                BuildConsole.WriteLine("Set phase to pre_game");
                                break;
                            }
                        case "round_starting":
                            {
                                activeRM.currentPhase = RoundManager.RoundPhase.ROUND_STARTING;
                                BuildConsole.WriteLine("Set phase to round_starting");
                                break;
                            }
                        case "round_running":
                            {
                                activeRM.currentPhase = RoundManager.RoundPhase.ROUND_RUNNING;
                                BuildConsole.WriteLine("Set phase to round_running");
                                break;
                            }
                        case "round_ending":
                            {
                                activeRM.currentPhase = RoundManager.RoundPhase.ROUND_ENDING;
                                BuildConsole.WriteLine("Set phase to round_ending");
                                break;
                            }
                        case "round_over":
                            {
                                activeRM.currentPhase = RoundManager.RoundPhase.ROUND_OVER;
                                BuildConsole.WriteLine("Set phase to round_over");
                                break;
                            }
                        default:
                            {
                                BuildConsole.WriteLine("Invalid parameter. Expecting \'pre_game\', \'round_starting\', \'round_running\', \'round_ending\', or \'round_over\'");
                                break;
                            }
                    }
                    break;
                }
            default:
                {
                    BuildConsole.WriteLine("Invalid parameters for round");
                    break;
                }
        }
    }

    protected void Console(string[] keywords)
    {
        if(keywords.Length < 2)
        {
            BuildConsole.WriteLine("Insufficient parameters");
            return;
        }

        switch (keywords[1])
        {
            case "clear":
                {
                    BuildConsole.ClearLog();
                    break;
                }
        }
    }

    protected void Player(string[] keywords)
    {
        if (keywords.Length < 3)
        {
            BuildConsole.WriteLine("Insufficient parameters");
            return;
        }

        RoundManager activeRM = FindObjectOfType<RoundManager>();
        if (!activeRM)
        {
            BuildConsole.WriteLine("Round not active");
            return;
        }

        PlayerController[] activePlayers = activeRM.GetPlayers();

        if (keywords[1] == "all")
        {
            foreach(PlayerController pc in activePlayers)
            {
                switch (keywords[2])
                {
                    case "respawn":
                        {
                            int candy = 0;
                            if (pc.IsControllingPawn)
                            {
                                candy = pc.ControlledPawn.MyCandy.candy;
                                Destroy(pc.ControlledPawn.gameObject);
                            }
                            GameObject pawn = pc.ControlledPawn.gameObject;
                            SpawnPoint.GetRandomValidSpawn().SpawnPlayer(pc, activeRM.playerPrefab);
                            pc.ControlledPawn.MyCandy.candy = candy;
                            break;
                        }
                    case "candy":
                        {
                            if (keywords.Length < 5)
                            {
                                BuildConsole.WriteLine("Insufficient parameters");
                                return;
                            }
                            int amount;
                            if (!int.TryParse(keywords[4], out amount))
                            {
                                BuildConsole.WriteLine("Looking for integer, found " + keywords[4]);
                                return;
                            }
                            if (keywords[3] == "set")
                            {
                                pc.ControlledPawn.MyCandy.candy = amount;
                            }
                            else if (keywords[3] == "add")
                            {
                                pc.ControlledPawn.MyCandy.candy += amount;
                            }
                            else
                            {
                                BuildConsole.WriteLine("Invalid command. Looking for \'set\' or \'add\'");
                            }
                            break;
                        }
                    case "ammo":
                        {
                            if(keywords.Length < 6)
                            {
                                BuildConsole.WriteLine("Insufficent parameters");
                                return;
                            }
                            WeaponInventory inv = pc.ControlledPawn.GetComponent<WeaponInventory>();
                            int amount;
                            if (!int.TryParse(keywords[5], out amount))
                            {
                                BuildConsole.WriteLine("Looking for integer, found " + keywords[4]);
                                return;
                            }
                            if (keywords[4] == "set")
                            {
                                if(keywords[3] == "eggs" || keywords[3] == "egg")
                                {
                                    inv.numberEggs = amount;
                                    inv.UpdateDisplay();
                                }
                                else if(keywords[3] == "toiletpaper" || keywords[3] == "tp")
                                {
                                    inv.numberToiletPaper = amount;
                                    inv.UpdateDisplay();
                                }
                                else
                                {
                                    BuildConsole.WriteLine("Looking for \'eggs\' or \'toiletpaper\', found " + keywords[3]);
                                    return;
                                }
                            }
                            else if (keywords[4] == "add")
                            {
                                if (keywords[3] == "eggs" || keywords[3] == "egg")
                                {
                                    inv.numberEggs += amount;
                                    inv.UpdateDisplay();
                                }
                                else if (keywords[3] == "toiletpaper" || keywords[3] == "tp")
                                {
                                    inv.numberToiletPaper += amount;
                                    inv.UpdateDisplay();
                                }
                                else
                                {
                                    BuildConsole.WriteLine("Looking for \'eggs\' or \'toiletpaper\', found " + keywords[3]);
                                    return;
                                }
                            }
                            else
                            {
                                BuildConsole.WriteLine("Invalid command. Looking for \'set\' or \'add\'");
                            }
                            break;
                        }
                    case "showinput":
                        {
                            BuildConsole.WriteLine(pc.playerInput);
                            break;
                        }
                }
            }
        }
        else
        {
            int playerNum;
            if(int.TryParse(keywords[1], out playerNum))
            {
                playerNum--;
                if(0 > playerNum || playerNum >= activePlayers.Length)
                {
                    BuildConsole.WriteLine("There is not a player " + (playerNum + 1));
                    return;
                }

                switch(keywords[2])
                {
                    case "respawn":
                        {
                            int candy = 0;
                            if(activePlayers[playerNum].IsControllingPawn)
                            {
                                candy = activePlayers[playerNum].ControlledPawn.MyCandy.candy;
                                Destroy(activePlayers[playerNum].ControlledPawn.gameObject);
                            }
                            GameObject pawn = activePlayers[playerNum].ControlledPawn.gameObject;
                            SpawnPoint.GetRandomValidSpawn().SpawnPlayer(activePlayers[playerNum], activeRM.playerPrefab);
                            activePlayers[playerNum].ControlledPawn.MyCandy.candy = candy;
                            break;
                        }
                    case "candy":
                        {
                            if(keywords.Length < 5)
                            {
                                BuildConsole.WriteLine("Insufficient parameters");
                                return;
                            }
                            int amount;
                            if(!int.TryParse(keywords[4], out amount))
                            {
                                BuildConsole.WriteLine("Looking for integer, found " + keywords[4]);
                                return;
                            }
                            if(keywords[3] == "set")
                            {
                                activePlayers[playerNum].ControlledPawn.MyCandy.candy = amount;
                            }
                            else if(keywords[3] == "add")
                            {
                                activePlayers[playerNum].ControlledPawn.MyCandy.candy += amount;
                            }
                            else
                            {
                                BuildConsole.WriteLine("Invalid command. Looking for \'set\' or \'add\'");
                            }
                            break;
                        }
                    case "ammo":
                        {
                            if (keywords.Length < 6)
                            {
                                BuildConsole.WriteLine("Insufficent parameters");
                                return;
                            }
                            WeaponInventory inv = activePlayers[playerNum].ControlledPawn.GetComponent<WeaponInventory>();
                            int amount;
                            if (!int.TryParse(keywords[5], out amount))
                            {
                                BuildConsole.WriteLine("Looking for integer, found " + keywords[4]);
                                return;
                            }
                            if (keywords[4] == "set")
                            {
                                if (keywords[3] == "eggs" || keywords[3] == "egg")
                                {
                                    inv.numberEggs = amount;
                                    inv.UpdateDisplay();
                                }
                                else if (keywords[3] == "toiletpaper" || keywords[3] == "tp")
                                {
                                    inv.numberToiletPaper = amount;
                                    inv.UpdateDisplay();
                                }
                                else
                                {
                                    BuildConsole.WriteLine("Looking for \'eggs\' or \'toiletpaper\', found " + keywords[3]);
                                    return;
                                }
                            }
                            else if (keywords[4] == "add")
                            {
                                if (keywords[3] == "eggs" || keywords[3] == "egg")
                                {
                                    inv.numberEggs += amount;
                                    inv.UpdateDisplay();
                                }
                                else if (keywords[3] == "toiletpaper" || keywords[3] == "tp")
                                {
                                    inv.numberToiletPaper += amount;
                                    inv.UpdateDisplay();
                                }
                                else
                                {
                                    BuildConsole.WriteLine("Looking for \'eggs\' or \'toiletpaper\', found " + keywords[3]);
                                    return;
                                }
                            }
                            else
                            {
                                BuildConsole.WriteLine("Invalid command. Looking for \'set\' or \'add\'");
                            }
                            break;
                        }
                    case "showinput":
                        {
                            BuildConsole.WriteLine(activePlayers[playerNum].playerInput);
                            break;
                        }
                }
            }
            else
            {
                BuildConsole.WriteLine("Looking for player number");
                return;
            }
        }
    }

    protected void Cursor(string[] keywords)
    {
        if (keywords.Length < 2)
        {
            BuildConsole.WriteLine("Insufficient parameters");
            return;
        }

        switch(keywords[1])
        {
            case "lock":
                {
                    if(keywords.Length < 3)
                    {
                        if(UnityEngine.Cursor.lockState == CursorLockMode.None)
                        {
                            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                            BuildConsole.WriteLine("Cursor locked");
                        }
                        else
                        {
                            UnityEngine.Cursor.lockState = CursorLockMode.None;
                            BuildConsole.WriteLine("Cursor unlocked");
                        }
                    }
                    else
                    {
                        if(keywords[2] == "true")
                        {
                            UnityEngine.Cursor.lockState = CursorLockMode.Locked ;
                            BuildConsole.WriteLine("Cursor locked");
                        }
                        else if (keywords[2] == "false")
                        {
                            UnityEngine.Cursor.lockState = CursorLockMode.None;
                            BuildConsole.WriteLine("Cursor unlocked");
                        }
                        else
                        {
                            BuildConsole.WriteLine("Unexpected parameter \'" + keywords[2] + "\', expecting \'true\' or \'false\'");
                            return;
                        }
                    }
                    break;
                }
            case "visible":
                {
                    if (keywords.Length < 3)
                    {
                        UnityEngine.Cursor.visible = !UnityEngine.Cursor.visible;
                        if(UnityEngine.Cursor.visible)
                        {
                            BuildConsole.WriteLine("Cursor set to visible");
                        }
                        else
                        {
                            BuildConsole.WriteLine("Cursor set to not visible");
                        }
                    }
                    else
                    {
                        if (keywords[2] == "true")
                        {
                            UnityEngine.Cursor.visible = true;
                            BuildConsole.WriteLine("Cursor set to visible");
                        }
                        else if (keywords[2] == "false")
                        {
                            UnityEngine.Cursor.visible = false;
                            BuildConsole.WriteLine("Cursor set to not visible");
                        }
                        else
                        {
                            BuildConsole.WriteLine("Unexpected parameter \'" + keywords[2] + "\', expecting \'true\' or \'false\'");
                            return;
                        }
                    }
                    break;
                }
            default:
                {
                    BuildConsole.WriteLine("Invalid parameter. Expecting \'lock\' or \'visible\'");
                    break;
                }
        }
    }

    protected void Display(string[] keywords)
    {
        if(keywords.Length > 1)
        {
            switch(keywords[1])
            {
                case "roundmanager":
                    {
                        RoundManager activeRM = FindObjectOfType<RoundManager>();
                        if (!activeRM)
                        {
                            BuildConsole.WriteLine("Round not active");
                            return;
                        }

                        string msg = "{\n";
                        if(Photon.Pun.PhotonNetwork.IsMasterClient)
                        {
                            msg += "\tIs Master\n";
                        }
                        else
                        {
                            msg += "\tNot Master\n";
                        }
                        msg += activeRM.currentPhase + "\n\t" + activeRM.roundElapsedTime + "\n}";
                        BuildConsole.WriteLine(msg);
                        break;
                    }
                case "roomproperties":
                    {
                        if(PhotonManager.photonInstance)
                        {
                            string msg = "{";
                            foreach(DictionaryEntry kvp in PhotonManager.photonInstance.RoomProperties)
                            {
                                msg += "\n\t{ " + kvp.Key + " | " + kvp.Value + " }";
                            }
                            msg += "\n}";

                            BuildConsole.WriteLine(msg);
                        }
                        break;
                    }
            }
        }
    }
    #endregion

    #region Util
    protected string waitForResponseMessage;
    protected delegate void OnResponseMatch();
    protected OnResponseMatch waitForResponseDelegate;

    protected void OnResponse(string cmd)
    {
        if(cmd == waitForResponseMessage)
        {
            waitForResponseDelegate.Invoke();
        }
        waitForResponseDelegate = null;
        waitForResponseMessage = "";
    }

    protected void SetFocus()
    {
        _field.Select();
        _field.ActivateInputField();
        //EventSystem.current.SetSelectedGameObject(gameObject, null);
        //_field.OnPointerClick(new PointerEventData(EventSystem.current));
    }
    #endregion
}
