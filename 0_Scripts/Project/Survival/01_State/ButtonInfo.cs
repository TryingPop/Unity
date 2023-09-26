using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonInfo
{

    public VariableManager.STATE_BUTTON_OPTION buttonOpt;
    public InputManager.STATE_KEY buttonKey;

    private static ButtonInfo empty;

    public static ButtonInfo Empty
    {

        get
        {
            if (empty == null)
            {

                empty = new ButtonInfo();
            }
            return empty;
        }
    }

    public ButtonInfo(VariableManager.STATE_BUTTON_OPTION _buttonOpt = VariableManager.STATE_BUTTON_OPTION.NULL, 
        InputManager.STATE_KEY _buttonKey = InputManager.STATE_KEY.NONE)
    {

        buttonOpt = _buttonOpt;
        buttonKey = _buttonKey;
    }
}
