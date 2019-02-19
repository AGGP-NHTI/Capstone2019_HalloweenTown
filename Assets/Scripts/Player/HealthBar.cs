using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthBar;
    public float health;
    public bool ghostUlt = false;

    SoundManager soundManager;

    //public GameObject dropCandy;//The candy
    //public Transform player;//get player transform
    //public ParticleSystem system;//particles

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
