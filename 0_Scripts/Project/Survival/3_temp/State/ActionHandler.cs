using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionHandler
{

    private IUnitState[] states;

    public ActionHandler(int MAX_STATESNUM)
    {


        states = new IUnitState[MAX_STATESNUM];
    }

    public void Action(int _num)
    {

        if (_num >= states.Length || _num < 0)
        {

            // �ε��� ���̸� ���� X
            // ���� ���̰ų� ����� ��� �ε��� ��
            return;
        }

        Execute(_num);
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
