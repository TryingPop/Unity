using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
[CreateAssetMenu(fileName = "Repair", menuName = "Attack/Repair")]
public class Repair : Attack
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

        if (team == null) 
        {

#if UNITY_EDITOR

            Debug.Log($"{_unit.name}�� Team ������ �����ϴ�.");
#endif
            return atk; 
        }

        return atk + GetAddedAtk(team.lvlAtk);
    }

    public override void OnAttack(Unit _unit)
    {

        _unit.Target.Heal(GetAtk(_unit));
    }
}