using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfMask : BaseMask {

    public override void Ult()
    {
        pawn.MyBoo.damage *= 2;//add all weapons
        if (wait == null)
        {
            wait = StartCoroutine("BeginGameCountDown");
        }
    }
    public override void UltFinished()
    {
        pawn.MyBoo.damage /= 2;
    }
}
