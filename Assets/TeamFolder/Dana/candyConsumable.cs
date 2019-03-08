using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candyConsumable : MonoBehaviour {

    public bool canCollect = false;
    private void Start()
    {
        Destroy(gameObject, 10f);
        StartCoroutine(wait());
        gameObject.GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 10;
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Candy>() && canCollect)
        {
            GameObject player = collision.gameObject;
            player.GetComponent<Candy>().candy += 1;
            Destroy(gameObject);
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        canCollect = true;
    }
    
}
