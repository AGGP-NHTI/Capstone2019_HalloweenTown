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
                myBag = PhotonNetwork.Instantiate(bags[(int)pawn.MyController.PlayerNumber - 1].name, pawn.LhandBagSpawn.transform.position, pawn.LhandBagSpawn.transform.rotation);
                myBag.transform.SetParent(pawn.LhandBagSpawn.transform);
                setbag = false;
            }
            
        }
    }
}
