﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {

    public static RoundManager Instance;

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
    [SerializeField]protected bool _letRoundTimerRun = false;

    protected List<InputObject> _activeInputs;
    protected List<PlayerController> _activePlayers;

    #endregion

    #region Specific Phase Variables
    [Header("Round Starting")]
    public int GameSceneBuildIndex = 1;
    public GameObject playercontrollerPrefab;
    public GameObject playerPrefab;

    //[Header("Round Ending")]
    protected MomPawn _momInScene;

    [Header("Round Over")]
    public float timeBeforeReturningToMenu = 5.0f;
    public int MainMenuBuildIndex = 0;
    #endregion

    protected virtual void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
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
            //Debug.Log("Loading progress: " + loadingOperation.progress);
            yield return null;
        }

        currentPhase = RoundPhase.ROUND_STARTING;
        _letRoundTimerRun = true;
        ActiveLogicRoutine = null;
    }

    protected virtual IEnumerator RoundStartingLogic()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CameraManager.Instance.StartCutscene(LevelInfo.GetIntroCutscene(), SetRoundToRunning);

        while(currentPhase == RoundPhase.ROUND_STARTING)
        {
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
        _momInScene = LevelInfo.GetMom();
        _momInScene.gameObject.SetActive(true);

        BackgroundMusic.instance.MomMusic();

        GameObject momCutscene = LevelInfo.GetMomCutscene();
        if(momCutscene)
        {
            CameraManager.Instance.StartCutscene(momCutscene, ActivateMom);
        }
        else
        {
            ActivateMom();
        }

        while (currentPhase == RoundPhase.ROUND_ENDING)
        {
            if(_momInScene.DoneHunting())
            {
                currentPhase = RoundPhase.ROUND_OVER;
            }
            yield return null;
        }
        
        ActiveLogicRoutine = null;
    }

    protected virtual IEnumerator RoundOverLogic()
    {
        BackgroundMusic.instance.EndGameMusic();

        DisplayScores();

        //Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        for (float timer = 0.0f; timer < timeBeforeReturningToMenu; timer += Time.deltaTime)
        {
            yield return null;
        }

        SceneManager.LoadScene(MainMenuBuildIndex);
        ActiveLogicRoutine = null;
        Destroy(gameObject);
    }
    #endregion

    #region Extra Utility

    
    protected virtual void SpawnPlayers(List<InputObject> inputObjects)
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

    protected virtual void SetRoundToRunning()
    {
        SpawnPlayers(_activeInputs);
        currentPhase = RoundPhase.ROUND_RUNNING;
    }

    protected virtual void ActivateMom()
    {
        Pawn[] foundPawns = FindObjectsOfType<Pawn>();

        _momInScene.LetMomHunt(foundPawns);
    }

    public virtual PlayerController[] GetPlayers()
    {
        return _activePlayers.ToArray();
    }

    public virtual void DisplayScores()
    {
        Text[] playerscores = LevelInfo.GetPlayerScores();
        foreach (KeyValuePair<uint, int> kvp in Candy.Scoreboard)
        {
            playerscores[kvp.Key - 1].enabled = true;
            playerscores[kvp.Key - 1].text = "Player " + kvp.Key + ": " + kvp.Value.ToString();
        }

        Canvas scoreboard = LevelInfo.GetScoreBoard();
        scoreboard.gameObject.SetActive(true);
    }
    #endregion
}
