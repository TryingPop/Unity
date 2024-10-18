using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAction", menuName = "StateAction/UnitAction")]
public class UnitStateAction : StateHandler<IUnitAction>
{

    protected Dictionary<STATE_SELECTABLE, int> myActionNum;
    protected Dictionary<STATE_SELECTABLE, int> MyActionNum
    {

        get
        {

            if (myActionNum == null)
            {

                myActionNum = new Dictionary<STATE_SELECTABLE, int>(actions.Length);
                for (int i = 0; i < actions.Length; i++)
                {

                    myActionNum.Add((STATE_SELECTABLE)i, i);
                }
            }

            return myActionNum;
        }
    }

    public int GetIdx(STATE_SELECTABLE _state)
    {

        return MyActionNum.ContainsKey(_state) ? myActionNum[_state] : -1;
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

    public string GetStateName(STATE_SELECTABLE _state)
    {

        if (MyActionNum.ContainsKey(_state))
        {

            return actions[myActionNum[_state]].StateName;
        }
        else
        {

            return "";
        }
    }
}