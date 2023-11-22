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


    public int GetSize()
    {

        return actions.Length;
    }
}