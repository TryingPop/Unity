using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 빌딩 행동 모음
/// </summary>
[CreateAssetMenu(fileName = "BuildingAction", menuName = "StateAction/BuildingAction")]
public class BuildingStateAction : StateHandler<BuildingAction>
{
    
    public void Action(Building _building)
    {

        int idx = GetIdx(_building.MyState);
        if (idx != -1) actions[idx].Action(_building);
    }

    public override int GetIdx(int _idx)
    {

        if (actions.Length < _idx
            || _idx < 1) return -1;

        return _idx - 1;
    }

    public void Changed(Building _building)
    {

        int idx = GetIdx(_building.MyState);
        if (idx != -1) actions[idx].OnEnter(_building);
        // else Debug.Log($"{gameObject.name}의 {(STATE_UNIT)_unit.MyState} 행동이 없습니다.");
    }
    
    /// <summary>
    /// 해당 행동 강제 종료
    /// </summary>
    /// <param name="_building"></param>
    public void ForcedQuit(Building _building)
    {

        int idx = GetIdx(_building.MyState);
        if (idx != -1) actions[idx].ForcedQuit(_building);
    }
}