using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandma : MonoBehaviour
{
    public int cost = 15;
    public int amountHealthGiven = 100;
    public AudioSource audioSource;
    public AudioClip gma1;
    public AudioClip ducttape;
    Coroutine waittospeak = null;
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 10);
        for(int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].tag == "Player")
            {
                Debug.Log("player");
                GrandmaVoiceLine1();                
                i += hitColliders.Length;
            }
        }
    }

    void GrandmaVoiceLine1()
    {
        Debug.Log("audioSource: " + (!audioSource.isPlaying) + " | waitToSpeak: " + (waittospeak == null));
        if (!audioSource.isPlaying && waittospeak == null)
        {
            audioSource.clip = gma1;
            audioSource.Play();
            waittospeak = StartCoroutine(waitToSpeak());
        }
    }

    IEnumerator waitToSpeak()
    {
        //if (!audioSource.isPlaying)
        //{
            yield return new WaitForSeconds(25);
            waittospeak= null;
        //}
    }

    public virtual void RecieveInteract(Pawn source, Interactable myInteractable)
    {        
        if (source.myMask.hasMask)
        {
            audioSource.clip = ducttape;
            audioSource.Play();

            if (source.MyCandy.candy < cost)
            {
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
}
