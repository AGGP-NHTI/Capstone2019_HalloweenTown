using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using UnityEngine.AI;

public class MoveToTarget : Behavior
{
    protected Pawn target;
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
        float sqrDistance = (ai.transform.position - target.transform.position).sqrMagnitude;
        if(sqrDistance > recalculatePathDistance * recalculatePathDistance)
        {
            doPathCalculation = true;
        }
        else if(ai.aiPawn is MomPawn)
        {
            MomPawn mom = ai.aiPawn as MomPawn;
            float capRange = mom.CaptureRange;
            if(sqrDistance <= capRange)
            {
                mom.ChildScream();//new
                Pawn p = target.GetComponent<Pawn>();
                if(p)
                {
                    if(p.MyController)
                    {
                        p.MyController.ControlledPawn = null;
                    }
                    mom.Targets.Remove(p);
                }
                target.Capture();
                //DestroyerOfObjects.DestroyObject(target.gameObject);
                _currentPhase = StatePhase.INACTIVE;
                mom.UpdateTargetCount();
                ai.localBlackboard.SetProperty("target");
                ai.localBlackboard.SetProperty(MomPawn.PROPERTY_TARGETSET, false);
            }            
            else
            {
                doPathCalculation = true;
                if(sqrDistance <= 100)
                {
                    Debug.Log("mom speaks");
                    mom.GetRandomMomSaying();
                }
            }
        }

        if (doPathCalculation && !ai.aiPawn.agent.pathPending)
        {
            ai.aiPawn.agent.SetDestination(target.transform.position);
            currentDestination = target.transform.position;
        }
    }

    public override void OnExit(AIController ai)
    {
        //throw new System.NotImplementedException();
    }
}
