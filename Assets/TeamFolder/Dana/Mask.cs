using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{

    float startHealth;
    bool hasMask = false;
    public bool ultButton = false;
    Pawn pawn;
    HealthBar hbar;
    GhostMask gMask;

    void Start()
    {
        pawn = gameObject.GetComponent<Pawn>();
        hbar = GetComponent<HealthBar>();
        gMask = gameObject.AddComponent<GhostMask>();//test script
    }


    void Update()
    {

        if (ultButton && hasMask == false)
        {
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 10f);//for testing
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Mask")
                {
                    hasMask = true;
                    UpdateHealth();
                    Destroy(hitColliders[i].gameObject);
                }
            }
        }

        if (ultButton && hasMask)
        {
            Color color = GetComponent<MeshRenderer>().material.color;
            color.a -= 0;
            GetComponent<MeshRenderer>().material.color = color;
        }

        if (hbar.health <= startHealth)
        {
            Destroy(gMask);
        }
    }

    void UpdateHealth()
    {
        startHealth = hbar.health;
        hbar.health += 50;
    }
}
