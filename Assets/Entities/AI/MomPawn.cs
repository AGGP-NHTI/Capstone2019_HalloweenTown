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

    Coroutine waitingformom = null;
    int randomVoiceLine = 1;
    public AudioSource audioSource;
    public AudioClip childscream;
    public AudioClip mom1;
    public AudioClip mom2;
    public AudioClip mom3;

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

    public void ChildScream()
    {
        audioSource.clip = childscream;
        audioSource.Play();
    }

    public void GetRandomMomSaying()
    {
        if (waitingformom == null && !audioSource.isPlaying)
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
        audioSource.clip = mom1;
        audioSource.Play();
    }
    public void MomSaying2()
    {
        audioSource.clip = mom2;
        audioSource.Play();
    }
    public void MomSaying3()
    {
        audioSource.clip = mom3;
        audioSource.Play();
    }

    IEnumerable waitForMomToTalk()
    {
        if (!audioSource.isPlaying)
        {
            yield return new WaitForSeconds(25);
            waitingformom = null;
        }
        
    }
}