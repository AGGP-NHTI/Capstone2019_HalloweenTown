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
            pawn.MyCamera.cullingMask |= 1 << LayerMask.NameToLayer("GlowyBoy");
            pawn.MyCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("RegularBoy"));

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
            pawn.MyCamera.cullingMask |= 1 << LayerMask.NameToLayer("RegularBoy");
            pawn.MyCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("GlowyBoy"));
            pawn.MyBoo.VampireUltDone();
            ulted = false;
        }
    }
}
