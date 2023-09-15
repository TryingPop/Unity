using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeArea : MeleeTarget
{

    public override void OnAttack(Unit _unit)
    {

        RaycastHit[] hits = Physics.SphereCastAll(_unit.transform.position, 
            atkRange, _unit.transform.forward, 0f, atkLayers);

        if (hits.Length > 1)
        {


            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].transform == _unit.transform) continue;

                hits[i].transform.GetComponent<IDamagable>()?.OnDamaged(atk);
            }
        }
        isAtk = false;
    }
}
