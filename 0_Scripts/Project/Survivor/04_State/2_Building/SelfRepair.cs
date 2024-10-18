using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfRepair", menuName = "Action/Building/SelfRepair")]
public class SelfRepair : BuildingAction
{

    [SerializeField] protected int amount;

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {

            _building.Heal(amount);
            _building.MyTurn = 0;
            // OnExit(_building);
        }
    }
}
