using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̻��� ����
/// </summary>
[CreateAssetMenu(fileName = "RangeTarget", menuName = "Attack/RangeTarget")]
public class RangeTarget : Attack
{

    [SerializeField] protected int missileIdx;       // �̻��� ����
    [SerializeField] protected Vector3 offset;          // ���� ��ġ

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

        // ��ġ ����
        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
        
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(_unit, _unit.Atk, prefabIdx);
        }
    }
}