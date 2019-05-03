using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBagSpawner : MonoBehaviour {

    public List<GameObject> bags;
    Pawn pawn;
    public GameObject myBag;
    bool setbag = true;
	void Start () {
        pawn = GetComponent<Pawn>();
        
    }
    
    void Update () {

        if(pawn.MyController != null && setbag)
        {
            if(PhotonNetwork.OfflineMode)
            {
                myBag = Instantiate(bags[(int)pawn.MyController.PlayerNumber - 1]);
                myBag.transform.SetParent(pawn.LhandBagSpawn.transform, false);
                setbag = false;
            }
            else
            {
                setbag = false;

                //myBag.transform.SetParent(pawn.LhandBagSpawn.transform);
                if (gameObject.GetPhotonView().IsMine)
                {
                    gameObject.GetPhotonView().RPC("NetworkSetParent", RpcTarget.AllBuffered);
                }
            }
            
        }
    }

    [PunRPC]
    void NetworkSetParent()
    {
        //myBag.transform.parent = pawn.LhandBagSpawn.transform;
        myBag = PhotonNetwork.Instantiate(bags[GetPhotonPlayerIndex()].name, pawn.LhandBagSpawn.transform.position, pawn.LhandBagSpawn.transform.rotation);
        myBag.transform.SetParent(pawn.LhandBagSpawn.transform);
    }

    int GetPhotonPlayerIndex()
    {
        if (PhotonNetwork.NickName == "Player 1")
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
}
