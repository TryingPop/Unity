using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjs : BaseGameEvent
{

    [SerializeField] private BaseObj[] targets;
    [SerializeField] private bool isActive;


    public override void InitalizeEvent()
    {

        if (isActive) Active();
        else InActive();
    }

    private void Active()
    {

        for (int i = 0; i < targets.Length; i++)
        {

            targets[i].gameObject.SetActive(true);
            targets[i].AfterSettingLayer();
            targets[i].ChkSupply(false);
        }
    }

    private void InActive()
    {

        for (int i = 0; i < targets.Length; i++)
        {

            targets[i].Dead(true);
        }
    }
}
