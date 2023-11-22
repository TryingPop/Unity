using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Button/Main/Upgrade")]
public class BtnUpgrade : BtnDefault
{

    [SerializeField] TYPE_MANAGEMENT upgradeType;

    public override void GetTitle(Text _titleText)
    {

        int value = GetMyUpgradeValue();
        if (value <= 0) _titleText.text = $"{title}";
        else _titleText.text = $"{title}{value}";
    }

    protected int GetMyUpgradeValue()
    {

        TeamInfo myTeam = TeamManager.instance.PlayerTeamInfo;

        switch (upgradeType)
        {

            case TYPE_MANAGEMENT.UP_ATK:
                return myTeam.AddedAtk;

            case TYPE_MANAGEMENT.UP_DEF:
                return myTeam.AddedDef;

            case TYPE_MANAGEMENT.UP_HP:
                return myTeam.AddedHp;

            case TYPE_MANAGEMENT.UP_SUPPLY:
                return myTeam.AddedSupply;

            case TYPE_MANAGEMENT.UP_GOLD:
                return myTeam.AddGetGold;

            default:
                return -1;
        }
    }
}
