using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WerewolfMask : BaseMask {
    bool ulted = false;

    void Start()
    {
        base.Start();
        ultMultiplier = 30f;
    }
    public override void Ult()
    {
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            pawn._anim.SetBool("wolfUltBool", true);

            pawn.soundMan.WerewolfUltScream();
            ulted = true;
            pawn.myProjectileManager.eggDamage *= 2;
            //pawn.MyBoo.damage *= 2;
            pawn.myProjectileManager.werewolfUlt = true;
            pawn.MyBoo.WerewolfUlt();

            ulttimerCoroutine = StartCoroutine("UltTimer");
        }
    }
    public override void UltFinished()
    {
        if (ulted)
        {
            pawn.MyBoo.WerewolfUltDone();
            pawn._anim.SetBool("wolfUltBool", false);

            pawn.myProjectileManager.eggDamage /= 2;
            // pawn.MyBoo.damage /= 2;
            pawn.myProjectileManager.werewolfUlt = false;
            ulted = false;
        }
    }
}
