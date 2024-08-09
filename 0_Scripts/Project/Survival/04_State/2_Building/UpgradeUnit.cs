using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대장간용도
/// </summary>
[CreateAssetMenu(fileName = "UpgradeUnit", menuName = "Action/Building/UpgradeUnit")]
public class UpgradeUnit : BuildingAction
{

    [SerializeField] protected TYPE_MANAGEMENT upgradeType;
    [SerializeField] protected int cost;
    [SerializeField] protected int grade;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.MyTeam.UpgradeUnit(upgradeType, grade);
            _building.MyTurn = 0;
            OnExit(_building);
        }
    }

    public override void ForcedQuit(Building _building)
    {

        // 환불
        int refundCost = Mathf.FloorToInt(VarianceManager.REFUND_RATE * cost);
        _building.MyTeam.AddGold(refundCost);
        OnExit(_building);
    }

    public override void OnEnter(Building _building)
    {

        // 골드 확인
        if (!_building.MyTeam.ChkGold(cost))
        {

            OnExit(_building);
            return;
        }

        _building.MyTeam.AddGold(-cost);
        base.OnEnter(_building);
    }
}
