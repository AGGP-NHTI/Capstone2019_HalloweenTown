using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class MomPawn : AIPawn
{
    public const string PROPERTY_TARGETCOUNT = "TargetCount";
    public const string PROPERTY_TARGETSET = "TargetSet";
    public float CaptureRange = 2.0f;

    public override void Init(AIController controller)
    {
        base.Init(controller);
        int targetCount = 0;
        if(Candy.Scoreboard != null)
        {
            targetCount = Candy.Scoreboard.Count;
        }
        _controller.localBlackboard.SetProperty(PROPERTY_TARGETCOUNT, targetCount);
        _controller.localBlackboard.SetProperty(PROPERTY_TARGETSET, false);
    }
}
