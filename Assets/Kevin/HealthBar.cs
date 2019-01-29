using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    
    public float startHealth = 100;
    public float health;
	
	void Start () {

        health = 0f;

	}

    public void TakeDamage(float amount) // this is how the damage needs to work for fillAmount.
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

    }
    public void HealHealth(float amount) // this is how the damage needs to work for fillAmount.
    {
        health += amount;

        healthBar.fillAmount = health / startHealth;

    }

    private void Update()
    {
        if (health != 110)
        {
            HealHealth(1f);
        }      
    }

}
