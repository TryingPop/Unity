using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTarget : Attack
{

    public override void OnAttack(Unit _unit)
    {

        // Debug.Log($"{_unit.Target.name}���� {_unit.Atk}��ŭ ����!");
        _unit.Target.GetComponent<IDamagable>()?.OnDamaged(_unit.Atk, transform);
        // Debug.Log($"{_unit.Target.GetComponent<Unit>().curHp} ���ҽ��ϴ�.");
    }
}