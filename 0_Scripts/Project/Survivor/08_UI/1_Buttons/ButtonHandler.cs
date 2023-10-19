using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 모음집!
/// </summary>
[CreateAssetMenu(fileName = "Button", menuName = "StateAction/Button")]
public class ButtonHandler : StateHandler<ButtonInfo>
{

    protected sbyte[] idxs = null;

    // 쓰는 키가 많아지거나 cmd로 행동을 구분하고 싶을 때는 딕셔너리로 해야한다!
    public sbyte[] Idxs
    {

        get
        {

            if (actions == null)
            {

                idxs = new sbyte[1] { -1 };
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

                idxs = new sbyte[VariableManager.MAX_USE_BUTTONS];
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

    public void Action(InputManager _inputManager)
    {

        int idx = GetIdx(_inputManager.MyState);
        if (idx != -1) actions[idx].Action(_inputManager);
    }

    /// <summary>
    /// 즉발형 확인
    /// </summary>
    public void Changed(InputManager _inputManager)
    {

        int idx = GetIdx(_inputManager.MyState);
        if (idx != -1) actions[idx].OnEnter(_inputManager);
    }

    /// <summary>
    /// 강제 종료
    /// </summary>
    public void ForcedQuit(InputManager _inputManager)
    {

        int idx = GetIdx(_inputManager.MyState);
        if (idx != -1) actions[idx].OnExit(_inputManager);
    }

    public override int GetIdx(int _idx)
    {

        if (Idxs.Length < _idx
            || _idx < 1) return -1;

        return Idxs[_idx - 1];
    }
}
