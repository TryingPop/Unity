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
}
