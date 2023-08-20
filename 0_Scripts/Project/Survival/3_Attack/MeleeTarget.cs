using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTarget : Attack
{

    public override void OnAttack(Unit _unit)
    {

        target?.OnDamaged(atk, transform);
        isAtk = false;
    }
}