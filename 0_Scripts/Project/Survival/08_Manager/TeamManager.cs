using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{

    public static TeamManager instance;
    [SerializeField] private AllianceInfo[] teams;

    [SerializeField] private UpgradeInfo[] upgrades;

    public AllianceInfo PlayerTeamInfo => teams[VariableManager.TEAM_PLAYER];
    public AllianceInfo EnemyTeamInfo => teams[VariableManager.TEAM_ENEMY];

    public UpgradeInfo PlayerUpgradeInfo => upgrades[VariableManager.TEAM_PLAYER];
    public UpgradeInfo EnemyUpgradeInfo => upgrades[VariableManager.TEAM_ENEMY];

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 레이어로 번호 찾아주는 메서드
    /// </summary>
    public int ChkTeamNumber(int _layer)
    {

        if (_layer == VariableManager.LAYER_PLAYER) return VariableManager.TEAM_PLAYER;
        else if (_layer == VariableManager.LAYER_ENEMY) return VariableManager.TEAM_ENEMY;
        else return -1;
    }

    public AllianceInfo GetTeamInfo(int _layer)
    {

        int teamNum = ChkTeamNumber(_layer);

        if (teamNum == -1) return null;

        return teams[teamNum];
    }

    public UpgradeInfo GetUpgradeInfo(int _layer)
    {

        int teamNum = ChkTeamNumber(_layer);

        if (teamNum == -1) return null;

        return upgrades[teamNum];
    }
}