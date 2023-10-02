using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangeTarget", menuName = "Attack/RangeTarget")]
public class RangeTarget : Attack
{

    [SerializeField] protected ushort missileIdx;
    [SerializeField] protected Vector3 offset;

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


    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;

        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
        
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(unitTrans, _unit.Target, _unit.Atk, prefabIdx);
        }
    }
}