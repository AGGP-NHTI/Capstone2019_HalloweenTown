using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMask : BaseMask
{
    public override void Ult()
    {        
        if (wait == null)
        {
            pawn.MyMoveScript.moveSpeed *= 2;
            wait = StartCoroutine("BeginGameCountDown");
        }
    }
    public override void UltFinished()
    {
        pawn.MyMoveScript.moveSpeed /= 2;
    }
}
