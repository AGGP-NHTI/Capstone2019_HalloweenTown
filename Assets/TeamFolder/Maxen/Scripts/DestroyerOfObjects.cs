using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class DestroyerOfObjects : MonoBehaviour
{
    public static void DestroyObject(GameObject obj)
    {
        DestroyerOfObjects doo = obj.GetComponent<DestroyerOfObjects>();
        if(doo)
        {
            doo.DestroyMe();
        }
        else
        {
            Destroy(obj);
        }
    }

    public virtual void DestroyMe()
    {
        if(PhotonNetwork.OfflineMode)
        {
            Destroy(gameObject);
        }
        else
        {
            PhotonView pv = GetComponent<PhotonView>();
            if (pv)
            {
                pv.RPC("NetworkedDestroy", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    protected virtual void NetworkedDestroy()
    {
        Destroy(gameObject);
    }
}
