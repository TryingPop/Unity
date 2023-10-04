using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAtk", menuName = "Attack/BossAtk")]
public class RangeDouble : RangeTarget
{

    [SerializeField] protected Vector3 nextOffset;


    public override void OnAttack(Unit _unit)
    {

        Transform unitTrans = _unit.transform;
        Vector3 dir;
        
        if (_unit.MyTurn <= atkTime) dir = Quaternion.LookRotation(unitTrans.forward) * offset;
        else dir = Quaternion.LookRotation(unitTrans.forward) * nextOffset;

        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_BULLET, unitTrans.position + dir, unitTrans.forward);

        if (go)
        {

            go.SetActive(true);
            go.GetComponent<TargetMissile>().Init(unitTrans, _unit.Target, _unit.Atk, prefabIdx);
        }
    }
}
