using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Missile : MonoBehaviour, IActionable
{

    protected BaseObj atker;
    protected Attack atkType;

    public abstract void Init(BaseObj _atker, Attack _atkType, int _prefabIdx);

    protected abstract void Used();

    public abstract void Action();
}
