using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¼ö¸®
/// </summary>
[CreateAssetMenu(fileName = "Repair", menuName = "Attack/Repair")]
public class Repair : Attack
{

    [SerializeField] protected int atk;
    [SerializeField] protected int addedAtk;

    public override int GetAtk(int _lvlInfo)
    {

        return atk + addedAtk * _lvlInfo;
    }

    public override void OnAttack(BaseObj _atker)
    {

        _atker.Target.Heal(CalcUnitAtk(_atker.MyTeam));
    }
}