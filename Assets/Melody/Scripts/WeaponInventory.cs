using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour {
    public Text txtNumberEggs;
	public int numberEggs;
    public int numberToiletPaper;
    // Use this for initialization
	void Start () {
        txtNumberEggs.text = "Eggs:  10";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateEggCountDisplay()
    {
        txtNumberEggs.text = string.Format("Eggs: {0}", numberEggs);
    }

}
