using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 범위 공격
/// </summary>
[CreateAssetMenu(fileName = "MeleeArea", menuName = "Attack/MeleeArea")]
public class MeleeArea : MeleeTarget
{

    public override void OnAttack(Unit _unit)
    {

        int len = Physics.SphereCastNonAlloc(_unit.transform.position, 
            atkRange, _unit.transform.forward, VarianceManager.hits, 0f, _unit.MyTeam.EnemyLayer);

        for (int i = 0; i < len; i++)
        {

            if (VarianceManager.hits[i].transform == _unit.transform) continue;

            VarianceManager.hits[i].transform.GetComponent<IDamagable>()?.OnDamaged(GetAtk(_unit), isPure, isEvade);
        }
    }
}
