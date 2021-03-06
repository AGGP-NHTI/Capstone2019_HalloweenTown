﻿using System.Collections;
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

    Coroutine waitingformom = null;
    int randomVoiceLine = 1;
    public AudioSource talkingSource;
    public AudioSource screamSource;
    public AudioClip childscream;
    public AudioClip mom1;
    public AudioClip mom2;
    public AudioClip mom3;

    public List<Pawn> Targets;

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

    public void LetMomHunt(Pawn[] targets)
    {
        Targets = new List<Pawn>(targets);

        UpdateTargetCount();
        _huntStarted = true;
    }

    public void UpdateTargetCount()
    {   
        _controller.localBlackboard.SetProperty(PROPERTY_TARGETCOUNT, Targets.Count);
    }

    public bool DoneHunting()
    {
        return _huntStarted && (_controller.localBlackboard.GetProperty<int>(PROPERTY_TARGETCOUNT) <= 0);
    }

    public void ChildScream()
    {
        screamSource.clip = childscream;
        screamSource.Play();
    }

    public void GetRandomMomSaying()
    {
        if (waitingformom == null && !talkingSource.isPlaying)
        {          
            if (randomVoiceLine == 1)
            {
                MomSaying1();
            }
            else if (randomVoiceLine == 2)
            {
                MomSaying2();
            }
            else if (randomVoiceLine == 3)
            {
                MomSaying3();
            }
            randomVoiceLine += 1;
            if(randomVoiceLine >3)
            {
                randomVoiceLine = 1;
            }
            waitingformom = StartCoroutine("waitForMomToTalk");
        }
    }
    public void MomSaying1()
    {
        talkingSource.clip = mom1;
        talkingSource.Play();
    }
    public void MomSaying2()
    {
        talkingSource.clip = mom2;
        talkingSource.Play();
    }
    public void MomSaying3()
    {
        talkingSource.clip = mom3;
        talkingSource.Play();
    }

    IEnumerator waitForMomToTalk()
    {
        if (!talkingSource.isPlaying)
        {
            yield return new WaitForSeconds(25);
            waitingformom = null;
        }
        
    }
}