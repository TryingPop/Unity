using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ÿ� ��� ���� Ÿ�̹� �Ǹ� ��� ����
/// </summary>
[CreateAssetMenu(fileName = "MeleeTarget", menuName = "Attack/MeleeTarget")]
public class MeleeTarget : Attack
{

    [SerializeField] protected int atk;
    [SerializeField] protected int addedAtk;

    public override int GetAtk(int _lvlInfo)
    {

        return addedAtk * _lvlInfo + atk;
    }

    public override void OnAttack(BaseObj _atker)
    {

        _atker.Target.OnDamaged(CalcUnitAtk(_atker.MyTeam), isPure, isEvade, _atker.transform);
    }
}