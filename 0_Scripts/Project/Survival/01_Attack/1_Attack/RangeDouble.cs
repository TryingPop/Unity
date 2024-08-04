using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스용 두 발 쏘는거
/// </summary>
[CreateAssetMenu(fileName = "BossAtk", menuName = "Attack/BossAtk")]
public class RangeDouble : RangeTarget
{

    [SerializeField] protected Vector3 nextOffset;
    [SerializeField] protected int nextAtk;

    public override int AtkTime(int _chk)
    {

        if (_chk == atkTime) return 0;
        else if (_chk >= nextAtk) return 1;
        else return -1;
    }

    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;
        Vector3 dir;
        
        if (_unit.MyTurn < nextAtk) dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        else dir = Quaternion.LookRotation(unitTrans.forward) * nextOffset;

        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VarianceManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);

        if (go)
        {

            go.SetActive(true);
            go.GetComponent<TargetMissile>().Init(_unit, this, prefabIdx);
        }
    }
}
