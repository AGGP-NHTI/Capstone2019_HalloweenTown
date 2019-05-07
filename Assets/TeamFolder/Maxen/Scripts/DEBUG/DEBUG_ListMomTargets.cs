using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUG_ListMomTargets : MonoBehaviour
{
    public MomPawn mom;
    public Text output;
	
	void Update ()
    {
		if(mom)
        {
            string msg = "";
            for(int t = 0; t < 4; t++)
            {
                string targetName = "NULL";
                if(t < mom.Targets.Count)
                {
                    if(mom.Targets[t])
                    {
                        targetName = "Player " + mom.Targets[t].name.ToString();
                    }
                }

                msg += targetName + "\n";
            }

            output.text = msg;
        }
	}
}
