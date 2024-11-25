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

            targets[i].ResetTeamStat();
            targets[i].gameObject.layer = targetLayer;
            targets[i].ApplyTeamStat();
        }
    }
}
