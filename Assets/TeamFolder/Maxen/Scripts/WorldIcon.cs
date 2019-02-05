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

    public Vector3 screenPos;

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
        
        screenPos = renderCamera.WorldToScreenPoint(MoveToPosition);
        if (screenPos.z > 0.0f && LetRender)
        {
            //screenPos.z = 0.0f;
            _myRect.position = screenPos;
            if(!_image.enabled)
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
