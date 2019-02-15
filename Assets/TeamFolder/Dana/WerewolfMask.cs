using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfMask : BaseMask {

    public override void Ult()
    {
        pawn.MyBoo.damage *= 2;//add all weapons
        pawn.myProjectileManager.werewolfUlt = true;
        if (wait == null)
        {
            wait = StartCoroutine("BeginGameCountDown");
        }
    }
    public override void UltFinished()
    {
        pawn.MyBoo.damage /= 2;
        pawn.myProjectileManager.werewolfUlt = false;
    }
}
