using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일 생성
/// </summary>
[CreateAssetMenu(fileName = "RangeTarget", menuName = "Attack/RangeTarget")]
public class RangeTarget : Attack
{

    [SerializeField] protected int missileIdx;       // 미사일 생성
    [SerializeField] protected Vector3 offset;          // 생성 위치

    protected int prefabIdx = -1;
    
    protected int PrefabIdx
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

        // 위치 설정
        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
        
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(_unit, _unit.Atk, prefabIdx);
        }
    }
}