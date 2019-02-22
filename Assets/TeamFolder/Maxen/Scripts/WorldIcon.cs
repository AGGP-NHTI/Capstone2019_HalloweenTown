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

    private RectTransform _myRect;
    private Image _image;
    
    private void Start()
    {
        _myRect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

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
            if (!_image.enabled)
            {
                _image.enabled = true;
            }
        }
        else if(_image.enabled)
        {
            _image.enabled = false;
        }
    }
}
