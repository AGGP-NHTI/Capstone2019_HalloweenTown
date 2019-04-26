using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressOnPlayerInput : MonoBehaviour
{
    [SerializeField]protected Button button;

    public virtual void ClickButton()
    {
        if (button)
        {
            button.onClick.Invoke();
        }
    }
}
