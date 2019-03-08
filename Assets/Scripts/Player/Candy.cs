using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candy : MonoBehaviour
{
    public static Dictionary<PlayerController, int> Scoreboard;

    public int candy = 0;
    public Text candyText;
    float radius = 3.0f;
    public bool waitforcandy = false;
    public bool actionButton;
    public bool showXForCandy; 
    public GameObject candyPrefab;

    void Start()
    {
        if (candyText)
        {
            candyText.text = " " + candy.ToString();
        }
        actionButton = false;
        showXForCandy = false;
    }


    void Update()
    {
        if(candyText)
        {
            candyText.text = " " + candy.ToString();
        }
    }

    public void DropCandy()
    {
        int numCandy = 0;
        if (candy > 20)
        {
            numCandy = Random.Range(5, 20);
        }
        else if(candy > 0)
        {
            numCandy = Random.Range(1, candy);
        }

        if (candy > 0)
        {
            for (int i = 0; i < numCandy; i++)
            {
                Vector3 vel = Random.onUnitSphere;
                vel.y = Mathf.Abs(vel.y);
                Vector3 pos = new Vector3(0f, 1f, 0f) + vel;
                vel *= 5;

                GameObject candy;

                candy = Instantiate(candyPrefab, transform.position + pos, transform.rotation);
                candy.GetComponent<Rigidbody>().velocity = vel;
            }
        }

        candy -= numCandy;
    }

}