using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossShot", menuName = "Attack/BossShot")]
public class BossShotAttack : RangeTarget
{

    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;
        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;

        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
        
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<BossShotMissile>().Init(_unit.TargetPos - unitTrans.position, _unit.Atk, 
                (short)(atkRange * 50), (short)(chaseRange * 50), 
                _unit.MyAlliance.GetLayer(false));
        }
    }
}
