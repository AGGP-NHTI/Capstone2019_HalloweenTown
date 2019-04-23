using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{
    public Transform Barrel;
    public Transform DefaultAnchor;
    public Transform AimedAnchor;

    public bool IsAiming = false;

    protected virtual void Start()
    {
        if(!Barrel)
        {
            Debug.LogWarning("No barrel assigned on barrelcontroller!");
        }
        else if(IsAiming)
        {
            SetBarrelAnchor(AimedAnchor);
        }
        else
        {
            SetBarrelAnchor(DefaultAnchor);
        }
    }

    protected virtual void FixedUpdate()
    {
        if(IsAiming)
        {
            //if(Barrel.parent != AimedAnchor)
            //{
            //    SetBarrelAnchor(AimedAnchor);
            //}
            SetBarrelAnchor(AimedAnchor);
        }
        else
        {
            //if(Barrel.parent != DefaultAnchor)
            //{
            //    SetBarrelAnchor(DefaultAnchor);
            //}
            SetBarrelAnchor(DefaultAnchor);
        }
    }

    public virtual void SetBarrelAnchor(Transform t)
    {
        //if(t == null)
        //{
        //    Barrel.parent = transform;
        //}
        //else
        //{
        //    Barrel.parent = t;
        //}
        
        //Barrel.localPosition = Vector3.zero;
        //Barrel.localRotation = Quaternion.identity;
        if(t == null)
        {
            t = transform;
        }
        Barrel.position = t.position;
        Barrel.rotation = t.rotation;
    }
}
