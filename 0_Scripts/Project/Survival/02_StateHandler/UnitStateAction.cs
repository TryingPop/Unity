using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAction", menuName = "StateAction/UnitAction")]
public class UnitStateAction : StateHandler<Unit, IUnitAction>
// MonoBehaviour
{

    /// <summary>
    /// �ൿ ������ �ൿ ����
    /// </summary>
    /// <param name="_unit"></param>
    public override void Action(Unit _unit)
    {

        int idx = _unit.MyState;

        if (ChkAction(idx))
        {

            actions[idx].Action(_unit);
        }
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }

    public void Changed(Unit _unit)
    {

        int idx = _unit.MyState;
        if (ChkAction(idx)) actions[idx].OnEnter(_unit);
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }
}