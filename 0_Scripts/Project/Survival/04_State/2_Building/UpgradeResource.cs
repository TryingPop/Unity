using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeResource", menuName = "Action/Building/UpgradeResource")]
public class UpgradeResource : UpgradeUnit
{

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.MyTeam.UpgradeResource(upgradeType);
            _building.MyTurn = 0;
            OnExit(_building);
        }
    }

    public override void ForcedQuit(Building _building)
    {

        // 환불
        int cost = _building.MyTeam.GetUpgradeResourceCost(upgradeType);
        int refundCost = Mathf.FloorToInt(VarianceManager.REFUND_RATE * cost);
        _building.MyTeam.AddGold(refundCost);
        OnExit(_building);
    }

    public override void OnEnter(Building _building)
    {

        // 골드 확인
        int cost = _building.MyTeam.GetUpgradeResourceCost(upgradeType);

        if (cost == -1 || !_building.MyTeam.ChkGold(cost))
        {

            OnExit(_building);
            return;
        }

        _building.MyTeam.AddGold(-cost);
        _building.MaxTurn = turn;
        _building.MyTurn = 0;
    }
}
