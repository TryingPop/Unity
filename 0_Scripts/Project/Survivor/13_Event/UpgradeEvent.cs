using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEvent : BaseGameEvent
{

    [SerializeField] protected MY_TYPE.RESOURCE type;
    [SerializeField] protected int amount;
    [SerializeField] protected int layer;

    public override void InitalizeEvent()
    {

        var teamInfo = TeamManager.instance.GetTeamInfo(layer);
        if (teamInfo != null)
        {

            // 골드만 따로 구분해서 올라가게 한다
            // 인구는 업그레이드 말고 증가 안되게 했다
            if (type == MY_TYPE.RESOURCE.TURN_GOLD) teamInfo.AddGold(amount);
            else if (type == MY_TYPE.RESOURCE.MAX_SUPPLY) teamInfo.AddMaxSupply(amount);
            // else teamInfo.Upgrade(type, amount);
        }
    }
}