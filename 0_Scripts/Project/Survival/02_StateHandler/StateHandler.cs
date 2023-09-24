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
    /// 외부에서 행동 추가(스킬 바꾸게될때?
    /// </summary>
    /// <param name="_idx"></param>
    /// <param name="_action"></param>
    public void AddActions(int _idx, V _action)
    {

        if (ChkAction(_idx)) actions[_idx] = _action;
    }

    /// <summary>
    /// index 있는지 확인
    /// </summary>
    public bool ChkAction(int _idx)
    {

        if (_idx < 0 || _idx >= actions.Length) return false;
        return true;
    }
    
    
    /*
    /// <summary>
    /// 행동 번호 파악
    /// </summary>
    /// <param name="_startNum">시작 번호</param>
    /// <returns></returns>
    public int GetActionNum(int _startNum = 0)
    {

        // 전부 0으로 초기화
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
    /// 값을 비교해서 같으면 그대로 두고, 다르면 buttons에서 제거한다
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

            // 다른 경우에 해제!
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
    /// 버튼 정보 넘겨주기
    /// </summary>
    /// <param name="_buttons">담을 버튼 정보 배열</param>
    /// <param name="_startButtonIdx">버튼 배열의 시작 번호</param>
    /// <param name="_startActionIdx">넘겨줄 버튼의 시작 번호</param>
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