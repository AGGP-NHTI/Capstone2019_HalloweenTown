using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    public float health;
    public bool ghostUlt = false;

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
        if (health >=0 ||!ghostUlt)//if ghost is not ulting
        {
            health -= amount;
            soundManager.Oof();
        }
    }
    public void HealHealth(float amount) // this is how the damage needs to work for fillAmount.
    {
        health += amount;
    }

    public void Hit()
    {
        if (!ghostUlt)
        {
            soundManager.Oof();
        }
    }

    //private void Update()
   // {
        /*if (health != 110)
        {
            HealHealth(1f);
        }  */    
   // }

}
