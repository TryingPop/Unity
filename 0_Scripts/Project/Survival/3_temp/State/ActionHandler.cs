using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionHandler
{

    private IUnitState[] states;
    protected bool isDone;
    public bool IsDone
    {

        get
        {

            bool result = isDone;
            if (result) isDone = false;
            return result;
        }
    }

    public ActionHandler(int MAX_STATESNUM)
    {


        states = new IUnitState[MAX_STATESNUM];
    }

    public void Action(int _num)
    {

        if (_num >= states.Length || _num < 0)
        {

            // 인덱스 밖이면 실행 X
            // 공격 중이거나 사망인 경우 인덱스 밖
            isDone = false;
            return;
        }

        Execute(_num);

        if (states[_num].IsDone) isDone = true;
    }


    protected void Execute(int _num) 
    {

        states[_num].Execute();
    }

    public void AddState(int _idx, IUnitState _addState)
    {

        if (_idx >= states.Length) return;

        states[_idx] = _addState;
    }
}
