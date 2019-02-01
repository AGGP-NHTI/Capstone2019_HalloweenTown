using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour {

    #region Overall Round Management Variables
    public enum RoundPhase
    {
        PRE_GAME,
        ROUND_STARTING,
        ROUND_RUNNING,
        ROUND_ENDING,
        ROUND_OVER
    }
    [Header("Round Management Variables")]
    public Manager myManager;
    public RoundPhase currentPhase = RoundPhase.PRE_GAME;
    protected Coroutine ActiveLogicRoutine;

    public float roundElapsedTime = 0.0f;
    public float roundStartEndTime = -1.0f;
    public float roundRunningEndTime = -1.0f;
    public float roundEndingEndTime = -1.0f;
    protected bool letRoundTimerRun = false;
    #endregion

    #region Specific Phase Variables
    [Header("Round Ending")]
    public GameObject momPrefab;
    [HideInInspector] public GameObject spawnedMom;

    [Header("Round Over")]
    public float timeBeforeReturningToMenu = 5.0f;
    public int MainMenuBuildIndex = 0;
    #endregion

    public virtual void StartRound()
    {
        currentPhase = RoundPhase.ROUND_STARTING;
        letRoundTimerRun = true;
    }

    #region Round Logic
    protected virtual void FixedUpdate()
    {
        if(letRoundTimerRun)
        {
            roundElapsedTime += Time.fixedDeltaTime;
        }

        if(ActiveLogicRoutine == null)
        {
            switch (currentPhase)
            {
                case RoundPhase.PRE_GAME:
                    {
                        ActiveLogicRoutine = StartCoroutine(PreGameLogic());
                        break;
                    }
                case RoundPhase.ROUND_STARTING:
                    {
                        ActiveLogicRoutine = StartCoroutine(RoundStartingLogic());
                        break;
                    }
                case RoundPhase.ROUND_RUNNING:
                    {
                        ActiveLogicRoutine = StartCoroutine(RoundRunningLogic());
                        break;
                    }
                case RoundPhase.ROUND_ENDING:
                    {
                        ActiveLogicRoutine = StartCoroutine(RoundEndingLogic());
                        break;
                    }
                case RoundPhase.ROUND_OVER:
                    {
                        ActiveLogicRoutine = StartCoroutine(RoundOverLogic());
                        break;
                    }
            }
        }
    }

    protected virtual IEnumerator PreGameLogic()
    {
        while(currentPhase == RoundPhase.PRE_GAME)
        {
            yield return null;
        }

        ActiveLogicRoutine = null;
    }

    protected virtual IEnumerator RoundStartingLogic()
    {
        //Maybe move player spawning here. Otherwise only necessary for future-proofing.
        while(currentPhase == RoundPhase.ROUND_STARTING)
        {
            if (roundElapsedTime >= roundStartEndTime && roundStartEndTime > 0.0f)
            {
                currentPhase = RoundPhase.ROUND_RUNNING;
            }

            yield return null;
        }

        ActiveLogicRoutine = null;
    }

    protected virtual IEnumerator RoundRunningLogic()
    {
        //Probably manage scoreboard stuff here
        while(currentPhase == RoundPhase.ROUND_RUNNING)
        {
            if (roundElapsedTime >= roundRunningEndTime && roundRunningEndTime > 0.0f)
            {
                currentPhase = RoundPhase.ROUND_ENDING;
            }

            yield return null;
        }

        ActiveLogicRoutine = null;
    }

    protected virtual IEnumerator RoundEndingLogic()
    {
        spawnedMom = Instantiate(momPrefab);
        Mom momClass = spawnedMom.GetComponent<Mom>();
        momClass.FindPlayers(myManager.activePlayers);
        momClass.huntChildren = true;

        while(currentPhase == RoundPhase.ROUND_ENDING)
        {
            if(momClass.RemainingPlayersToFind.Count <= 0)
            {
                currentPhase = RoundPhase.ROUND_OVER;
            }
            yield return null;
        }
        
        ActiveLogicRoutine = null;
    }

    protected virtual IEnumerator RoundOverLogic()
    {
        for(float timer = 0.0f; timer < timeBeforeReturningToMenu; timer += Time.deltaTime)
        {
            yield return null;
        }

        SceneManager.LoadScene(MainMenuBuildIndex);
        ActiveLogicRoutine = null;
    }
    #endregion
}
