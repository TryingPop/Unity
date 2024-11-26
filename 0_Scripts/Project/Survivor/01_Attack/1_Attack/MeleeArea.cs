using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 범위 공격
/// </summary>
[CreateAssetMenu(fileName = "MeleeArea", menuName = "Attack/MeleeArea")]
public class MeleeArea : MeleeTarget
{

    public override void OnAttack(BaseObj _atker)
    {

        int len = Physics.SphereCastNonAlloc(_atker.transform.position, 
            atkRange, _atker.transform.forward, VarianceManager.hits, 0f, _atker.MyTeam.EnemyLayer);

        int atk = StatManager.CalcUnitAtk(_atker.MyTeam, this);
        for (int i = 0; i < len; i++)
        {

            if (VarianceManager.hits[i].transform == _atker.transform) continue;
            VarianceManager.hits[i].transform.GetComponent<IDamagable>()?.OnDamaged(atk, isPure, isEvade);
        }
    }
}
