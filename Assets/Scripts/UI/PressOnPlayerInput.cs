using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressOnPlayerInput : MonoBehaviour
{
    public Manager playerManager;
    public Button button;

    protected virtual void Update()
    {
        for(int i = 0; i < playerManager.inputObject.Count; i++)
        {
            if(playerManager.inputObject[i].GetBooInput() && !playerManager.joinedGame[i])
            {
                button.onClick.Invoke();
            }
        }
    }
}
