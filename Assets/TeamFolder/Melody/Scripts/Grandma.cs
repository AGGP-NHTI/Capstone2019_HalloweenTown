using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandma : MonoBehaviour
{
    public int cost = 5;
    public int amountHealthGiven = 50;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void RecieveInteract(Pawn source, Interactable myInteractable)
    {
        if(source.MyCandy.candy < cost)
        {
            Debug.Log("Not enough scarabs");
            return;
        }
        
        float maxHealth = 100.0f;
        float health = source.myHealth.health;
        if (source.myHealth.health != 0 && health < maxHealth)
        {
            if (health > maxHealth - amountHealthGiven)
            {
                source.myHealth.HealHealth(maxHealth - health);
            }
            else
            {
                source.myHealth.HealHealth(amountHealthGiven);
            }
            source.MyCandy.candy -= cost;
           
        }
    }
}
