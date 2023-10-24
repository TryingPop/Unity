using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 팀 정보 보유 클래스
/// </summary>
public class TeamManager : MonoBehaviour
{

    public static TeamManager instance;
    //[SerializeField] private AllianceInfo[] teams;      // 팀 상태
    // [SerializeField] private UpgradeInfo[] upgrades;    // 업글 상태

    // public AllianceInfo PlayerTeamInfo => teams[VariableManager.TEAM_PLAYER];
    // public AllianceInfo EnemyTeamInfo => teams[VariableManager.TEAM_ENEMY];
    // public AllianceInfo NeutralTeamInfo => teams[VariableManager.TEAM_NEUTRAL];

    // public UpgradeInfo PlayerUpgradeInfo => upgrades[VariableManager.TEAM_PLAYER];
    // public UpgradeInfo EnemyUpgradeInfo => upgrades[VariableManager.TEAM_ENEMY];
    // public UpgradeInfo NeutralUpgradeInfo => upgrades[VariableManager.TEAM_NEUTRAL];

    [SerializeField] private TeamInfo[] teams;


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

    /*
    /// <summary>
    /// 레이어로 팀 정보 찾기
    /// </summary>
    public AllianceInfo GetTeamInfo(int _layer)
    {

        int teamNum = ChkTeamNumber(_layer);

        if (teamNum == -1
            || teamNum >= teams.Length) return null;

        return teams[teamNum];
    }

    /// <summary>
    /// 레이어로 업그레이드 정보 찾기
    /// </summary>
    public UpgradeInfo GetUpgradeInfo(int _layer)
    {

        int teamNum = ChkTeamNumber(_layer);

        if (teamNum == -1
            || teamNum >= upgrades.Length) return null;

        return upgrades[teamNum];
    }
    */

    /// <summary>
    /// 팀 정보와 레이어 정보 비교 -> 미션 확인용!
    /// </summary>
    public bool CompareTeam(TeamInfo _teamInfo, int _layer)
    {

        return _teamInfo.TeamLayerNumber == _layer;
    }
}