using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildConsole : MonoBehaviour
{
#region Static Stuff
    const string outputPrefix = "> ";
    public static void WriteLine(object msg)
    {
        if(_instance)
        {
            _instance.output.text += outputPrefix + msg.ToString() + "\n";
        }
    }

    public static void ClearLog()
    {
        _instance.output.text = "";
    }

    protected static BuildConsole _instance;
#endregion

    protected void Awake()
    {
        if(_instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public KeyCode ConsoleVisibleToggleKey = KeyCode.BackQuote;
    public GameObject console;
    public Text output;
    public InputField input;

    protected void Update()
    {
        if (Input.GetKeyDown(ConsoleVisibleToggleKey))
        {
            console.SetActive(!console.activeSelf);
            if(console.activeSelf)
            {
                input.Select();
                input.ActivateInputField();
            }
        }
    }
}
