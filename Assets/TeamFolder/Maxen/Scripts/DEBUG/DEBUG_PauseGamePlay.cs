using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_PauseGamePlay : MonoBehaviour
{
    public KeyCode togglePauseKey = KeyCode.P;

    protected bool isPaused = false;
    protected float runtimeScale;

	void Start ()
    {
        runtimeScale = Time.timeScale;
	}
	
	
	void Update ()
    {
		if(Input.GetKeyDown(togglePauseKey))
        {
            UpdatePausedState(!isPaused);
        }
	}

    void UpdatePausedState(bool newState)
    {
        if(newState && !isPaused)
        {
            runtimeScale = Time.timeScale;

            Time.timeScale = 0.0f;
        }
        else if(isPaused && !newState)
        {
            Time.timeScale = runtimeScale;
        }

        isPaused = newState;
    }
}
