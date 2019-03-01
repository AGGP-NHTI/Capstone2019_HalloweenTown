using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneSkipper : MonoBehaviour
{
    public InputObject[] validInputObjects;

    public Image SkipIcon;
    public Image ProgressBar;

    public float ButtonHoldTime = 1.5f;
    protected float _currentHeldTime = 0.0f;
    
    protected void Update ()
    {
        if(!CameraManager.Instance)
        {
            return;
        }

        if(CameraManager.Instance.CutsceneRunning)
        {
            bool buttonBeingPressed = false;
            foreach (InputObject io in validInputObjects)
            {
                if(io.GetJumpInput())
                {
                    buttonBeingPressed = true;
                }
            }

            if(buttonBeingPressed)
            {
                _currentHeldTime += Time.deltaTime;
                float heldPercent = _currentHeldTime / ButtonHoldTime;

                if (SkipIcon)
                {
                    SkipIcon.enabled = true;
                }
                if (ProgressBar)
                {
                    ProgressBar.enabled = true;
                    ProgressBar.fillAmount = heldPercent;
                }

                if(heldPercent >= 1.0f)
                {
                    CameraManager.Instance.SkipCutscene();
                }
            }
            else
            {
                if(SkipIcon)
                {
                    SkipIcon.enabled = false;
                }
                if(ProgressBar)
                {
                    ProgressBar.enabled = false;
                }

                _currentHeldTime = 0.0f;
            }
        }
        else
        {
            if (SkipIcon)
            {
                SkipIcon.enabled = false;
            }
            if (ProgressBar)
            {
                ProgressBar.enabled = false;
            }
        }
	}
}
