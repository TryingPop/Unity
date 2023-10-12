using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateHandler<T> : ScriptableObject
{

    public T[] actions;


    // public abstract void Action(T _param);

    /// <summary>
    /// 외부에서 행동 추가(스킬 바꾸게될때?
    /// </summary>
    /// <param name="_idx"></param>
    /// <param name="_action"></param>
    public void AddActions(int _idx, T _action)
    {

        if (ChkIdx(_idx)) actions[_idx] = _action;
    }

    /// <summary>
    /// index 있는지 확인
    /// </summary>
    public bool ChkIdx(int _idx)
    {

        if (_idx < 0 
            || _idx >= actions.Length
            || actions[_idx] == null) return false;
        
        return true;
    }

    public int GetSize()
    {

        return actions.Length;
    }
}