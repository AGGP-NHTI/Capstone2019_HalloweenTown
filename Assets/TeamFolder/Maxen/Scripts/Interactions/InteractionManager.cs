using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Pawn MyPawn;
    public Interactable selectedInteractable;

    public float interactionRange = 3.0f;

    public virtual void TryToInteract()
    {
        if (selectedInteractable != null)
        {
            selectedInteractable.Interact(MyPawn);
        }
    }

    protected virtual void FixedUpdate()
    {
        FindInteractableToSelect();
    }

    protected virtual void FindInteractableToSelect()
    {
        selectedInteractable = null;
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
                    selectedInteractable = i;
                }
                else if(nearestInteractableSqrDistance > sqrDistance)
                {
                    nearestInteractableSqrDistance = sqrDistance;
                    selectedInteractable = i;
                }
            }
        }
    }
}