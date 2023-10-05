using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("안써용")]
public class BossShotAttack : RangeTarget
{

    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;
        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;

        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
        
        // 현재 안쓴다;
        if (go)
        {

            go.SetActive(true);
            // go.GetComponent<BossShotMissile>().Init(_unit.TargetPos - unitTrans.position, _unit.Atk, 
            //     (short)(atkRange * 50), (short)(chaseRange * 50), 
            //    _unit.MyAlliance.GetLayer(false));
        }
    }
}
