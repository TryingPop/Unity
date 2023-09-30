using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAtk", menuName = "Attack/BossAtk")]
public class RangeDouble : RangeTarget
{

    [SerializeField] protected Vector3 nextOffset;


    public override void OnAttack(Unit _unit)
    {

        // Ç®¸µ 
        GameObject go = PoolManager.instance.GetPrefabs(PrefabIdx, TargetMissile.LAYER_BULLET);
        if (go)
        {


            Transform unitTrans = _unit.transform;

            go.SetActive(true);
            go.GetComponent<TargetMissile>().Init(unitTrans, _unit.Target, _unit.Atk, prefabIdx);


            // if (coolTime <= atkTime)
            if (_unit.MyTurn <= atkTime)
            {

                Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * offset;

                go.transform.position = dir + unitTrans.position;
            }
            else
            {

                Vector3 dir = Quaternion.LookRotation(unitTrans.forward) * nextOffset;

                go.transform.position = dir + unitTrans.position;
            }
        }
    }
}
