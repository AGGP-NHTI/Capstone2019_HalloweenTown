using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    protected PlayerController[] _controllersInScene;

    public enum SplitScreenDirection
    {
        HORIZONTAL,
        VERTICAL
    }
    public SplitScreenDirection FavoredSplitDirection = SplitScreenDirection.HORIZONTAL;

    void Start ()
    {
        _controllersInScene = FindObjectsOfType<PlayerController>();
        ConfigureScreenSpace();
	}

    protected void ConfigureScreenSpace()
    {
        Camera[] playerCameras = new Camera[_controllersInScene.Length];
        for (int i = 0; i < _controllersInScene.Length; i++)
        {
            playerCameras[i] = _controllersInScene[i].MyCamera;
        }

        int verticalCameras = 1;
        int horizontalCameras = 1;
        if (playerCameras.Length <= 2)
        {
            if(FavoredSplitDirection == SplitScreenDirection.HORIZONTAL)
            {
                verticalCameras = 1;
                horizontalCameras = 2;
            }
            else if(FavoredSplitDirection == SplitScreenDirection.VERTICAL)
            {
                verticalCameras = 2;
                horizontalCameras = 1;
            }
        }
        else
        {
            float Lsqrt = Mathf.CeilToInt(Mathf.Sqrt(playerCameras.Length));
            if (FavoredSplitDirection == SplitScreenDirection.HORIZONTAL)
            {
                horizontalCameras = Mathf.CeilToInt(Lsqrt);
                verticalCameras = playerCameras.Length / horizontalCameras;
            }
            else if(FavoredSplitDirection == SplitScreenDirection.VERTICAL)
            {
                verticalCameras = Mathf.CeilToInt(Mathf.Sqrt(playerCameras.Length));
                horizontalCameras = playerCameras.Length / verticalCameras;
            }
        }
        float cameraWidth = 1.0f / horizontalCameras;
        float cameraHeight = 1.0f / verticalCameras;
    }
}
