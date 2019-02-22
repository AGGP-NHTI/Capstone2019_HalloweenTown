﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : Projectile {

   
    public float throwForce = 500;
    public float upMultiplier = 2;
    public float arcAfterElapsedTime = 5;
    public float damage = 10.0f;
    public static Vector3 moveSpeed = Vector3.zero;
    bool startFalling = false;
    Vector3 fallSpeed = new Vector3(0, -1, 0);
   
	void Start () {

        rb.useGravity = true;
        velocityXZ = (transform.forward * throwForce) + moveSpeed;
        velocityY = transform.up * upMultiplier;
        
        addVelocity();

        //StartCoroutine(turnOnGravity());

    }

    // Update is called once per frame
    new void Update () {

        base.Update();

        StartCoroutine(turnOnGravity());
        if (startFalling)
        {
            rb.AddForce(fallSpeed);
            fallSpeed.y *= Time.deltaTime + 1f;
        }
        
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

    IEnumerator turnOnGravity()
    {
        float timer = arcAfterElapsedTime;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        startFalling = true;
        //rb.useGravity = true;
        
        
    }
}
