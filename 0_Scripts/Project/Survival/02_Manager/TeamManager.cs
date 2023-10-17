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
    public AllianceInfo NeutralTeamInfo => teams[VariableManager.TEAM_NEUTRAL];

    public UpgradeInfo PlayerUpgradeInfo => upgrades[VariableManager.TEAM_PLAYER];
    public UpgradeInfo EnemyUpgradeInfo => upgrades[VariableManager.TEAM_ENEMY];
    public UpgradeInfo NeutralUpgradeInfo => upgrades[VariableManager.TEAM_NEUTRAL];

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
        else if (_layer == VariableManager.LAYER_NEUTRAL) return VariableManager.TEAM_NEUTRAL;
        else return -1;
    }

    public AllianceInfo GetTeamInfo(int _layer)
    {

        int teamNum = ChkTeamNumber(_layer);

        if (teamNum == -1
            || teamNum >= teams.Length) return null;

        return teams[teamNum];
    }

    public UpgradeInfo GetUpgradeInfo(int _layer)
    {

        int teamNum = ChkTeamNumber(_layer);

        if (teamNum == -1
            || teamNum >= upgrades.Length) return null;

        return upgrades[teamNum];
    }

    public bool CompareTeam(AllianceInfo _allianceInfo, int _layer)
    {

        return _allianceInfo.TeamLayer == _layer;
    }
}