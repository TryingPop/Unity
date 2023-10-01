using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingAction", menuName = "StateAction/BuildingAction")]
public class BuildingStateAction : StateHandler<Building, BuildingAction>
{

    public override void Action(Building _building)
    {

        int idx = _building.MyState;
        if (ChkAction(idx)) actions[idx].Action(_building);
    }
}