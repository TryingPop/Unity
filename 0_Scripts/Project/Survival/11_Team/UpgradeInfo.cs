using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeInfo
{

    [SerializeField] private int addAtk = 0;
    [SerializeField] private int addDef = 0;
    [SerializeField] private int addHp = 0;

    public int AddAtk => addAtk;
    public int AddDef => addDef;
    public int AddHp => addHp;

    public void UpgradeStat(TYPE_UPGRADE _type, int _grade)
    {

        switch (_type)
        {

            case TYPE_UPGRADE.ATK:
                addAtk += _grade;
                return;

            case TYPE_UPGRADE.DEF:
                addDef += _grade;
                return;

            case TYPE_UPGRADE.HP:
                addHp += _grade;
                return;

            default:
                return;
        }
    }
}
