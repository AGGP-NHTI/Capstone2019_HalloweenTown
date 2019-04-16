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
            gameObject.GetPhotonView().RPC("SetMaster", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void SetMaster()
    {
        Manager.managerInstance.photonManager.MasterPhotonView = gameObject.GetPhotonView();
    }

    [PunRPC]
    public void ReadyUp(int i)
    {
        Manager.managerInstance.readyUp[i] = true;
    }

    [PunRPC]
    public void JoinedGame(int i)
    {
        Manager.managerInstance.joinedGame[i] = true;
    }

    [PunRPC]
    public void NotReadyUp(int i)
    {
        Manager.managerInstance.readyUp[i] = false;
    }

    [PunRPC]
    public void NotJoinedGame(int i)
    {
        Manager.managerInstance.joinedGame[i] = false;
    }

    [PunRPC]
    public void StartGameCountdown()
    {
        if (Manager.managerInstance.RoundReadyToStart)
        {
            if (Manager.managerInstance.startGameTimer == null)
            {
                Debug.Log("timer");
                Manager.managerInstance.startGameTimer = StartCoroutine(Manager.managerInstance.BeginGameCountDown());
            }
        }
        else if (Manager.managerInstance.startGameTimer != null)
        {
            StopCoroutine(Manager.managerInstance.startGameTimer);

            Manager.managerInstance.startGameTimer = null;
        }
    }

    [PunRPC]
    public void EndGameCountdown(int index)
    {
        if (Manager.managerInstance.startGameTimer != null)
        {
            StopCoroutine(Manager.managerInstance.startGameTimer);
            
            Manager.managerInstance.startGameTimer = null;            
        }
    }

    [PunRPC]
    public void ReadyToStart(bool check1, bool check2)
    {
        Manager.managerInstance.RoundReadyToStart = check1 && check2;
    }

    [PunRPC]
    public void RemovePhotonObject(int index)
    {
       PhotonManager.photonInstance.PhotonObjects.RemoveAt(index);
    }    
    
}
