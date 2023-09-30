using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeTarget", menuName = "Attack/RangeTarget")]
public class RangeTarget : Attack
{

    [SerializeField] protected ushort missileIdx;
    protected short prefabIdx = -1;
    protected short PrefabIdx
    {

        get
        {

            if (prefabIdx == -1)
            {

                prefabIdx = PoolManager.instance.ChkIdx(missileIdx);
            }

            return prefabIdx;
        }
    }
    [SerializeField] protected Vector3 offset;

    public override void OnAttack(Unit _unit)
    {

        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, TargetMissile.LAYER_BULLET);
        if (go)
        {

            Transform unitTrans = _unit.transform;

            go.SetActive(true);
            go.GetComponent<TargetMissile>().Init(unitTrans, _unit.Target, _unit.Atk, prefabIdx);

            Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;

            go.transform.position = dir + unitTrans.position;
        }
    }
}
