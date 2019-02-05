using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boo : MonoBehaviour {
    public AudioSource boo;
    public GameObject barrel;
    public GameObject Model;

    public int radiusOfBoo = 60;
    bool canBoo = true;

    private void Update()
    {
        Mask mask = gameObject.GetComponent<Mask>();
        Model = mask.currentModel;
        barrel = Model.GetComponent<GetBarrel>().barrel;
    }
    public void GoBoo(bool value)
    {
        if (value && canBoo)
        {            
            if(!boo.isPlaying)
            {
                Collider[] hitColliders = Physics.OverlapSphere(barrel.transform.position, 1.0f);
                
                for(int i = 0; i < hitColliders.Length; i++)
                {
                    if(hitColliders[i].tag == "Player")
                    {
                        if(hitColliders[i].transform.position != transform.position)
                        {
                            Debug.Log("Boo");

                            GameObject otherModel = hitColliders[i].GetComponent<Boo>().Model;//gets other model in boo
                            float difference = otherModel.transform.rotation.eulerAngles.y - Model.transform.rotation.eulerAngles.y;

                            //checks if the difference is a negative value
                            if(Mathf.Sign(difference) == -1)
                            {
                                difference *= -1;
                            }

                            if (difference <= radiusOfBoo)
                            {
                                Debug.Log("Got Booed");
                                HealthBar hb = hitColliders[i].GetComponent<HealthBar>();
                                hb.TakeDamage(50f);
                            }                            
                        }
                    }
                }
                boo.Play();
                canBoo = false;
                StartCoroutine(WaitingToBoo());
            }            
        }
    }

    IEnumerator WaitingToBoo()
    {
        yield return new WaitForSeconds(3);
        canBoo = true;
    }
}
