using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ButtonInfo
{

    public TYPE_BUTTON_OPTION buttonOpt;
    public TYPE_KEY buttonKey;

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

    public ButtonInfo(TYPE_BUTTON_OPTION _buttonOpt = TYPE_BUTTON_OPTION.NULL, 
        TYPE_KEY _buttonKey = TYPE_KEY.NONE)
    {

        buttonOpt = _buttonOpt;
        buttonKey = _buttonKey;
    }
}
