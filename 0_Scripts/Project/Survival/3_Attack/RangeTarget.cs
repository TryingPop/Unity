using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTarget : Attack
{

    public int missileIdx;

    public override void OnAttack(Unit _unit)
    {

        /*
        Instantiate(missile, transform.position, Quaternion.identity)
            .GetComponent<Missile>().Init(_unit.transform, _unit.Target, Target,
            _unit.AtkRange * 1f, _unit.Atk);
        */
        // Ç®¸µ 
        GameObject go = PoolManager.instance.GetPrefabs(missileIdx, Missile.LAYER_BULLET);
        if (go)
        {

            go.SetActive(true);
            go.GetComponent<Missile>().Init(_unit.transform, _unit.Target, Target,
                _unit.AtkRange * 1f, _unit.Atk);
        }

        isAtk = false;
    }
}
