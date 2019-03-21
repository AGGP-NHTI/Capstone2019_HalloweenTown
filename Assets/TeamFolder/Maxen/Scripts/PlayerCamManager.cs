using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamManager : MonoBehaviour
{
    public CinemachineVirtualCamera[] VirtualCameras;
    public int ActivePriority = 10;

    protected int _activeCameraIndex = -1;

    private void Start()
    {
        SetVirtualCamera(0);
    }

    public void SetVirtualCamera(int index)
    {
        if(_activeCameraIndex == index)
        {
            return;
        }

        if (0 <= index && index < VirtualCameras.Length)
        {
            for(int i = 0; i < VirtualCameras.Length; i++)
            {
                if(i == index)
                {
                    VirtualCameras[i].Priority = ActivePriority;
                }
                else
                {
                    VirtualCameras[i].Priority = 0;
                }
            }
        }

        _activeCameraIndex = index;
    }

    public void SetCameraLayer(int layer)
    {
        foreach(CinemachineVirtualCamera cam in VirtualCameras)
        {
            cam.gameObject.layer = layer;
        }
    }
}
