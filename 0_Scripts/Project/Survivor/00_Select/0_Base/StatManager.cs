using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스텟 계산기 역할 해준다.
/// </summary>
public static class StatManager
{
    
    /// <summary>
    /// 업그레이드 정도 가져온다.
    /// </summary>
    public static int GetUpgrade(TeamInfo _team, TYPE_SELECTABLE _type)
    {

        if (_team == null) return 0;
        return _team.GetLvl(_type);
    }

    /// <summary>
    /// 유닛 공격력 계산 메소드
    /// </summary>
    public static int CalcUnitAtk(TeamInfo _team, Attack _atk)
    {

        int lvl = GetUpgrade(_team, TYPE_SELECTABLE.UP_UNIT_ATK);
        return _atk.GetAtk(lvl);
    }

    /// <summary>
    /// 유닛 공격력 계산 메소드
    /// </summary>
    public static int CalcUnitAtk(int _lvl, Attack _atk)
    {

        return _atk.GetAtk(_lvl);
    }

    /// <summary>
    /// 유닛 최대 체력 계산 메소드
    /// </summary>
    public static int CalcUnitMaxHp(TeamInfo _team, Stats _stats)
    {

        int lvl = GetUpgrade(_team, TYPE_SELECTABLE.UP_UNIT_HP);
        return _stats.GetMaxHp(lvl);
    }

    /// <summary>
    /// 유닛 최대 체력 계산 메소드
    /// </summary>
    public static int CalcUnitMaxHp(int _lvl, Stats _stats)
    {

        return _stats.GetMaxHp(_lvl);
    }

    /// <summary>
    /// 유닛 방어력 계산 메소드
    /// </summary>
    public static int CalcUnitDef(TeamInfo _team, Stats _stats)
    {

        int lvl = GetUpgrade(_team, TYPE_SELECTABLE.UP_UNIT_DEF);
        return _stats.GetDef(lvl);
    }

    /// <summary>
    /// 유닛 방어력 계산 메소드
    /// </summary>
    public static int CalcUnitDef(int _lvl, Stats _stats)
    {

        return _stats.GetDef(_lvl);
    }

    /// <summary>
    /// 건물 최대체력 계산 메소드
    /// </summary>
    public static int CalcBuildingMaxHp(TeamInfo _team, Stats _stats)
    {

        int lvl = GetUpgrade(_team, TYPE_SELECTABLE.UP_BUILDING_HP);
        return _stats.GetMaxHp(lvl);
    }

    /// <summary>
    /// 건물 최대 체력 계산 메소드
    /// </summary>
    public static int CalcBuildingMaxHp(int _lvl, Stats _stats)
    {

        return _stats.GetMaxHp(_lvl);
    }

    /// <summary>
    /// 건물 방어력 계산 메소드
    /// </summary>
    public static int CalcBuildingDef(TeamInfo _team, Stats _stats)
    {

        int lvl = GetUpgrade(_team, TYPE_SELECTABLE.UP_BUILDING_DEF);
        return _stats.GetDef(lvl);
    }

    /// <summary>
    /// 건물 방어력 계산 메소드
    /// </summary>
    public static int CalcBuildingDef(int _lvl, Stats _stats)
    {

        return _stats.GetDef(_lvl);
    }
}