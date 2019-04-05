using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Projectile {

   
    public float throwForce = 25;
    public float upMultiplier = 2;
    public float arcAfterElapsedTime = 5;
    public float damage = 10.0f;
    public static Vector3 moveSpeed = Vector3.zero;
    bool startFalling = false;
    Vector3 fallSpeed = new Vector3(0, -1, 0);

    public List<Vector3> parabolaPoints = new List<Vector3>();
    float speed = 500f;

    void Start () {
        StartCoroutine(ThrowEgg());

        //rb.useGravity = true;
        //velocityXZ = (transform.forward * throwForce) + moveSpeed;
        //velocityY = transform.up * upMultiplier;

        //addVelocity();

        //StartCoroutine(turnOnGravity());

    }

    // Update is called once per frame
    new void Update () {

        base.Update();

        //StartCoroutine(turnOnGravity());
        /*if (startFalling)
        {
            rb.AddForce(fallSpeed);
            fallSpeed.y *= Time.deltaTime + 1f;
        }*/
        
        //if (elapsedTime >= arcAfterElapsedTime)            


    }
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            HealthBar hb = player.GetComponent<HealthBar>();
            hb.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    IEnumerator ThrowEgg()
    {
        for (int i = 1; i < parabolaPoints.Count; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, parabolaPoints[i], speed * Time.deltaTime);
            //gameObject.transform.position = throwyPoints[i];
            if (gameObject.transform.position == parabolaPoints[parabolaPoints.Count - 1])
            {
                Destroy(gameObject);
            }
            yield return null;// new WaitForSeconds(0.2f);
        }
    }

    /*IEnumerator turnOnGravity()
    {
        float timer = arcAfterElapsedTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        startFalling = true;
        //rb.useGravity = true;
        
        
    }*/
}
