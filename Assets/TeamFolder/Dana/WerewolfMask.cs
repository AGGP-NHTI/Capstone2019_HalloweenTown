using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfMask : BaseMask {
    public override void Ult()
    {
        Debug.Log("werewolf ulting");
        pawn.MyBoo.damage *= 2;//add all weapons
        
        pawn.myProjectileManager.werewolfUlt = true;
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            ulttimerCoroutine = StartCoroutine("UltTimer");
        }
    }
    public override void UltFinished()
    {
        Debug.Log("werewolf done");
        pawn.MyBoo.damage /= 2;
        pawn.myProjectileManager.werewolfUlt = false;
    }
}
