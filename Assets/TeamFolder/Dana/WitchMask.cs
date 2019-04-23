using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchMask : BaseMask
{
    bool ulted = false;
    public override void Ult()
    {        
        if (ulttimerCoroutine == null && waitforultCoroutine == null)
        {
            pawn._anim.SetBool("witchUltBool", true);
            pawn.myMask.currentModel.GetComponent<GetBarrel>().witchBroom.SetActive(true);
            pawn.soundMan.WitchUltScream();
            ulted = true;
            pawn.MyMoveScript.moveSpeed *= 2;
            ulttimerCoroutine = StartCoroutine("UltTimer");
        }
    }
    public override void UltFinished()
    {
        if(ulted)
        {
            pawn.myMask.currentModel.GetComponent<GetBarrel>().witchBroom.SetActive(false);

            pawn._anim.SetBool("witchUltBool", false);
            pawn.MyMoveScript.moveSpeed /= 2;
        }
        ulted = false;
    }
}
