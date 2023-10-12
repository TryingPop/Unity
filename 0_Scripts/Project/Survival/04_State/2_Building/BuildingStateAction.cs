using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingAction", menuName = "StateAction/BuildingAction")]
public class BuildingStateAction : StateHandler<BuildingAction>
{

    public void Action(Building _building)
    {

        int idx = _building.MyState;
        if (ChkIdx(idx)) actions[idx].Action(_building);
    }
}