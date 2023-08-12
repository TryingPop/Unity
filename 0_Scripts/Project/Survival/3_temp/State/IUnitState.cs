using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitState
{

    public bool IsDone { get; }

    public abstract void Execute();
}
