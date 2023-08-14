using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTarget : Attack
{

    public override void OnAttack(Unit _unit)
    {

        // Debug.Log($"{_unit.Target.name}에게 {_unit.Atk}만큼 공격!");
        _unit.Target.GetComponent<IDamagable>()?.OnDamaged(_unit.Atk, transform);
        // Debug.Log($"{_unit.Target.GetComponent<Unit>().curHp} 남았습니다.");
    }
}