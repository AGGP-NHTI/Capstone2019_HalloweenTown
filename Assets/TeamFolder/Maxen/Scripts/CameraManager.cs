using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public Camera fallbackCamera;
    public Camera[] AllCameras;
    public int playerCameraDepth;

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

    public void ConfigurePlayerCameraDepths(List<Camera> PlayerCameras)
    {
        if(fallbackCamera)
        {
            fallbackCamera.depth = playerCameraDepth - 1;
        }
        
        foreach(Camera c in AllCameras)
        {
            if(PlayerCameras.Contains(c))
            {
                c.depth = playerCameraDepth;
            }
            else if(c.depth >= playerCameraDepth - 1)
            {
                c.depth = playerCameraDepth - 2;
            }
        }
    }

    public void StartCutscene(GameObject CutsceneObject)
    {
        Camera cam = CutsceneObject.GetComponent<Camera>();
        if(cam)
        {
            cam.enabled = true;
            cam.depth = playerCameraDepth + 1;
        }

        Animator anim = CutsceneObject.GetComponent<Animator>();
        if(anim)
        {
            anim.SetTrigger("Start");
        }
    }
}
