using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class FindRichestChild : Behavior
{
    protected MomPawn mom;

    public override void OnEnter(AIController ai)
    {
        SharedBehavior(ai);
    }

    public override void ActiveBehavior(AIController ai)
    {
        SharedBehavior(ai);
    }

    public override void OnExit(AIController ai)
    {
        SharedBehavior(ai);
    }

    protected virtual void SharedBehavior(AIController ai)
    {
        Vector3 aiPosition;
        if (ai.aiPawn)
        {
            aiPosition = ai.aiPawn.transform.position;
            mom = (ai.aiPawn as MomPawn);
        }
        else
        {
            aiPosition = ai.transform.position;
        }

        Pawn target = GetRichestChild(aiPosition);
        ai.localBlackboard.SetProperty("target", target);
        ai.localBlackboard.SetProperty(MomPawn.PROPERTY_TARGETSET, target != null);

        _currentPhase = StatePhase.INACTIVE;
    }

    protected virtual Pawn GetRichestChild(Vector3 aiPosition)
    {
        Pawn richestChild = null;

        foreach(Pawn childPawn in mom.Targets)
        {
            if (richestChild == null && childPawn.MyCandy)
            {
                richestChild = childPawn;
            }
            else if(childPawn.MyCandy)
            {
                if (childPawn.MyCandy.candy > richestChild.MyCandy.candy)
                {
                    richestChild = childPawn;
                }
                else if (childPawn.MyCandy.candy == richestChild.MyCandy.candy)
                {
                    //If there are multiple richest children, pick the one who's closest
                    float richestSqrDistance = (aiPosition - richestChild.transform.position).sqrMagnitude;
                    float pcSqrDistance = (aiPosition - childPawn.transform.position).sqrMagnitude;

                    if(pcSqrDistance < richestSqrDistance)
                    {
                        richestChild = childPawn;
                    }
                }
            }
        }

        if(richestChild)
        {
            return richestChild;
        }
        else
        {
            return null;
        }
    }
}
