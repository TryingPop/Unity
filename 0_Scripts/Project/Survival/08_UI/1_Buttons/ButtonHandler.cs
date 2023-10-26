using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 모음집!
/// </summary>
[CreateAssetMenu(fileName = "Button", menuName = "StateAction/Button")]
public class ButtonHandler : StateHandler<ButtonInfo>
{

    // sbyte였으나 백준 문제 풀다보니 다른 자료형이 성능 저하를 불러와서 10 -> 40으로 되었지만 int로 수정
    protected int[] idxs = null;

    // 쓰는 키가 많아지거나 cmd로 행동을 구분하고 싶을 때는 딕셔너리로 해야한다!
    public int[] Idxs
    {

        get
        {

            if (actions == null)
            {

                idxs = new int[1] { -1 };
                return idxs;
            }

            if (idxs == null
                || idxs.Length < actions.Length)
            {

                // 여기서 MAX_USE_BUTTONS과 유닛이 취할 수 있는 최대 상태와 같기에 버튼으로 그대로 둔다
                // 다른 경우 override되게 해야한다!
                if (actions.Length > VariableManager.MAX_USE_BUTTONS)
                {

                    Array.Resize(ref actions, VariableManager.MAX_USE_BUTTONS);
                }

                idxs = new int[VariableManager.MAX_USE_BUTTONS];
                for (int i = 0; i < idxs.Length; i++)
                {

                    idxs[i] = -1;
                }

                for (int i = 0; i < actions.Length; i++)
                {

                    int key = (int)actions[i].BtnKey;
                    if (key > idxs.Length
                        || key <= 0) continue;

                    idxs[key - 1] = (sbyte)i;
                }
            }

            return idxs;
        }
    }

    public void Action(SelectManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (idx != -1) actions[idx].Action(_selectManager);
    }

    /// <summary>
    /// 즉발형 확인
    /// </summary>
    public void Changed(SelectManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (idx != -1) actions[idx].OnEnter(_selectManager);
    }

    /// <summary>
    /// 강제 종료
    /// </summary>
    public void ForcedQuit(SelectManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (idx != -1) actions[idx].OnExit(_selectManager);
    }

    public override int GetIdx(int _idx)
    {

        if (Idxs.Length < _idx
            || _idx < 1) return -1;

        return Idxs[_idx - 1];
    }
}
