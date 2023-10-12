using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IAction<T> : ScriptableObject
{

    public abstract void Action(T _param);

    public abstract void OnEnter(T _param);
}
