using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitAction", menuName = "StateAction/UnitAction")]
public class UnitStateAction : StateHandler<IUnitAction>
{

    protected Dictionary<MY_STATE.GAMEOBJECT, int> myActionNum;
    protected Dictionary<MY_STATE.GAMEOBJECT, int> MyActionNum
    {

        get
        {

            if (myActionNum == null)
            {

                myActionNum = new Dictionary<MY_STATE.GAMEOBJECT, int>(actions.Length);
                for (int i = 0; i < actions.Length; i++)
                {

                    myActionNum.Add((MY_STATE.GAMEOBJECT)i, i);
                }
            }

            return myActionNum;
        }
    }

    public int GetIdx(MY_STATE.GAMEOBJECT _state)
    {

        return MyActionNum.ContainsKey(_state) ? myActionNum[_state] : -1;
    }

    /// <summary>
    /// 행동 있으면 행동 실행
    /// </summary>
    /// <param name="_unit"></param>
    public void Action(Unit _unit)
    {

        int idx = GetIdx(_unit.MyState);

        if (idx != -1)
        {

            actions[idx].Action(_unit);
        }
        // else Debug.Log($"{gameObject.name}의 {(STATE_UNIT)_unit.MyState} 행동이 없습니다.");
    }

    public void Changed(Unit _unit)
    {

        int idx = GetIdx(_unit.MyState);
        if (idx != -1) actions[idx].OnEnter(_unit);
        // else Debug.Log($"{gameObject.name}의 {(STATE_UNIT)_unit.MyState} 행동이 없습니다.");
    }

    public string GetStateName(MY_STATE.GAMEOBJECT _state)
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