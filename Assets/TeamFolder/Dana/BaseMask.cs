using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMask : MonoBehaviour
{
    public float duration = 20f;
    protected Coroutine wait;
    public Pawn pawn;
    public GameObject barrel;

    // Use this for initialization
    protected void Start()
    {
        pawn = GetComponent<Pawn>();
        barrel = pawn.myMask.currentModel.GetComponent<GetBarrel>().barrel;
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
