using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// �ൿ ������
/// </summary>
public abstract class StateHandler<T> : ScriptableObject
{

    public T[] actions;



    // public abstract void Action(T _param);

    /// <summary>
    /// �ܺο��� �ൿ �߰�(��ų �ٲٰԵɶ�?
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