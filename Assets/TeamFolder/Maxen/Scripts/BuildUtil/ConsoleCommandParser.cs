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
        if(gameObject.activeSelf && _field)
        {
            if(!_field.IsActive())
            {
                SetFocus();
            }
        }
    }

    //Main Parser
    public void ParseCommand(string cmd)
    {
        cmd = cmd.ToLower();
        string[] splitCmd = cmd.Split(' ');

        switch(splitCmd[0])
        {
            case "round":
                {
                    round(splitCmd);
                    break;
                }
            default:
                {
                    BuildConsole.WriteLine("Invalid command");
                    break;
                }
        }

        _field.text = "";
        SetFocus();
    }

    //Command Specific functions
    [Header("For command \"round\"")]
    public GameObject RoundManagerPrefab;
    protected void round(string[] keywords)
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
                            BuildConsole.WriteLine(activeRM.roundElapsedTime);
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
                                BuildConsole.WriteLine(activeRM.roundElapsedTime);
                            }
                            else if(RoundManagerPrefab)
                            {
                                RoundManager rm = RoundManagerPrefab.GetComponent<RoundManager>();
                                if(rm)
                                {
                                    rm.roundRunningEndTime = value;
                                    BuildConsole.WriteLine("Round time set to" + value);
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
            default:
                {
                    BuildConsole.WriteLine("Invalid parameters for round");
                    break;
                }
        }
    }

    protected void SetFocus()
    {
        _field.Select();
        _field.ActivateInputField();
        //EventSystem.current.SetSelectedGameObject(gameObject, null);
        //_field.OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
