﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    
    public float startHealth = 100;
    public float health;
	
	void Start () {

        health = startHealth;

	}

    public void TakeDamage(float amount) // this is how the damage needs to work for fillAmount.
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

    }

    private void Update()
    {
        //health -= 10;
        //healthBar.fillAmount = health / startHealth;
    }

}
