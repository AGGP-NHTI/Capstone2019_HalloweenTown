using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMask : BaseMask
{
    public override void Ult()
    {        
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            pawn.MyMoveScript.moveSpeed *= 2;
            ulttimerCoroutine = StartCoroutine("UltTimer");
        }
    }
    public override void UltFinished()
    {
        pawn.MyMoveScript.moveSpeed /= 2;
    }
}
