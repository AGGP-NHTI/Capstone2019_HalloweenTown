using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUpDisplay : MonoBehaviour
{
    public int myPlayerIndex = 0;
    public Image bkgPanel;
    public Text ReadyText;
    public string unreadyString = "A to ready up";
    public string readyString = "Ready!";
    public float unreadyOpacity = 0.4f;
    public float readyOpacity = 1.0f;
    public Manager playerManager;

    protected bool _isJoined = true;
    protected bool _isReady = true;

    protected void Update()
    {
        if(!playerManager)
        {
            return;
        }

        if(playerManager.joinedGame[myPlayerIndex] != _isJoined)
        {
            SetJoined(playerManager.joinedGame[myPlayerIndex]);
        }

        if(playerManager.readyUp[myPlayerIndex] != _isReady)
        {
            SetReady(playerManager.readyUp[myPlayerIndex]);
        }
    }

    public void SetReady(bool value)
    {
        _isReady = value;

        if (_isJoined)
        {
            if (value)
            {
                ReadyText.text = readyString;
                Color newColor = bkgPanel.color;
                newColor.a = readyOpacity;
                bkgPanel.color = newColor;
            }
            else
            {
                ReadyText.text = unreadyString;
                Color newColor = bkgPanel.color;
                newColor.a = unreadyOpacity;
                bkgPanel.color = newColor;
            }
        }
    }

    public void SetJoined(bool value)
    {
        _isJoined = value;
        bkgPanel.enabled = value;
        for(int i = 0; i < bkgPanel.transform.childCount; i++)
        {
            bkgPanel.transform.GetChild(i).gameObject.SetActive(value);
        }
        if(value)
        {
            SetReady(_isReady);
        }
    }
}
