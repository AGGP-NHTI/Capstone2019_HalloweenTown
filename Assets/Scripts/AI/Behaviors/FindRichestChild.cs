using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class FindRichestChild : Behavior
{
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
        PlayerController richestChild = null;

        foreach(KeyValuePair<PlayerController, int> kvp in Candy.Scoreboard)
        {
            if (kvp.Key.ControlledPawn)
            {
                if (richestChild == null)
                {
                    richestChild = kvp.Key;
                }
                else
                {
                    if (kvp.Value > Candy.Scoreboard[richestChild])
                    {
                        richestChild = kvp.Key;
                    }
                    else if (kvp.Value == Candy.Scoreboard[richestChild])
                    {
                        //If there are multiple richest children, pick the one who's closest
                        float richestSqrDistance = (aiPosition - richestChild.ControlledPawn.transform.position).sqrMagnitude;
                        float pcSqrDistance = (aiPosition - kvp.Key.ControlledPawn.transform.position).sqrMagnitude;

                        if(pcSqrDistance < richestSqrDistance)
                        {
                            richestChild = kvp.Key;
                        }
                    }
                }
            }
        }

        if(richestChild)
        {
            return richestChild.ControlledPawn;
        }
        else
        {
            return null;
        }
    }
}
