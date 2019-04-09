using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using UnityEngine.AI;

public class MoveToTarget : Behavior
{
    protected Pawn target;
    protected float reach;
    Vector3 currentDestination;

    protected const float recalculatePathDistance = 5.0f;

    public override void OnEnter(AIController ai)
    {
        object objTarget = ai.localBlackboard.GetProperty("target");
        if(objTarget is Pawn)
        {
            target = objTarget as Pawn;
            ai.aiPawn.agent.SetDestination(target.transform.position);
            currentDestination = target.transform.position;
            _currentPhase = StatePhase.ACTIVE;
        }
        else
        {
            _currentPhase = StatePhase.INACTIVE;
        }
    }

    public override void ActiveBehavior(AIController ai)
    {
        //DEBUG LINE DRAWING
        AI.Util.DrawPath(ai.transform.position, ai.aiPawn.agent.path.corners, ai.treeUpdateInterval * Time.fixedDeltaTime);

        bool doPathCalculation = false;
        if ((ai.transform.position - target.transform.position).sqrMagnitude > recalculatePathDistance * recalculatePathDistance)
        {
            doPathCalculation = true;
        }

        //Need some kind of "weapon" class is needed
        /*if ((ai.transform.position - target.transform.position).sqrMagnitude <= weapon.reach * weapon.reach)
        {
            weapon.DoAttack(target.gameObject, ai.aiPawn);
        }
        else if (!movement.DoMovement)
        {
            doPathCalculation = true;
        }*/

        if (doPathCalculation && !ai.aiPawn.agent.pathPending)
        {
            ai.aiPawn.agent.SetDestination(target.transform.position);
            currentDestination = target.transform.position;
        }
    }

    public override void OnExit(AIController ai)
    {
        throw new System.NotImplementedException();
    }
}
