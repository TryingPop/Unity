using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �ൿ ����
/// </summary>
[CreateAssetMenu(fileName = "BuildingAction", menuName = "StateAction/BuildingAction")]
public class BuildingStateAction : StateHandler<BuildingAction>
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

                    myActionNum.Add((STATE_SELECTABLE)(i + 1), i);
                }
            }

            return myActionNum;
        }
    }

    public int GetIdx(STATE_SELECTABLE _state)
    {

        return MyActionNum.ContainsKey(_state) ? MyActionNum[_state] : -1;
    }

    public void Action(Building _building)
    {

        int idx = GetIdx(_building.MyState);
        if (idx != -1) actions[idx].Action(_building);
    }


    public void Changed(Building _building)
    {

        int idx = GetIdx(_building.MyState);
        if (idx != -1) actions[idx].OnEnter(_building);
        // else Debug.Log($"{gameObject.name}�� {(STATE_UNIT)_unit.MyState} �ൿ�� �����ϴ�.");
    }
    
    /// <summary>
    /// �ش� �ൿ ���� ����
    /// </summary>
    /// <param name="_building"></param>
    public void ForcedQuit(Building _building)
    {

        int idx = GetIdx(_building.MyState);
        if (idx != -1) actions[idx].ForcedQuit(_building);
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