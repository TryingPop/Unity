using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionHandler<T> where T : BaseUnit
{

    private IUnitState<T>[] states;

    public ActionHandler (int _MAX_STATES)
    {

        states = new IUnitState<T>[_MAX_STATES];
    }

    public void AddState(int _idx, IUnitState<T> _state)
    {

        if (_idx < 0 || _idx >= states.Length) return;
        states[_idx] = _state;
    }

    public void Action(T _unit)
    {

        int idx = _unit.MyState;
        if (idx < 0 || idx >= states.Length) return;
        if (states[idx] == null) return;

        states[idx].Execute(_unit);
    }

    public void Reset(T _unit)
    {

        int idx = _unit.MyState;
        if (idx < 0 || idx >= states.Length) return;
        if (states[idx] == null) return;

        states[idx].Reset(_unit);
    }
}
