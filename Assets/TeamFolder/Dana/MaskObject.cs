using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour {

    public virtual void RecieveInteract(Pawn source, Interactable myInteractable)
    {
        
        source.myMask.interactButton = true;
        source.myMask.scriptName = gameObject.tag;
        Destroy(gameObject);
    }
}
