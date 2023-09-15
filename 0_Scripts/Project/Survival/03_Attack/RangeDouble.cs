using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDouble : RangeTarget
{

    [SerializeField] protected Transform nextPos;


    public override void OnAttack(Unit _unit)
    {

        // Ç®¸µ 
        GameObject go = PoolManager.instance.GetPrefabs(missileIdx, TargetMissile.LAYER_BULLET);
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<TargetMissile>().Init(_unit.transform, _unit.Target, Target, atk);

            if (coolTime <= atkTime)
            {

                go.transform.position = initPos.position;
            }
            else
            {

                isAtk = false;
                go.transform.position = nextPos.position;
            }
        }
    }
}
