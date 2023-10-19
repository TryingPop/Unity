using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 행동 인터페이스!
/// </summary>
public abstract class IAction<T> : ScriptableObject
{

    public abstract void Action(T _param);

    public abstract void OnEnter(T _param);
}
