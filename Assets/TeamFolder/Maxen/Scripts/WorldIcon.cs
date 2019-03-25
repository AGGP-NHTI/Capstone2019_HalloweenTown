using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class WorldIcon : MonoBehaviour
{
    public Camera renderCamera;
    public Vector3 MoveToPosition;
    public Transform MoveToTransform;
    public bool LetRender = true;
    public Image[] images;

    private RectTransform _myRect;
    
    private void Start()
    {
        _myRect = GetComponent<RectTransform>();

    }
    private void LateUpdate()
    {
        if(MoveToTransform)
        {
            MoveToPosition = MoveToTransform.position;
        }

        if (!renderCamera)
        {
            return;
        }

        Vector3 screenPos = renderCamera.WorldToViewportPoint(MoveToPosition);
        bool onScreen = (screenPos.z >= 0) && (0 <= screenPos.x && screenPos.x <= 1.0f) && (0 <= screenPos.y && screenPos.y <= 1.0f);
        if (onScreen && LetRender)
        {
            _myRect.anchorMin = screenPos;
            _myRect.anchorMax = screenPos;
            foreach (Image img in images)
            {
                if (!img.enabled)
                {
                    img.enabled = true;
                }
            }
        }
        else
        {
            foreach (Image img in images)
            {
                if (img.enabled)
                {
                    img.enabled = false;
                }
            }
        }
    }

    public void SetProgress(float current, float goal)
    {
        if(goal == 0.0f)
        {
            images[0].fillAmount = 0.0f;
        }
        else
        {
            images[0].fillAmount = current / goal;
        }
    }
}
