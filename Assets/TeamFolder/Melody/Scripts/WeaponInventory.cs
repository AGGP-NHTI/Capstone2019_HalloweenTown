using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    Text txtNumberEggs;
    Text txtNumberToiletPaper;
    public int numberEggs;
    public int numberToiletPaper;
    // Use this for initialization
    void Start()
    {
        //  txtNumberEggs = transform.Find("EggText").gameObject.Te
        // txtNumberEggs= GameObject.Find("EggText").GetComponent<Text>();
        //txtNumberToiletPaper = GameObject.Find("TPText").GetComponent<Text>();
        //txtNumberToiletPaper.text = string.Format("Toilet Paper: {0}", numberToiletPaper);
        // txtNumberEggs.text = "lol";
        //txtNumberToiletPaper.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateDisplay()
    {
        //txtNumberEggs.text = string.Format("Eggs: {0}", numberEggs);
       // txtNumberToiletPaper.text = string.Format("Toilet Paper: {0}", numberToiletPaper);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("other name: {0}", other.gameObject.name));
        if (other.gameObject.name.Contains("EggCarton"))
        {
            numberEggs += 12;
            UpdateDisplay();
            Destroy(other.gameObject);
        }
    }
    public void subtractFromInventory(GameObject thrown)
    {
        if (thrown.GetComponent<Egg>()) numberEggs--;
        else if (thrown.GetComponent<ToiletPaper>()) numberToiletPaper--;
        UpdateDisplay();
    }
    public bool hasProjectile(GameObject go)
    {
        if (go.GetComponent<Egg>() && numberEggs > 0) return true;
        else if (go.GetComponent<ToiletPaper>() && numberToiletPaper > 0) return true;
        return false;
    }

}
