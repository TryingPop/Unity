using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAction", menuName = "StateAction/UnitAction")]
public class UnitStateAction : StateHandler<IUnitAction>
{

    protected Dictionary<STATE_SELECTABLE, string> stateNames;
    public Dictionary<STATE_SELECTABLE, string> StateNames
    {

        get
        {

            if (stateNames == null)
            {

                stateNames = new Dictionary<STATE_SELECTABLE, string>();
                for (int i = 0; i < actions.Length; i++)
                {

                    stateNames.Add((STATE_SELECTABLE)i, actions[i].StateName);
                }
            }

            return stateNames;
        }
    }

    public override int GetIdx(int _idx)
    {

        if (actions.Length <= _idx
            || _idx < 0) return -1;

        return _idx;
    }

    /// <summary>
    /// �ൿ ������ �ൿ ����
    /// </summary>
    /// <param name="_unit"></param>
    public void Action(Unit _unit)
    {

        int idx = GetIdx(_unit.MyState);

        if (idx != -1)
        {

            actions[idx].Action(_unit);
        }
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }

    public void Changed(Unit _unit)
    {

        int idx = GetIdx(_unit.MyState);
        if (idx != -1) actions[idx].OnEnter(_unit);
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }
}