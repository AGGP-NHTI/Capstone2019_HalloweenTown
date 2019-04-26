using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera fallbackCamera;
    public Camera[] AllCameras;
    public int playerCameraDepth;

    public delegate void FinishedAction();
    protected event FinishedAction CutsceneFinished;

    PlayableDirector activeCutsceneObject;

    public bool CutsceneRunning
    {
        get
        {
            return activeCutsceneObject != null;
        }
    }

    private void Awake()
    {
        if(Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        AllCameras = FindObjectsOfType<Camera>();
    }

    public void SkipCutscene()
    {
        if(activeCutsceneObject)
        {
            activeCutsceneObject.Stop();
        }
    }

    public void ConfigurePlayerCameraDepths(List<Camera> PlayerCameras)
    {
        foreach(Camera c in AllCameras)
        {
            if(PlayerCameras.Contains(c))
            {
                //Debug.Log(c.name + " is playerCam");
                c.depth = playerCameraDepth;
            }
            else if(c.depth >= playerCameraDepth - 1)
            {
                //Debug.Log(c.name + " is NOT playerCam");
                c.depth = playerCameraDepth - 2;
            }
        }

        if (fallbackCamera)
        {
            fallbackCamera.depth = playerCameraDepth - 1;
        }
    }

    public void StartCutscene(GameObject CutsceneObject, FinishedAction CutsceneFinishedEvent = null)
    {
        if (activeCutsceneObject)
        {
            activeCutsceneObject.Stop();
        }

        Camera cam = CutsceneObject.GetComponent<Camera>();
        PlayableDirector timeline = CutsceneObject.GetComponent<PlayableDirector>();

        if (cam && timeline)
        {
            CutsceneFinished = CutsceneFinishedEvent;

            cam.enabled = true;
            cam.depth = playerCameraDepth + 1;
        
            timeline.Play();
            timeline.stopped += OnCustsceneFinished;

            activeCutsceneObject = timeline;
        }
    }

    protected void OnCustsceneFinished(PlayableDirector pd)
    {
        Camera cam = pd.GetComponent<Camera>();
        if(cam)
        {
            cam.enabled = false;
            cam.depth = playerCameraDepth - 1;
        }

        activeCutsceneObject = null;

        if(CutsceneFinished != null)
        {
            CutsceneFinished();
            CutsceneFinished = null;
        }
    }
}
