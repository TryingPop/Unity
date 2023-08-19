using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTarget : Attack
{

    public override void OnAttack(Unit _unit)
    {

        target?.OnDamaged(_unit.Atk, transform);
        isAtk = false;
    }
}