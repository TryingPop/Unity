using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 거리 상관 없이 타이밍 되면 대상 공격
/// </summary>
[CreateAssetMenu(fileName = "MeleeTarget", menuName = "Attack/MeleeTarget")]
public class MeleeTarget : Attack
{

    [SerializeField] protected int atk;
    [SerializeField] protected int addedAtk;

    public override int GetAddedAtk(int _lvlInfo)
    {

        return addedAtk * _lvlInfo;
    }

    public override int GetAtk(Selectable _unit)
    {

        TeamInfo team = _unit.MyTeam;

        if (team == null) return atk;

        return atk + GetAddedAtk(team.lvlAtk);
    }

    public override void OnAttack(Unit _unit)
    {

        _unit.Target.OnDamaged(GetAtk(_unit), isPure, isEvade, _unit.transform);
    }
}