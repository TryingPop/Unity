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

    public void Action(Unit _unit)
    {

        int idx = _unit.MyState;
        if (ChkActions(idx)) actions[idx].Action(_unit);
    }

    /// <summary>
    /// 해당 행동이 있는지 확인
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
    }

    public void AddActions(int _idx, IUnitAction _action)
    {

        if (ChkActions(_idx)) actions[_idx] = _action;
    }
}
