using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfMask : BaseMask {
    bool ulted = false;
    public override void Ult()
    {
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            ulted = true;
            pawn.MyBoo.damage *= 2;
            pawn.myProjectileManager.werewolfUlt = true;
            ulttimerCoroutine = StartCoroutine("UltTimer");
        }
    }
    public override void UltFinished()
    {
        if (ulted)
        {
            pawn.MyBoo.damage /= 2;
            pawn.myProjectileManager.werewolfUlt = false;
            ulted = false;
        }
    }
}
