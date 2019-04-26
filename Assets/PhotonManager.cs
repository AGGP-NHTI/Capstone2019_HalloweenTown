using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

//using UnityEngine.Networking;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager photonInstance;

    public string verNum = "0.1";
    public string roomName = "Lobby1";

    public GameObject playerPrefab;

    public bool isConnectedToLobby = false; //ready up menu
    public bool isConnectedToGame = false; //actual game
    public bool connectedtomaster = false;

    public bool connecting = false;

    public static Manager manager;

    public Text test;

    public GameObject playerControllerPref;
    public List<GameObject> PhotonObjects;
    public PhotonView MasterPhotonView;

    

    public void Start()
    {
        if (photonInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
        }


        photonInstance = this;
    }

    public void Update()
    {

        if (!isConnectedToGame)
        {
            if (isConnectedToLobby)
            {

                int i = 0;
                /*test.text = "";
                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    test.text += p.NickName + " - ";
                    i++;
                }*/
                if (PhotonNetwork.CurrentRoom.PlayerCount > 2)
                {
                    //PhotonNetwork.LoadLevel(2);
                    isConnectedToGame = true;
                    //SpawnPlayer();
                }
            }
        }

    }

    public void SpawnController()
    {
        Debug.Log("SpawnController");
        PhotonObjects.Add(PhotonNetwork.Instantiate(playerControllerPref.name, Vector3.zero, transform.rotation) as GameObject);
        
    }

    public void OnlineStart()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.ConnectUsingSettings())
        {
            Debug.Log("Starting Connection");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        connectedtomaster = true;
    }

    public void JoinLobby() //on button press
    {
        if (connectedtomaster)
        {
            OnJoinedLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        RoomOptions rm = new RoomOptions();
        rm.MaxPlayers = 4;

        //PhotonNetwork.LoadLevel("WaitRoom");

        if (PhotonNetwork.JoinOrCreateRoom(roomName, rm, null) == false)
        {
            roomName = "Lobby" + PhotonNetwork.CountOfRooms;
            PhotonNetwork.JoinOrCreateRoom(roomName, rm, null);
        }
        Debug.Log("joined lobby");


    }

    public override void OnJoinedRoom()//use to spawn here
    {
        /*if (MasterPhotonView.IsMine)
        {
            Debug.Log("master");
            Players.Add(gameObject);
            photonView = MasterPhotonView;
        }
        else
        {
           // MasterPhotonView.RPC("OnPhotonPlayerConnected", RpcTarget.Others);
        }   */
        


        isConnectedToLobby = true;
        Debug.Log("Joined Room");
        PhotonNetwork.NickName = "Player " + PhotonNetwork.PlayerList.Length.ToString();
        SpawnController();
    }

}

