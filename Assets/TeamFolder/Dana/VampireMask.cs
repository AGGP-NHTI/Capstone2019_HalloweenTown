using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireMask : BaseMask
{
    bool ulted = false;

    void Start()
    {
        base.Start();
        ultMultiplier = 20f;
    }

    public override void Ult()
    {
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            pawn.soundMan.VampireUltScream();
            pawn.MyBoo.VampireUlt();

            ulted = true;
            // ultTimeFloat = 0;
            ulttimerCoroutine = StartCoroutine("UltTimer");
        }
    }

    public override void UltFinished()
    {
        if (ulted)
        {
            pawn.MyBoo.VampireUltDone();
            ulted = false;
        }
    }
}
