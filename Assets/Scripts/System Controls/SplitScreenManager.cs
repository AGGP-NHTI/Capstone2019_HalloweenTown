using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    public static SplitScreenManager Instance;

    public List<Camera> PlayerCameras;
    public List<uint> CameraNumbers;
    

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

    private void OnValidate()
    {
        if(TriggerConfigureScreenSpace)
        {
            ConfigureScreenSpace();
            TriggerConfigureScreenSpace = false;
        }
    }

    public virtual void ConfigureScreenSpace()
    {
        //Configure Camera Depth (which renders on top of which)
        CameraManager.Instance.ConfigurePlayerCameraDepths(PlayerCameras);
        
        //Configure screen space
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

        Vector2 CameraSize = new Vector2(1.0f / xCameras, 1.0f / yCameras);
        Vector2 CameraPosition = new Vector2(0, 1 - CameraSize.y);

        for (int cameraIndex = 0; cameraIndex < PlayerCameras.Count; cameraIndex++)
        {
            PlayerCameras[cameraIndex].rect = new Rect(CameraPosition, CameraSize);
            //Debug.Log(PlayerCameras[cameraIndex].transform.parent.parent.name + ": " + PlayerCameras[cameraIndex].rect);
            PlayerCameras[cameraIndex].enabled = true;

            CameraPosition.x += CameraSize.x;
            if (CameraPosition.x >= 1.0f)
            {
                CameraPosition.x = 0.0f;
                CameraPosition.y -= CameraSize.y;
            }
        }
    }
}