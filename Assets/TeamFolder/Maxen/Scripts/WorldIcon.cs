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

    public Vector2 screenCenter;
    public Vector2 screenOffset;
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

        screenCenter = new Vector2(renderCamera.pixelWidth / 2, renderCamera.pixelHeight / 2);
        screenOffset = renderCamera.rect.position * screenCenter;
        
        screenPos = renderCamera.WorldToScreenPoint(MoveToPosition);
        if (screenPos.z > 0.0f && LetRender)
        {
            _myRect.localPosition = (Vector2)screenPos - screenCenter - screenOffset;
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
