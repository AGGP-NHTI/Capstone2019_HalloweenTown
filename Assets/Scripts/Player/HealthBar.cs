using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    //public float startHealth = 100;
    public float health;
    SoundManager soundManager;
	void Start () {
        soundManager = GetComponent<SoundManager>();
	}

    private void Update()
    {
        healthBar.fillAmount = health/100;
    }
    public void TakeDamage(float amount) // this is how the damage needs to work for fillAmount.
    {
        health -= amount;
        soundManager.Oof();
       // healthBar.fillAmount = health;

    }
    public void HealHealth(float amount) // this is how the damage needs to work for fillAmount.
    {
        health += amount;

       // healthBar.fillAmount = health;

    }

    public void Hit()
    {
        soundManager.Oof();
    }

    //private void Update()
   // {
        /*if (health != 110)
        {
            HealHealth(1f);
        }  */    
   // }

}
