using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAction", menuName = "StateAction/UnitAction")]
public class UnitStateAction : StateHandler<IUnitAction>
{

    /// <summary>
    /// �ൿ ������ �ൿ ����
    /// </summary>
    /// <param name="_unit"></param>
    public void Action(Unit _unit)
    {

        int idx = _unit.MyState;

        if (ChkIdx(idx))
        {

            actions[idx].Action(_unit);
        }
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }

    public void Changed(Unit _unit)
    {

        int idx = _unit.MyState;
        if (ChkIdx(idx)) actions[idx].OnEnter(_unit);
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }
}