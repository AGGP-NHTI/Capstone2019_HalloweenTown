using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    public Text txtNumberEggs;
    public Text txtNumberToiletPaper;
    public int numberEggs = 0;
    public int numberToiletPaper = 0;
    public int toiletPaperMax = 3;
    public int eggMax = 12;
    Pawn pawn;
    int selectedWeapon;
    List<GameObject> weaponList;
    // Use this for initialization
    void Start()
    {
        pawn = GetComponent<Pawn>();
        weaponList = pawn.myProjectileManager.weaponList;

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

    public void UpdateDisplay()
    {
        selectedWeapon = pawn.myProjectileManager.selectedWeaponIndex;

        if (weaponList[selectedWeapon].CompareTag("Egg") /* selectedWeapon == 0*/)
        {
            if(txtNumberToiletPaper) txtNumberToiletPaper.enabled = false;
            if(txtNumberEggs) txtNumberEggs.enabled = true;
        }
        if (/*weaponList[selectedWeapon].CompareTag("Toilet Paper")*/ selectedWeapon == 1)
        {
            if (txtNumberToiletPaper) txtNumberToiletPaper.enabled = true;
             if (txtNumberEggs) txtNumberEggs.enabled = false;
        }


        if (txtNumberEggs) txtNumberEggs.text = string.Format("{0}", numberEggs);
        if (txtNumberToiletPaper) txtNumberToiletPaper.text = string.Format("{0}", numberToiletPaper);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(string.Format("other name: {0}", other.gameObject.name));
        if (other.gameObject.name.Contains("EggCarton"))
        {
            if (numberEggs < eggMax)
            {
                if(numberEggs + 6 > eggMax)
                {
                    numberEggs = eggMax;
                }
                else
                {
                    numberEggs += 6;                    
                }
                UpdateDisplay();
                DestroyerOfObjects.DestroyObject(other.gameObject);
            }
        }
        else if (other.gameObject.tag == "TPPickUp")
        {
            //Debug.Log(string.Format("numberToiletPaper: {0}; toiletPaperMax:  {1}", numberToiletPaper, toiletPaperMax));
            if (numberToiletPaper < toiletPaperMax)
            {
                numberToiletPaper++;
                UpdateDisplay();
                DestroyerOfObjects.DestroyObject(other.gameObject);
            }

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
        if (go.GetComponent<Egg>() && numberEggs > 0)
        {
            return true;
        }
        else if (go.GetComponent<ToiletPaper>() && numberToiletPaper > 0) return true;
        return false;
    }

}
