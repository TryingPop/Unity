using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Action/Building/Upgrade")]
public class UpgradeStat : BuildingAction
{

    [SerializeField] private TYPE_UPGRADE upgradeType;

    [SerializeField] private short add;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.MyUpgrades.UpgradeStat(upgradeType, add);
            _building.MyTurn = 0;
        }

        if (_building.MyTurn == 0)
        {

            OnExit(_building);
        }
            
    }
}
