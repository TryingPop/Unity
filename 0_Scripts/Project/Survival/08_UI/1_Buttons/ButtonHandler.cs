using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 모음집!
/// </summary>
[CreateAssetMenu(fileName = "Button", menuName = "StateAction/Button")]
public class ButtonHandler : StateHandler<ButtonInfo>
{

    protected Dictionary<TYPE_INPUT, int> myActionNum;

    public Dictionary<TYPE_INPUT, int> MyActionNum
    {

        get
        {

            if (myActionNum == null)
            {

                myActionNum = new Dictionary<TYPE_INPUT, int>(actions.Length);

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
    /// 즉발형 확인
    /// </summary>
    public void Changed(PlayerManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (actions[idx].ActiveBtn && idx != -1) actions[idx].OnEnter(_selectManager);
    }

    /// <summary>
    /// 강제 종료
    /// </summary>
    public void ForcedQuit(PlayerManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (idx != -1) actions[idx].OnExit(_selectManager);
    }

    public int GetIdx(TYPE_INPUT _key)
    {

        return MyActionNum.ContainsKey(_key) ? myActionNum[_key] : -1;
    }
}
