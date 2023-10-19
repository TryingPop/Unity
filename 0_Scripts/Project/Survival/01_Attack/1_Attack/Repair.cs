using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
[CreateAssetMenu(fileName = "Repair", menuName = "Attack/Repair")]
public class Repair : Attack
{

    public override void OnAttack(Unit _unit)
    {

        _unit.Target.Heal(_unit.Atk);
    }
}