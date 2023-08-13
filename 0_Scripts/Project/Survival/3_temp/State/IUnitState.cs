using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitState<T> where T : BaseUnit
{ 
    public abstract void Execute(T _state);

    public abstract void Reset(T _state);
}