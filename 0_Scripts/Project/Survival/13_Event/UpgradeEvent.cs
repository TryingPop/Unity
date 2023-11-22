using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEvent : BaseGameEvent
{

    [SerializeField] protected TYPE_MANAGEMENT type;
    [SerializeField] protected int amount;
    [SerializeField] protected int layer;

    public override void InitalizeEvent()
    {

        var teamInfo = TeamManager.instance.GetTeamInfo(layer);
        if (teamInfo != null)
        {

            // ��常 ���� �����ؼ� �ö󰡰� �Ѵ�
            // �α��� ���׷��̵� ���� ���� �ȵǰ� �ߴ�
            if (type == TYPE_MANAGEMENT.GOLD) teamInfo.AddGold(amount);
            else if (type == TYPE_MANAGEMENT.MAX_SUPPLY) teamInfo.AddMaxSupply(amount);
            else teamInfo.Upgrade(type, amount);
        }
    }
}