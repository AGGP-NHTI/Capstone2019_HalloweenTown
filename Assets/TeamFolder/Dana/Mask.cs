using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{

    float startHealth;
    bool hasMask = false;
    public bool ultButton = false;
    public bool help;
    Pawn pawn;
    HealthBar hbar;
    public GameObject playerPref;
    public GameObject ghostPref;
    public GameObject currentModel;

    void Start()
    {
        pawn = gameObject.GetComponent<Pawn>();
        hbar = GetComponent<HealthBar>();
    }


    void Update()
    {
        if (ultButton && hasMask == false)
        {
            Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 10f);//for testing
            
            for(int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].tag == "Mask")
                {
                    hasMask = true;
                    UpdateHealth();
                    gameObject.AddComponent<GhostMask>();//test script

                    Destroy(hitColliders[i].gameObject);
                    Vector3 trans = currentModel.transform.position;
                    Quaternion rot = currentModel.transform.rotation;
                    Destroy(currentModel);

                    GameObject mask = Instantiate(ghostPref, gameObject.transform);
                    mask.transform.position = trans;
                    mask.transform.rotation = rot;
                    gameObject.transform.parent = mask.transform;
                    AlignToMovement al = mask.GetComponent<AlignToMovement>();
                    al.TrackedRigidBody = gameObject.GetComponent<Rigidbody>();
                    currentModel = mask;
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
            Destroy(gameObject.GetComponent<GhostMask>());
            Destroy(currentModel);

            GameObject mask = Instantiate(playerPref, gameObject.transform);
            currentModel = mask;
        }
    }

    void UpdateHealth()
    {
        startHealth = hbar.health;
        hbar.health += 50;
    }
}
