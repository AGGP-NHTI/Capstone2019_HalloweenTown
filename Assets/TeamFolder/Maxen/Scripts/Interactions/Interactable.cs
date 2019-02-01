using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractEvent : UnityEvent<Pawn, Interactable> { }

public class Interactable : MonoBehaviour
{
    public InteractEvent OnInteract;
    public bool AvailableForInteraction = true;

    private void Awake()
    {
        if(OnInteract == null)
        {
            OnInteract = new InteractEvent();
        }
    }

    public virtual void Interact(Pawn source)
    {
        OnInteract.Invoke(source, this);
    }
}
