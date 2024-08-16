using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Button/Main/Upgrade")]
public class BtnUpgrade : BtnDefault
{

    [SerializeField] TYPE_SELECTABLE upgradeType;

    public override void GetTitle(Text _titleText)
    {

        int value = GetMyUpgradeValue();
        if (value <= 0) _titleText.text = $"{title}";
        else _titleText.text = $"{title}{value}";
    }

    protected int GetMyUpgradeValue()
    {

        TeamInfo myTeam = TeamManager.instance.PlayerTeamInfo;
        return myTeam.GetLvl(upgradeType);
    }
}
