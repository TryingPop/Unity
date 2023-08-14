using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTarget : Attack
{

    public GameObject missile;

    public override void OnAttack(Unit _unit)
    {

        GameObject go = Instantiate(missile, transform.position, Quaternion.identity);
        go.GetComponent<Missile>().Init(_unit.transform, _unit.Target, 
            _unit.AtkRange * 1f, _unit.Atk);
    }
}
