using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ѿ� ����
/// </summary>
[CreateAssetMenu(fileName = "Gun", menuName = "Attack/Gun")]
public class GunAttack : RangeTarget
{

    [SerializeField] protected int effectIdx;            // ź�� ����Ʈ
    protected int effectPrefabIdx = -1;

    [SerializeField] protected Vector3 effectOffset;        // ����Ʈ ���� ��ġ

    protected int EffectPrefabIdx
    {
        get 
        {
            if (effectPrefabIdx == -1)
            {

                effectPrefabIdx = PoolManager.instance.ChkIdx(effectIdx);
            }

            return effectPrefabIdx;
        } 
    }

    /// <summary>
    /// ���� ź�� ����
    /// </summary>
    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;

        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);

        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(_unit, _unit.Atk, prefabIdx);
            dir = Quaternion.LookRotation(unitTrans.forward) * effectOffset;

            GameObject bulletCase = PoolManager.instance.GetPrefabs(EffectPrefabIdx, VariableManager.LAYER_DEAD, unitTrans.position + dir, unitTrans.forward);
            bulletCase.GetComponent<BulletCase>().Init();
        }
    }
}