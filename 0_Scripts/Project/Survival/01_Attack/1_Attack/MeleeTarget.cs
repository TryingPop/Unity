using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ÿ� ��� ���� Ÿ�̹� �Ǹ� ��� ����
/// </summary>
[CreateAssetMenu(fileName = "MeleeTarget", menuName = "Attack/MeleeTarget")]
public class MeleeTarget : Attack
{

    public override void OnAttack(Unit _unit)
    {

        _unit.Target.OnDamaged(GetAtk(_unit), isPure, isEvade, _unit.transform);
    }
}