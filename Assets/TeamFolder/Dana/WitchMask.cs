using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMask : BaseMask
{
    public override void Ult()
    {
        pawn.MyMoveScript.moveSpeed *=2;
        if (wait == null)
        {
            wait = StartCoroutine("BeginGameCountDown");
        }
    }
    public override void UltFinished()
    {
        pawn.MyMoveScript.moveSpeed /= 2;
    }
}
