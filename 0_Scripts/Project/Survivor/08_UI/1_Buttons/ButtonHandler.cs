using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ư ������!
/// </summary>
[CreateAssetMenu(fileName = "Button", menuName = "StateAction/Button")]
public class ButtonHandler : StateHandler<ButtonInfo>
{

    protected Dictionary<MY_STATE.INPUT, int> myActionNum;

    public Dictionary<MY_STATE.INPUT, int> MyActionNum
    {

        get
        {

            if (myActionNum == null)
            {

                myActionNum = new Dictionary<MY_STATE.INPUT, int>(actions.Length);

                for (int i = 0; i < actions.Length; i++)
                {

                    myActionNum[actions[i].BtnKey] = i;
                }
            }

            return myActionNum;
        }
    }

    public void Action(PlayerManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (idx != -1) actions[idx].Action(_selectManager);
    }

    /// <summary>
    /// ����� Ȯ��
    /// </summary>
    public void Changed(PlayerManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (actions[idx].ActiveBtn && idx != -1) actions[idx].OnEnter(_selectManager);
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void ForcedQuit(PlayerManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (idx != -1) actions[idx].OnExit(_selectManager);
    }

    public int GetIdx(MY_STATE.INPUT _key)
    {

        return MyActionNum.ContainsKey(_key) ? myActionNum[_key] : -1;
    }
}
