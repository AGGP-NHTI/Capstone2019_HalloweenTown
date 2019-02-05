using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMask : MonoBehaviour
{
    public float duration = 5f;
    protected Coroutine wait;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Ult()
    {

    }

    public virtual void UltFinished()
    {

    }

    public IEnumerator BeginGameCountDown()
    {
        float temp = duration;
        while (temp >= 0)
        {
            temp -= Time.deltaTime;
            Debug.Log("Waiting for ult again " + Mathf.Round(temp).ToString());
            yield return null;
        }
        UltFinished();
        wait = null;
    }
}
