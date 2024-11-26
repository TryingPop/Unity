using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̻��� ����
/// </summary>
[CreateAssetMenu(fileName = "GenerateMissile", menuName = "Attack/GenerateMissile")]
public class GenerateMissile : Attack
{

    [SerializeField] protected int missileIdx;       // �̻��� ����
    [SerializeField] protected Vector3 offset;          // ���� ��ġ
    [SerializeField] protected Attack atkType;

    protected int prefabIdx = -1;

    public override int GetAtk(int _lvlInfo)
    { 
        
        return atkType.GetAtk(_lvlInfo); 
    }

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


    public override void OnAttack(BaseObj _unit)
    {

        Transform unitTrans = _unit.transform;

        // ��ġ ����
        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VarianceManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);
        
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(_unit, this, prefabIdx);
        }
    }
}