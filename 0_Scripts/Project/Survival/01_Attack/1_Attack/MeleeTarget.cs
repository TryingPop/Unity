using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeTarget", menuName = "Attack/MeleeTarget")]
public class MeleeTarget : Attack
{

    public override void OnAttack(Unit _unit)
    {

        _unit.Target.OnDamaged(_unit.Atk, _unit.transform);
    }
}