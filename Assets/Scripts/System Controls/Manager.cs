using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    public static Manager managerInstance;
    public PhotonManager photonManager;
    public GameObject RoundModePrefab;

    public List<InputObject> inputObject;
    /*[HideInInspector]*/ public List<bool> joinedGame = new List<bool>(new bool[] {false, false, false, false});
    /*[HideInInspector]*/ public List<bool> readyUp = new List<bool>(new bool[] { false, false, false, false });
    public float CountDownDuration { get; private set; }
    public bool RoundReadyToStart;// { get; set; }
    
    public Coroutine startGameTimer;

    protected bool lookForJoining = false;
	void Start ()
    {
        managerInstance = this;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}
	
	void Update ()
    {
        if (lookForJoining)
        {
            bool allPlayersReady = true;
            bool onePlayerInGame = false;
            
            if (!PhotonNetwork.OfflineMode)
            {
                //for (int j = 0; j < inputObject.Count; j++)
                //{


                    for (int i = 0; i < photonManager.PhotonObjects.Count; i++)//make loading screen
                    {
                        if (photonManager.PhotonObjects[i] != null)
                        {
                        if (PhotonNetwork.PhotonViews[i].IsMine)
                        {
                            int index = GetPhotonPlayerIndex();

                            //test if trying to Joined Game                
                            if (inputObject[0].GetStartInput())//PhotonNetwork.PhotonViews[i].GetComponent<PlayerController>().playerInput.GetStartInput())
                            {
                                PhotonNetwork.PhotonViews[i].RPC("JoinedGame", RpcTarget.AllBuffered, index);
                            }

                            //test if trying to Ready Up
                            if (inputObject[0].GetJumpInput() && joinedGame[i])//(PhotonNetwork.PhotonViews[i].GetComponent<PlayerController>().playerInput.GetJumpInput() && joinedGame[i])
                            {
                                PhotonNetwork.PhotonViews[i].RPC("ReadyUp", RpcTarget.AllBuffered, index);
                            }

                            //test if trying to Unready/Unjoin
                            if (inputObject[0].GetBooInput())//PhotonNetwork.PhotonViews[i].GetComponent<PlayerController>().playerInput.GetBooInput())
                            {

                                if (readyUp[index])
                                {
                                    Debug.Log("unreadyup");
                                    PhotonNetwork.PhotonViews[i].RPC("NotReadyUp", RpcTarget.AllBuffered, index);
                                }
                                else if (joinedGame[index])
                                {
                                    PhotonNetwork.PhotonViews[i].RPC("NotJoinedGame", RpcTarget.AllBuffered, index);
                                }
                            }
                        }
                        }

                        if (joinedGame[GetPhotonPlayerIndex()])
                        {
                            onePlayerInGame = true;
                            if (!readyUp[i])
                            {
                                allPlayersReady = false;
                            }
                        }
                    }
                //}
            }
            else//local game
            {
                for (int i = 0; i < inputObject.Count; i++)
                {
                    if (inputObject[i].GetStartInput())
                    {
                        joinedGame[i] = true;
                        Debug.Log("Player " + i + " joined the game!");
                    }

                    //test if trying to Ready Up
                    if (inputObject[i].GetJumpInput() && joinedGame[i])
                    {
                        readyUp[i] = true;
                        Debug.Log("Player " + i + " is ready!");
                    }

                    //test if trying to Unready/Unjoin
                    if (inputObject[i].GetBooInput())
                    {
                        if (readyUp[i])
                        {
                            readyUp[i] = false;
                        }
                        else if (joinedGame[i])
                        {
                            joinedGame[i] = false;
                        }
                    }

                    if (joinedGame[i])
                    {
                        onePlayerInGame = true;
                        if (!readyUp[i])
                        {
                            allPlayersReady = false;
                        }
                    }
                }
            }
            //Debug.Log("oneplayer " + onePlayerInGame.ToString());           
            if (PhotonNetwork.OfflineMode == false)
            {     
                if (photonManager.isConnectedToLobby && photonManager.MasterPhotonView.IsMine)
                {
                    photonManager.MasterPhotonView.RPC("ReadyToStart", RpcTarget.AllBuffered, allPlayersReady, onePlayerInGame);
                    photonManager.MasterPhotonView.RPC("StartGameCountdown", RpcTarget.AllBuffered);
                }
            }
            else
            {
                RoundReadyToStart = allPlayersReady && onePlayerInGame;
                if (RoundReadyToStart)
                {
                    if (startGameTimer == null)
                    {
                        Debug.Log("timer");
                        startGameTimer = StartCoroutine(BeginGameCountDown());
                    }
                }
                else if (startGameTimer != null)
                {
                    StopCoroutine(startGameTimer);

                    startGameTimer = null;
                }
            }
        }        
	}

    int GetPhotonPlayerIndex()
    {
        if(PhotonNetwork.NickName == "Player 1")
        {
            return 0;
        }
        else if (PhotonNetwork.NickName == "Player 2")
        {
            return 1;
        }
        else if (PhotonNetwork.NickName == "Player 3")
        {
            return 2;
        }
        else if (PhotonNetwork.NickName == "Player 4")
        {
            return 3;
        }
        return -1;
    }

    public IEnumerator BeginGameCountDown()
    {      
        CountDownDuration = 5f; 
        float endTime = 0;
        while (CountDownDuration >= endTime)
        {
            //countDown.text = Mathf.Round(duration).ToString();
            CountDownDuration -= Time.deltaTime;
            yield return null;
        }
        //countDown.text = Mathf.Round(Time.time).ToString();
        lookForJoining = false;
        //This is when the game actually begins.
        if(RoundModePrefab)
        {
            GameObject spawnedObj = Instantiate(RoundModePrefab);
            RoundManager roundMode = spawnedObj.GetComponent<RoundManager>();

            if(roundMode)
            {
                List<InputObject> participatingPlayers = new List<InputObject>();
                for (int i = 0; i < inputObject.Count; i++)
                {
                    if (joinedGame[i] && readyUp[i])
                    {
                        participatingPlayers.Add(inputObject[i]);
                    }
                }
                roundMode.StartRound(participatingPlayers);
            }
            else
            {
                Debug.LogWarning(spawnedObj + " prefab has no RoundManager component!");
            }
        }
        else
        {
            Debug.LogWarning(name + " has no RoundModePrefab!");
        }
    }

    public void StartLookingForPlayers()
    {
        lookForJoining = true;
        joinedGame = new List<bool>(new bool[] { false, false, false, false });
        readyUp = new List<bool>(new bool[] { false, false, false, false });
    }

    public void StopLookingForPlayers()
    {
        lookForJoining = false;
        photonManager.connectedtomaster = false;

        if (!PhotonNetwork.OfflineMode)
        {
            int index = GetPhotonPlayerIndex();
            photonManager.MasterPhotonView.RPC("EndGameCountdown", RpcTarget.AllBuffered, index);
            PhotonNetwork.PhotonViews[index].RPC("NotReadyUp", RpcTarget.AllBuffered, index);
            PhotonNetwork.PhotonViews[index].RPC("NotJoinedGame", RpcTarget.AllBuffered, index);

            for (int i = 0; i < photonManager.PhotonObjects.Count; i++)
            {
                if (photonManager.PhotonObjects[i].GetPhotonView().IsMine)
                {
                    photonManager.PhotonObjects[i].GetPhotonView().RPC("RemovePhotonObject", RpcTarget.All, index);
                }
            }

            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
            photonManager.isConnectedToLobby = false;
            
            
        }
        else
        {
            if (startGameTimer != null)
            {
                StopCoroutine(startGameTimer);

                startGameTimer = null;
            }
        }
    }
}
