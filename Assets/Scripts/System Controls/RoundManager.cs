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
    public RoundPhase currentPhase = RoundPhase.PRE_GAME;
    protected Coroutine ActiveLogicRoutine;

    public float roundElapsedTime = 0.0f;
    public float roundStartEndTime = -1.0f;
    public float roundRunningEndTime = -1.0f;
    public float roundEndingEndTime = -1.0f;
    protected bool _letRoundTimerRun = false;

    protected List<InputObject> _activeInputs;
    protected List<PlayerController> _activePlayers;
    #endregion

    #region Specific Phase Variables
    [Header("Round Starting")]
    public int GameSceneBuildIndex = 1;
    public GameObject playercontrollerPrefab;
    public GameObject playerPrefab;

    [Header("Round Ending")]
    public GameObject momPrefab;
    [HideInInspector] public GameObject spawnedMom;

    [Header("Round Over")]
    public float timeBeforeReturningToMenu = 5.0f;
    public int MainMenuBuildIndex = 0;
    #endregion

    protected virtual void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public virtual void StartRound(List<InputObject> ParticipatingPlayers)
    {
        currentPhase = RoundPhase.PRE_GAME;
        _activeInputs = ParticipatingPlayers;
    }

    #region Round Logic
    protected virtual void FixedUpdate()
    {
        if(_letRoundTimerRun)
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
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(GameSceneBuildIndex);

        while (!loadingOperation.isDone)
        {
            Debug.Log("Loading progress: " + loadingOperation.progress);
            yield return null;
        }

        currentPhase = RoundPhase.ROUND_STARTING;
        ActiveLogicRoutine = null;
    }

    protected virtual IEnumerator RoundStartingLogic()
    {
        SpawnPlayers(_activeInputs);

        currentPhase = RoundPhase.ROUND_RUNNING;

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
        momClass.FindPlayers(_activePlayers);
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
        Destroy(gameObject);
    }
    #endregion

    #region Extra Utility
    protected void SpawnPlayers(List<InputObject> inputObjects)
    {
        _activePlayers = new List<PlayerController>();

        for (int i = 0; i < inputObjects.Count; i++)
        {
            GameObject spawnedBoy = Instantiate(playercontrollerPrefab, Vector3.zero, Quaternion.identity);
            PlayerController pc = spawnedBoy.GetComponent<PlayerController>();
            pc.playerInput = inputObjects[i];

            SpawnPoint.GetRandomValidSpawn().SpawnPlayer(pc, playerPrefab);

            _activePlayers.Add(pc);
        }

        SplitScreenManager.Instance.ConfigureScreenSpace();
    }
    #endregion
}
