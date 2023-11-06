using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEvent : GameEvent
{

    [SerializeField] protected TYPE_MANAGEMENT type;
    [SerializeField] protected int amount;
    [SerializeField] protected int layer;

    public override void StartEvent()
    {

        var teamInfo = TeamManager.instance.GetTeamInfo(layer);
        if (teamInfo != null)
        {

            teamInfo.Upgrade(type, amount);
        }
    }
}
