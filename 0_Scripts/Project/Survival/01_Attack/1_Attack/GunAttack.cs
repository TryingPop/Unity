using System.Collections;
using System.Collections.Generic;
using UnityEditor.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Attack/Gun")]
public class GunAttack : RangeTarget
{

    [SerializeField] protected ushort effectIdx;
    protected short effectPrefabIdx = -1;

    [SerializeField] protected Vector3 effectOffset;

    protected short EffectPrefabIdx
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

    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;

        Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);

        if (go)
        {

            go.SetActive(true);
            go.GetComponent<TargetMissile>().Init(unitTrans, _unit.Target, _unit.Atk, prefabIdx);
            dir = Quaternion.LookRotation(unitTrans.forward) * effectOffset;

            GameObject bulletCase = PoolManager.instance.GetPrefabs(EffectPrefabIdx, VariableManager.LAYER_DEAD, unitTrans.position + dir, unitTrans.forward);
            bulletCase.GetComponent<BulletRotation>().Init();
        }
    }
}