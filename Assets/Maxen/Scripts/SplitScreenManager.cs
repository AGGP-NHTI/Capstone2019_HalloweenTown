using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public static SplitScreenManager Instance;

    public List<Camera> PlayerCameras;
    public List<uint> CameraNumbers;
    public Camera FallbackCamera;
    public int playerCameraDepth;

    public bool TriggerConfigureScreenSpace;

    protected virtual void Awake ()
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

    private void Update()
    {
        if(TriggerConfigureScreenSpace)
        {
            ConfigureScreenSpace();
            TriggerConfigureScreenSpace = false;
        }
    }

    public virtual void ConfigureScreenSpace()
    {
        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach(Camera c in allCameras)
        {
            if(c.depth >= playerCameraDepth - 1)
            {
                if(FallbackCamera)
                {
                    FallbackCamera.depth = playerCameraDepth - 1;
                }
                c.depth = playerCameraDepth - 2;
            }
        }
        
        int xCameras = 1;
        int yCameras = 1;

        if (0 < PlayerCameras.Count && PlayerCameras.Count <= 2)
        {
            xCameras = PlayerCameras.Count;
        }
        else
        {
            xCameras = Mathf.CeilToInt(Mathf.Sqrt(PlayerCameras.Count));
            yCameras = Mathf.CeilToInt((float)PlayerCameras.Count / xCameras);
        }

        Vector2 CameraPosition = Vector2.zero;
        Vector2 CameraSize = new Vector2(1.0f / xCameras, 1.0f / yCameras);

        for (int cameraIndex = 0; cameraIndex < PlayerCameras.Count; cameraIndex++)
        {
            PlayerCameras[cameraIndex].rect = new Rect(CameraPosition, CameraSize);
            Debug.Log(PlayerCameras[cameraIndex].transform.parent.parent.name + ": " + PlayerCameras[cameraIndex].rect);
            PlayerCameras[cameraIndex].enabled = true;

            CameraPosition.x += CameraSize.x;
            if (CameraPosition.x >= 1.0f)
            {
                CameraPosition.x = 0.0f;
                CameraPosition.y += CameraSize.y;
            }
            PlayerCameras[cameraIndex].depth = playerCameraDepth;
        }
    }
}
