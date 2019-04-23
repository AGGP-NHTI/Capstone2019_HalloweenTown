using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class MomPawn : AIPawn
{
    public const string PROPERTY_TARGETCOUNT = "TargetCount";
    public const string PROPERTY_TARGETSET = "TargetSet";
    public float CaptureRange = 2.0f;

    private bool _huntStarted = false;
    public Animator _anim;

    private void Awake()
    {
        _anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if(agent.velocity.magnitude > 0.001)
        {
            _anim.SetBool("runBool", true);
        }
        else
        {
            _anim.SetBool("runBool", false);

        }
    }

    public override void Init(AIController controller)
    {
        base.Init(controller);
        _controller.localBlackboard.SetProperty(PROPERTY_TARGETCOUNT, 0);
        _controller.localBlackboard.SetProperty(PROPERTY_TARGETSET, false);
    }

    public void LetMomHunt()
    {
        UpdateTargetCount();
        _huntStarted = true;
    }

    public void UpdateTargetCount()
    {
        int targetCount = 0;
        foreach (PlayerController pc in Candy.Scoreboard.Keys)
        {
            if(pc.ControlledPawn)
            {
                targetCount++;
            }
        }
        _controller.localBlackboard.SetProperty(PROPERTY_TARGETCOUNT, targetCount);
    }

    public bool DoneHunting()
    {
        return _huntStarted && (_controller.localBlackboard.GetProperty<int>(PROPERTY_TARGETCOUNT) <= 0);
    }
}