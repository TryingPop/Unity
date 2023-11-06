using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControl : GameEvent
{

    [SerializeField] private Selectable[] targets;
    [SerializeField] private int targetLayer;

    public override void StartEvent()
    {

        for (int i = 0; i < targets.Length; i++)
        {

            targets[i].gameObject.layer = targetLayer;
            targets[i].ResetTeam();
            targets[i].AfterSettingLayer();
            targets[i].ChkSupply(false);
        }
    }
}
