using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControl : BaseGameEvent
{

    [SerializeField] private BaseObj[] targets;
    [SerializeField] private int targetLayer;

    public override void InitalizeEvent()
    {

        for (int i = 0; i < targets.Length; i++)
        {

            targets[i].ResetTeam();
            targets[i].gameObject.layer = targetLayer;
            targets[i].AfterSettingLayer();
            targets[i].ChkSupply(false);
        }
    }
}
