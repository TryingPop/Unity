using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateHandler<T, V> : ScriptableObject
                                    where T : Selectable
                                    where V : IAction<T>
{

    public V[] actions;

    public abstract void Action(T _param);

    /// <summary>
    /// �ܺο��� �ൿ �߰�(��ų �ٲٰԵɶ�?
    /// </summary>
    /// <param name="_idx"></param>
    /// <param name="_action"></param>
    public void AddActions(int _idx, V _action)
    {

        if (ChkAction(_idx)) actions[_idx] = _action;
    }

    /// <summary>
    /// index �ִ��� Ȯ��
    /// </summary>
    public bool ChkAction(int _idx)
    {

        if (_idx < 0 || _idx >= actions.Length) return false;
        return true;
    }
    
    
    /*
    /// <summary>
    /// �ൿ ��ȣ �ľ�
    /// </summary>
    /// <param name="_startNum">���� ��ȣ</param>
    /// <returns></returns>
    public int GetActionNum(int _startNum = 0)
    {

        // ���� 0���� �ʱ�ȭ
        int actionType = 0;

        int add = VariableManager.MAX_ACTIONS;

        for (int i = 0; i < actions.Length; i++)
        {

            if (!actions[i].active) continue;
            if (actions[i].stateInfo.buttonOpt == VariableManager.STATE_BUTTON_OPTION.NEED_TARGET
                || actions[i].stateInfo.buttonOpt == VariableManager.STATE_BUTTON_OPTION.NEED_POS
                || actions[i].stateInfo.buttonOpt == VariableManager.STATE_BUTTON_OPTION.NEED_TARGET_OR_POS)
            {

                actionType += 1 << (i + add + _startNum);
            }
            else if (actions[i].stateInfo.buttonOpt == VariableManager.STATE_BUTTON_OPTION.BUILD)
            {

                actionType += 1 << (i + _startNum + add);

                if ((actionType & (1 << VariableManager.BUILD)) == 0) actionType += 1 << (VariableManager.BUILD + i);
            }
            else
            {

                actionType += 1 << (i + _startNum);
            }
        }

        return actionType;
    }
    */


    /// <summary>
    /// ���� ���ؼ� ������ �״�� �ΰ�, �ٸ��� buttons���� �����Ѵ�
    /// </summary>
    public void ChkButtons(ButtonInfo[] _buttons, int _startButtonIdx, int _startActionIdx, bool _resetStartButtonIdx = false)
    {

        if (_resetStartButtonIdx)
        {

            for (int i = 0; i < _startButtonIdx; i++)
            {

                _buttons[i] = ButtonInfo.Empty;
            }
        }

        int chkIdx = _buttons.Length - _startButtonIdx < actions.Length - _startActionIdx ?
            _buttons.Length - _startButtonIdx : actions.Length - _startActionIdx;

        for (int i = 0; i < chkIdx; i++)
        {

            // �ٸ� ��쿡 ����!
            if (_buttons[i + _startButtonIdx] != actions[i + _startActionIdx].buttonInfo)
            {

                _buttons[i + _startButtonIdx] = ButtonInfo.Empty;
            }
        }

        for (int i = chkIdx; i < _buttons.Length - _startButtonIdx; i++)
        {

            _buttons[i + _startButtonIdx] = ButtonInfo.Empty;
        }
    }

    /// <summary>
    /// ��ư ���� �Ѱ��ֱ�
    /// </summary>
    /// <param name="_buttons">���� ��ư ���� �迭</param>
    /// <param name="_startButtonIdx">��ư �迭�� ���� ��ȣ</param>
    /// <param name="_startActionIdx">�Ѱ��� ��ư�� ���� ��ȣ</param>
    public void GiveMyButtonInfos(ButtonInfo[] _buttons, int _startButtonIdx, int _startActionIdx)
    {

        int chkIdx = _startButtonIdx < _buttons.Length ? _startButtonIdx : _buttons.Length - 1;

        for (int i = 0; i < chkIdx; i++)
        {

            _buttons[i] = ButtonInfo.Empty;
        }

        chkIdx = _buttons.Length - _startButtonIdx < actions.Length - _startActionIdx ?
            _buttons.Length - _startButtonIdx : actions.Length - _startActionIdx;
        for (int i = 0; i < chkIdx; i++)
        {

            _buttons[i + _startButtonIdx] = actions[i + _startActionIdx].buttonInfo;
        }

        for (int i = chkIdx; i < _buttons.Length - _startButtonIdx; i++)
        {

            _buttons[i + _startButtonIdx] = ButtonInfo.Empty;
        }
    }
}