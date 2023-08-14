using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAction : MonoBehaviour
{

    public IUnitAction[] actions;

    public StateAction(int _actions)
    {

        actions = new IUnitAction[_actions];
    }

    /// <summary>
    /// �ൿ ������ �ൿ ����
    /// </summary>
    /// <param name="_unit"></param>
    public void Action(Unit _unit)
    {

        int idx = _unit.MyState;
        if (ChkActions(idx)) actions[idx].Action(_unit);
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }

    /// <summary>
    /// �ش� �ൿ�� �ִ��� Ȯ��
    /// </summary>
    /// <param name="_idx"></param>
    /// <returns></returns>
    public bool ChkActions(int _idx)
    {

        if (_idx >= actions.Length || _idx < 0) return false;
        if (actions[_idx] == null) return false;

        return true;
    }

    public void Changed(Unit _unit)
    {

        int idx = _unit.MyState;
        if (ChkActions(idx)) actions[idx].Changed(_unit);
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }

    public void AddActions(int _idx, IUnitAction _action)
    {

        if (ChkActions(_idx)) actions[_idx] = _action;
    }
}