using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningText : BaseGameEvent
{

    [SerializeField] protected Color textColor;
    [SerializeField] protected int chkTime;

    [SerializeField] protected bool quitWarningText = false;

    [SerializeField] protected string text;

    public override void InitalizeEvent()
    {

        if (quitWarningText) Quit();
        else UIManager.instance.SetWarningText(text, textColor, chkTime);
    }

    protected void Quit()
    {

        UIManager.instance.QuitWarningText();
        text = string.Empty;
    }
}
