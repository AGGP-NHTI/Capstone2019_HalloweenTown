using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ControllerRPC : MonoBehaviour {

    //public Manager manager;
   // public PhotonManager photonManager;

	void Start () {
        if (PhotonNetwork.IsMasterClient)
        {
             Manager.instance.photonManager.MasterPhotonView = gameObject.GetPhotonView();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [PunRPC]
    public void ReadyUp(int i)
    {
        Manager.instance.readyUp[i] = true;
    }

    [PunRPC]
    public void JoinedGame(int i)
    {
        Manager.instance.joinedGame[i] = true;
    }

    [PunRPC]
    public void NotReadyUp(int i)
    {
        Manager.instance.readyUp[i] = false;
    }

    [PunRPC]
    public void NotJoinedGame(int i)
    {
        Manager.instance.joinedGame[i] = false;
    }

    [PunRPC]
    public void StartGameCountdown()
    {
        if (Manager.instance.RoundReadyToStart)
        {
            if (Manager.instance.startGameTimer == null)
            {
                Debug.Log("timer");
                Manager.instance.startGameTimer = StartCoroutine(Manager.instance.BeginGameCountDown());
            }
        }
        else if (Manager.instance.startGameTimer != null)
        {
            StopCoroutine(Manager.instance.startGameTimer);

            Manager.instance.startGameTimer = null;
        }
    }

    [PunRPC]
    public void EndGameCountdown()
    {
        if (Manager.instance.startGameTimer != null)
        {
            StopCoroutine(Manager.instance.startGameTimer);

            Manager.instance.startGameTimer = null;
        }
    }

    [PunRPC]
    public void RoundReadyToStart(bool check1, bool check2)
    {
        Manager.instance.RoundReadyToStart = check1 && check2;
    }
    
}
