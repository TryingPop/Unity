using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Missile : MonoBehaviour
{

    public abstract void Init(Selectable _atker, int _atk, short _prefabIdx);

    protected abstract void Used();

    public abstract void Action();

    protected abstract void TargetAttack();
}
