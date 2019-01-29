using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour {
    public Text txtNumberEggs;
    public Text txtNumberToiletPaper;
	public int numberEggs;
    public int numberToiletPaper;
    // Use this for initialization
	void Start () {
        txtNumberEggs.text = string.Format("Eggs: {0}", numberEggs);
        txtNumberToiletPaper.text = string.Format("Toilet Paper: {0}", numberToiletPaper);
        txtNumberToiletPaper.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateEggCountDisplay()
    {
        txtNumberEggs.text = string.Format("Eggs: {0}", numberEggs);
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(string.Format("other name: {0}", other.gameObject.name));
        if (other.gameObject.name.Contains("EggCarton"))
        {
            numberEggs += 12;
            UpdateEggCountDisplay();
            Destroy(other.gameObject);
        }
    }

}
