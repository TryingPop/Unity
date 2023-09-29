using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossShot", menuName = "Attack/BossShot")]
public class BossShotAttack : RangeTarget
{

    public override void OnAttack(Unit _unit)
    {

        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, TargetMissile.LAYER_BULLET);
        if (go)
        {

            Transform unitTrans = _unit.transform;

            go.SetActive(true);
            go.GetComponent<BossShotMissile>().Init(_unit.TargetPos - unitTrans.position, _unit.Atk, 
                (short)(atkRange * 50), (short)(chaseRange * 50), 
                _unit.MyAlliance.GetLayer(false));

            Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;
            go.transform.position = dir + unitTrans.position;
        }
    }
}
