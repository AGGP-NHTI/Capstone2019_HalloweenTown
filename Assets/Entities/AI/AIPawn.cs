using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;
using UnityEngine.AI;

public class AIPawn : MonoBehaviour
{
    public const string PROPERTY_AGGRO = "IsAggrod";

    protected AIController _controller;

    public NavMeshAgent agent;

    public virtual void Init(AIController controller)
    {
        _controller = controller;
        _controller.localBlackboard.SetProperty(PROPERTY_AGGRO, false);
    }

    public virtual void GiveAggro(Pawn instigator)
    {
        if (!(instigator || _controller))
        {
            //This is here for cases like environmental hazards, so the AI doesn't get confused and attack a wall
            return;
        }

        object currentAggroObj = _controller.localBlackboard.GetProperty(PROPERTY_AGGRO);
        if (currentAggroObj is bool)
        {
            if (!(bool)currentAggroObj)
            {
                _controller.localBlackboard.SetProperty(PROPERTY_AGGRO, true);
                _controller.InterruptBehavior();
                _controller.localBlackboard.SetProperty("target", instigator);
            }
        }
    }

    public virtual void DeathBehavior(Pawn killer)
    {
        Debug.Log("Ye killed me, " + killer.name);
        Destroy(gameObject);
    }
}
