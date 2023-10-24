using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 대장간용도
/// </summary>
[CreateAssetMenu(fileName = "Upgrade", menuName = "Action/Building/Upgrade")]
public class UpgradeStat : BuildingAction
{

    [SerializeField] private TYPE_MANAGEMENT upgradeType;

    [SerializeField] private short add;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.MyTeam.Upgrade(upgradeType, add);
            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
            
    }
}
