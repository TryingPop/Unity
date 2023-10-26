using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ư ������!
/// </summary>
[CreateAssetMenu(fileName = "Button", menuName = "StateAction/Button")]
public class ButtonHandler : StateHandler<ButtonInfo>
{

    // sbyte������ ���� ���� Ǯ�ٺ��� �ٸ� �ڷ����� ���� ���ϸ� �ҷ��ͼ� 10 -> 40���� �Ǿ����� int�� ����
    protected int[] idxs = null;

    // ���� Ű�� �������ų� cmd�� �ൿ�� �����ϰ� ���� ���� ��ųʸ��� �ؾ��Ѵ�!
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

                // ���⼭ MAX_USE_BUTTONS�� ������ ���� �� �ִ� �ִ� ���¿� ���⿡ ��ư���� �״�� �д�
                // �ٸ� ��� override�ǰ� �ؾ��Ѵ�!
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
    /// ����� Ȯ��
    /// </summary>
    public void Changed(SelectManager _selectManager)
    {

        int idx = GetIdx(_selectManager.MyState);
        if (idx != -1) actions[idx].OnEnter(_selectManager);
    }

    /// <summary>
    /// ���� ����
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
