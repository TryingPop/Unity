using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShotAttack : RangeTarget
{

    [SerializeField] protected float sizeUpSpeed;

    public override void OnAttack(Unit _unit)
    {

        GameObject go = PoolManager.instance.GetPrefabs(missileIdx, TargetMissile.LAYER_BULLET);
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<BossShotMissile>().Init(_unit.TargetPos - _unit.transform.position, atk, (short)(atkRange * 50), (short)(chaseRange * 50), sizeUpSpeed);
            go.transform.position = initPos.position;
        }
    }
}
