using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{   
    public Pawn MyPawn;
    public Interactable selectedInteractable;
    public WorldIcon interactIcon;

    ParticleManager particleManager;
    
    public float interactionRange = 3.0f;

    protected float _interactionDuration = 0.0f;

    protected bool _waitingForButtonRelease = false;

    public void Start()
    {
        particleManager = GetComponent<ParticleManager>();
    }

    public virtual void TryToInteract(bool value)
    {
        if (value)
        {
            if (selectedInteractable && !_waitingForButtonRelease)
            {
                if(selectedInteractable.tag == "Vampire Mask" || selectedInteractable.tag == "Witch Mask" || selectedInteractable.tag == "Werewolf Mask" || selectedInteractable.tag == "Ghost Mask")
                {
                    particleManager.tornadoPart();
                }

                if (_interactionDuration >= selectedInteractable.interactTime)
                {
                    particleManager.tornadoPartStop();
                    selectedInteractable.Interact(MyPawn);
                    _waitingForButtonRelease = true;
                }
                _interactionDuration += Time.deltaTime;
            }
        }
        else
        {
            
            _interactionDuration = 0.0f;
            _waitingForButtonRelease = false;
        }
    }

    protected virtual void FixedUpdate()
    {
        FindInteractableToSelect();
    }

    protected virtual void FindInteractableToSelect()
    {
        Interactable foundInteractable = null;
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, interactionRange);

        List<Interactable> nearbyInteractables = new List<Interactable>();
        foreach(Collider c in nearbyColliders)
        {
            Interactable i = c.GetComponent<Interactable>();
            if(i)
            {
                nearbyInteractables.Add(i);
            }
        }

        //Just using the nearest interactable atm
        float nearestInteractableSqrDistance = -1.0f;
        foreach(Interactable i in nearbyInteractables)
        {
            if(i.IsInteractable(MyPawn))
            {
                float sqrDistance = (i.transform.position - transform.position).sqrMagnitude;

                if(nearestInteractableSqrDistance < 0.0f)
                {
                    nearestInteractableSqrDistance = sqrDistance;
                    foundInteractable = i;
                }
                else if(nearestInteractableSqrDistance > sqrDistance)
                {
                    nearestInteractableSqrDistance = sqrDistance;
                    foundInteractable = i;
                }
            }
        }

        if(interactIcon)
        {
            if (foundInteractable)
            {
                interactIcon.MoveToTransform = foundInteractable.transform;
                interactIcon.LetRender = true;
                interactIcon.SetProgress(_interactionDuration, foundInteractable.interactTime);
            }
            else
            {
                interactIcon.LetRender = false;
            }
        }

        if (foundInteractable != selectedInteractable)
        {
            selectedInteractable = foundInteractable;
            _interactionDuration = 0.0f;
            _waitingForButtonRelease = true;
        }
    }
}