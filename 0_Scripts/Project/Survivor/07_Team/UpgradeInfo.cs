using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 업그레이드 정보
/// </summary>
[System.Serializable]
public class UpgradeInfo
{

    [SerializeField] private int addAtk = 0;
    [SerializeField] private int addDef = 0;
    [SerializeField] private int addHp = 0;

    public int AddAtk => addAtk;
    public int AddDef => addDef;
    public int AddHp => addHp;

    public void UpgradeStat(TYPE_MANAGEMENT _type, int _grade)
    {

        switch (_type)
        {

            case TYPE_MANAGEMENT.UP_ATK:
                addAtk += _grade;
                return;

            case TYPE_MANAGEMENT.UP_DEF:
                addDef += _grade;
                return;

            case TYPE_MANAGEMENT.UP_HP:
                addHp += _grade;
                return;

            default:
                return;
        }
    }
}
