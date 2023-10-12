using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAction", menuName = "StateAction/UnitAction")]
public class UnitStateAction : StateHandler<IUnitAction>
{

    /// <summary>
    /// 행동 있으면 행동 실행
    /// </summary>
    /// <param name="_unit"></param>
    public void Action(Unit _unit)
    {

        int idx = _unit.MyState;

        if (ChkIdx(idx))
        {

            actions[idx].Action(_unit);
        }
        // else Debug.Log($"{gameObject.name}의 {(STATE_UNIT)_unit.MyState} 행동이 없습니다.");
    }

    public void Changed(Unit _unit)
    {

        int idx = _unit.MyState;
        if (ChkIdx(idx)) actions[idx].OnEnter(_unit);
        // else Debug.Log($"{gameObject.name}의 {(STATE_UNIT)_unit.MyState} 행동이 없습니다.");
    }
}