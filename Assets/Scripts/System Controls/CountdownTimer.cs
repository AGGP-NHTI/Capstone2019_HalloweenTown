using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Text CountDownText;
    public GameObject CountDownPanel;
    public Manager PlayerManager;
    public string CountdownMessage = "Round starting in ";

    protected void Update()
    {
        if(CountDownPanel && CountDownText && PlayerManager)
        {
            if(PlayerManager.RoundReadyToStart)
            {
                CountDownPanel.SetActive(true);
                CountDownText.text = CountdownMessage + (int)PlayerManager.CountDownDuration;
            }
            else
            {
                CountDownPanel.SetActive(false);
            }
        }
    }
}
