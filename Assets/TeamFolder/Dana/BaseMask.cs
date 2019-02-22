using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMask : MonoBehaviour
{
    public float waitingDuration = 100f;
    public float ultingDuration = 100f;
    protected Coroutine ulttimerCoroutine;
    protected Coroutine waitforultCoroutine;
    public Pawn pawn;
    public GameObject barrel;

    public float ultTimeFloat = 0;
    public bool isUlting = false;

    protected void Start()
    {
        pawn = GetComponent<Pawn>();
        barrel = pawn.myMask.currentModel.GetComponent<GetBarrel>().barrel;

        waitforultCoroutine = StartCoroutine("WaitForUltTimer");
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

    public IEnumerator UltTimer()
    {
        isUlting = true;
        ultTimeFloat = ultingDuration;
        while (ultTimeFloat >= 0)
        {
            ultTimeFloat -= Time.deltaTime *5;
            //Debug.Log("Ulting Time Left: " + Mathf.Round(ultTimeFloat).ToString());
            yield return null;
        }
        UltFinished();
        ulttimerCoroutine = null;
        waitforultCoroutine = StartCoroutine("WaitForUltTimer");
    }

    public IEnumerator WaitForUltTimer()
    {
        isUlting = false;
        while (ultTimeFloat <= waitingDuration)
        {
            ultTimeFloat += Time.deltaTime *5;
            //Debug.Log("Waiting For Ult: " + Mathf.Round(ultTimeFloat).ToString());
            yield return null;
        }
        UltFinished();
        waitforultCoroutine = null;
    }
}
