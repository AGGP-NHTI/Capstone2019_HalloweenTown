using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCutsceneDebug : MonoBehaviour {

    public GameObject cutsceneObject;
    public bool triggerCutscene;
	
	// Update is called once per frame
	void Update ()
    {
        if(triggerCutscene)
        {
            CameraManager.Instance.StartCutscene(cutsceneObject);
            triggerCutscene =  false;
        }
        
	}
}
