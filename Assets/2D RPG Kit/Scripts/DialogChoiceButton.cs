using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogChoiceButton : MonoBehaviour
{
    public void Press(int buttonValue)
    {
        DialogManager.instance.SelectDialogChoice(buttonValue);
    }
}
