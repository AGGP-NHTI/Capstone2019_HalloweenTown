using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireMask : BaseMask {
    
    float healthSuck = 10f;
    
    public override void Ult()
    {
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
               Collider[] hitColliders = Physics.OverlapSphere(barrel.transform.position, 3.0f);
               for (int i = 0; i < hitColliders.Length; i++)
               {
                   if (hitColliders[i].tag == "Player")
                   {
                     Pawn hitPlayerPawn = hitColliders[i].GetComponent<Pawn>();
                        if(hitPlayerPawn.myMask.hasMask)
                        {
                           if(hitPlayerPawn.myHealth.health < healthSuck)
                           {
                               pawn.myHealth.health += hitPlayerPawn.myHealth.health;
                               hitPlayerPawn.myHealth.health = 0;
                          }
                          else
                          {
                              pawn.myHealth.health += healthSuck;
                              hitPlayerPawn.myHealth.health -=healthSuck;
                          }                    
                        }
                   }
               }
            ultTimeFloat = 0;
            waitforultCoroutine = StartCoroutine("WaitForUltTimer");
        }
    }
}
