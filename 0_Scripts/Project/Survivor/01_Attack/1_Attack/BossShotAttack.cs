using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("�Ƚ��")]
public class BossShotAttack : RangeTarget
{

    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;
        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;

        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
        
        // ���� �Ⱦ���;
        if (go)
        {

            go.SetActive(true);
        }
    }
}
