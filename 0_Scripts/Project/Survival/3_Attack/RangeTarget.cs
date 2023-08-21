using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTarget : Attack
{

    [SerializeField] protected int missileIdx;
    [SerializeField] protected Transform initPos;

    public override void OnAttack(Unit _unit)
    {

        // Ç®¸µ 
        GameObject go = PoolManager.instance.GetPrefabs(missileIdx, Missile.LAYER_BULLET);
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(_unit.transform, _unit.Target, Target, atk);

            go.transform.position = initPos.position;
        }

        isAtk = false;
    }
}
