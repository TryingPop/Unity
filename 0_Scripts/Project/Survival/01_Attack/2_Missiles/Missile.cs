using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Missile : MonoBehaviour
{

    protected Selectable atker;
    protected Attack atkType;

    public abstract void Init(Selectable _atker, Attack _atkType, int _prefabIdx);

    protected abstract void Used();

    public abstract void Action();

    protected abstract void TargetAttack();
}
