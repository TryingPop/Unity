using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 행동 모음집
/// </summary>
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

        if (actions != null
            && _idx < actions.Length
            && 0 <= _idx) actions[_idx] = _action;
    }

    /// <summary>
    /// 해당 _idx가 사용가능한지 판별
    /// 사용 불가능한 경우 -1 반환
    /// </summary>
    public abstract int GetIdx(int _idx);

    public int GetSize()
    {

        return actions.Length;
    }
}