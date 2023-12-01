using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningText : BaseGameEvent
{

    [SerializeField] protected Color textColor;
    [SerializeField] protected int chkTime;
    [SerializeField] protected string text;

    public override void InitalizeEvent()
    {

        UIManager.instance.SetWarningText(text, textColor, chkTime);
    }
}
