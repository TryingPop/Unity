using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 거리 상관 없이 타이밍 되면 대상 공격
/// </summary>
[CreateAssetMenu(fileName = "MeleeTarget", menuName = "Attack/MeleeTarget")]
public class MeleeTarget : Attack
{

    public override void OnAttack(Unit _unit)
    {

        _unit.Target.OnDamaged(_unit.Atk, _unit.transform);
    }
}