using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class NetworkedRoundManager : RoundManager
{
    #region Room Property Names
    public const string ROOMPROPERTY_ROUNDPHASE = "enum_RoundPhase";
    public const string ROOMPROPERTY_TIMESTART = "double_RoundStartTime";
    #endregion
    #region Player Property Names
    public const string PLAYERPROPERTY_LEVELLOADED = "bool_LevelLoaded";
    public const string PLAYERPROPERTY_INTROCUTSCENEDONE = "bool_IntroCutsceneComplete";
    public const string PLAYERPROPERTY_CANDYSCORE = "int_CandyScore";
    #endregion

    protected Pawn LocalPlayerPawn;

    protected override void FixedUpdate()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            MasterClientOperations();
        }
        LocalClientOperations();

        if (ActiveLogicRoutine == null)
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

    protected virtual void MasterClientOperations()
    {
        ExitGames.Client.Photon.Hashtable newProperties = new ExitGames.Client.Photon.Hashtable
            {
                { ROOMPROPERTY_ROUNDPHASE, currentPhase }
            };
        PhotonManager.photonInstance.RoomProperties = newProperties;
    }

    protected virtual void LocalClientOperations()
    {
        object roundPhaseObject;
        if(PhotonManager.photonInstance.RoomProperties.TryGetValue(ROOMPROPERTY_ROUNDPHASE, out roundPhaseObject))
        {
            if((RoundPhase)roundPhaseObject != currentPhase)
            {
                currentPhase = (RoundPhase)roundPhaseObject;
            }
        }

        object startTimeObj;
        if(PhotonManager.photonInstance.RoomProperties.TryGetValue(ROOMPROPERTY_TIMESTART, out startTimeObj))
        {
            roundElapsedTime = (float)(PhotonNetwork.Time - (double)startTimeObj);
        }

        if(LocalPlayerPawn)
        {
            ExitGames.Client.Photon.Hashtable newProperties = new ExitGames.Client.Photon.Hashtable
            {
                { PLAYERPROPERTY_CANDYSCORE, LocalPlayerPawn.MyCandy.candy }
            };

            PhotonNetwork.SetPlayerCustomProperties(newProperties);
        }

        Player[] allPlayers = PhotonNetwork.PlayerList;
        if (Candy.Scoreboard == null)
        {
            Candy.Scoreboard = new Dictionary<uint, int>();
        }
        else
        {
            Candy.Scoreboard.Clear();
        }
        for(int i = 0; i < allPlayers.Length; i++)
        {
            object candyObj;
            if(allPlayers[i].CustomProperties.TryGetValue(PLAYERPROPERTY_CANDYSCORE, out candyObj))
            {
                Candy.Scoreboard.Add((uint)(i + 1), (int)candyObj);
            }
        }
    }

    #region Round Logic
    protected override IEnumerator PreGameLogic()
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(GameSceneBuildIndex);

        while(!loadingOperation.isDone)
        {
            yield return null;
        }

        ExitGames.Client.Photon.Hashtable newPlayerPropertiy = new ExitGames.Client.Photon.Hashtable
        {
            { PLAYERPROPERTY_LEVELLOADED, true }
        };
        PhotonNetwork.SetPlayerCustomProperties(newPlayerPropertiy);

        while(currentPhase == RoundPhase.PRE_GAME)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Player[] allPlayers = PhotonNetwork.PlayerList;
                bool allPlayersLoaded = true;
                foreach(Player p in allPlayers)
                {
                    object isLoadedObject;
                    if (p.CustomProperties.TryGetValue(PLAYERPROPERTY_LEVELLOADED, out isLoadedObject))
                    {
                        if (!(bool)isLoadedObject)
                        {
                            allPlayersLoaded = false;
                        }
                    }
                    else
                    {
                        allPlayersLoaded = false;
                    }
                }

                if(allPlayersLoaded)
                {
                    ExitGames.Client.Photon.Hashtable newRoomProperty = new ExitGames.Client.Photon.Hashtable
                    {
                        { ROOMPROPERTY_TIMESTART, PhotonNetwork.Time }
                    };
                    PhotonManager.photonInstance.RoomProperties = newRoomProperty;

                    currentPhase = RoundPhase.ROUND_STARTING;
                }
            }

            yield return null;
        }

        ActiveLogicRoutine = null;
    }

    protected override IEnumerator RoundStartingLogic()
    {
        Cursor.visible = false;

        CameraManager.Instance.StartCutscene(LevelInfo.GetIntroCutscene(), ConfirmCutsceneEnd);

        while (currentPhase == RoundPhase.ROUND_STARTING)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Player[] allPlayers = PhotonNetwork.PlayerList;
                bool allIntroCutscenesDone = true;
                foreach (Player p in allPlayers)
                {
                    object cutsceneDoneObject;
                    if (p.CustomProperties.TryGetValue(PLAYERPROPERTY_INTROCUTSCENEDONE, out cutsceneDoneObject))
                    {
                        if (!(bool)cutsceneDoneObject)
                        {
                            allIntroCutscenesDone = false;
                        }
                    }
                    else
                    {
                        allIntroCutscenesDone = false;
                    }
                }

                if (allIntroCutscenesDone)
                {
                    currentPhase = RoundPhase.ROUND_RUNNING;
                }
            }

            yield return null;
        }

        SpawnPlayers(_activeInputs);
        ActiveLogicRoutine = null;
    }

    protected override IEnumerator RoundRunningLogic()
    {
        while (currentPhase == RoundPhase.ROUND_RUNNING)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if(roundElapsedTime >= roundRunningEndTime && roundRunningEndTime > 0.0f)
                {
                    currentPhase = RoundPhase.ROUND_ENDING;
                }
            }

            yield return null;
        }

        ActiveLogicRoutine = null;
    }

    protected override IEnumerator RoundEndingLogic()
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

        while(currentPhase == RoundPhase.ROUND_ENDING)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                if(_momInScene.DoneHunting())
                {
                    currentPhase = RoundPhase.ROUND_OVER;
                }
            }

            yield return null;
        }

        ActiveLogicRoutine = null;
    }
    #endregion

    #region Extra Utility
    protected override void SpawnPlayers(List<InputObject> inputObjects)
    {
        GameObject spawnedBoy = Instantiate(playercontrollerPrefab, Vector3.zero, Quaternion.identity);
        PlayerController pc = spawnedBoy.GetComponent<PlayerController>();
        pc.playerInput = inputObjects[0];
        SpawnPoint spawnpoint = SpawnPoint.GetRandomValidSpawn();

        GameObject actualChild = spawnpoint.SpawnPlayer(pc, playerPrefab);
        LocalPlayerPawn = actualChild.GetComponent<Pawn>();

        //GameObject actualChild = spawnpoint.SpawnPlayer(pc, playerPrefab);

        /*for (int j = 0; j < Manager.managerInstance.photonManager.PhotonObjects.Count; j++)
        {
            if (Manager.managerInstance.photonManager.PhotonObjects[j].GetPhotonView().IsMine)
            {
                GameObject spawnedBoy = Instantiate(playercontrollerPrefab, Vector3.zero, Quaternion.identity);
                PlayerController pc = spawnedBoy.GetComponent<PlayerController>();
                pc.playerInput = inputObjects[0];
                SpawnPoint spawnpoint = SpawnPoint.GetRandomValidSpawn();
                if (spawnpoint != null)
                {
                    Debug.Log("WORKED");
                }
                GameObject actualChild = spawnpoint.SpawnPlayer(pc, playerPrefab);

                Manager.managerInstance.photonManager.PhotonObjects[j].transform.position = actualChild.transform.position;
                actualChild.transform.SetParent(Manager.managerInstance.photonManager.PhotonObjects[j].transform);
                // Manager.managerInstance.photonManager.PhotonObjects[j].GetPhotonView().RPC("AddActivePlayer", RpcTarget.All);

                // _activePlayers = Manager.managerInstance.photonManager.PhotonObjects;
                //SplitScreenManager.Instance.ConfigureScreenSpace();
            }
        }*/
    }

    protected virtual void ConfirmCutsceneEnd()
    {
        BuildConsole.WriteLine("Cutscene ending");
        ExitGames.Client.Photon.Hashtable newProperties = new ExitGames.Client.Photon.Hashtable
        {
            { PLAYERPROPERTY_INTROCUTSCENEDONE, true }
        };
        PhotonNetwork.SetPlayerCustomProperties(newProperties);
    }

    /*public override void DisplayScores()
    {
        Text[] playerscores = LevelInfo.GetPlayerScores();
        Player[] allPlayers = PhotonNetwork.PlayerList;

        for(int i = 0; i < playerscores.Length && i < allPlayers.Length; i++)
        {
            object candyObj;
            if(allPlayers[i].CustomProperties.TryGetValue(PLAYERPROPERTY_CANDYSCORE, out candyObj))
            {
                if(candyObj is int)
                {
                    playerscores[i].enabled = true;
                    playerscores[i].text = allPlayers[i].NickName + ": " + (int)candyObj;
                }
            }
        }

        Canvas scoreboard = LevelInfo.GetScoreBoard();
        scoreboard.gameObject.SetActive(true);
    }*/
    #endregion
}