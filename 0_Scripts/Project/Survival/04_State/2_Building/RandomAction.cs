using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandAction", menuName = "Action/Building/RandAction")]
public class RandomAction : BuildingAction
{

    public override void Action(Building _building)
    {

        _building.MyTurn++;

        if (_building.MyTurn >= turn)
        {


            int next = Random.Range(1, _building.MyStateAction.GetSize());

            OnExit(_building, (STATE_BUILDING)next);
        }
    }
}
