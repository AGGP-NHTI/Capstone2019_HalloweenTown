﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    public float health;
    public bool ghostUlt = false;
    Pawn pawn;

    SoundManager soundManager;
    ParticleManager particleManager;
    
    public ParticleSystem[] eggSplaters = new ParticleSystem[8];
    int eggArea = 0;
    //public GameObject dropCandy;//The candy
    //public Transform player;//get player transform
    //public ParticleSystem system;//particles

    void Start () {
        pawn = GetComponent<Pawn>();
        soundManager = GetComponent<SoundManager>();
        particleManager = GetComponent<ParticleManager>();
        
    }

    private void Update()
    {
        healthBar.fillAmount = health/100;
    }

    public void TakeDamage(float amount) // changed - needs testing
    {
        if (amount < 40)
        {
            if (eggArea >= 0)
            {
                eggSplaters[eggArea].Play();
                eggArea++;
            }

            if (eggArea >= 8)
            {
                eggArea = 0;
            }
        }
        if (pawn.myMask.hasMask)
        {
            if (health >= 0)//|| !ghostUlt)//if ghost is not ulting
            {
                health -= amount;
                soundManager.Oof();
                particleManager.hitPart();

            }
        }
        else
        {
            pawn.MyCandy.DropCandy();
        }
        
    }

    public void HealHealth(float amount)
    {
        health += amount;
        
    }

    public void HitOof()
    {
        soundManager.Oof();
    }
    //Causes Candy to Drop and particles
    //public void DropCandy(int numCandy)
    //{
    //    for (int i = 0; i < numCandy; i++)
    //    {
    //        system.Play();
    //        Vector3 pos = new Vector3(0f, 2f, 0f);
    //        GameObject candy;
    //        candy = Instantiate(dropCandy, transform.position + pos, transform.rotation);
    //    }
    //}

    //private void Update()
    // {
    /*if (health != 110)
    {
        HealHealth(1f);
    }  */
    // }

}
