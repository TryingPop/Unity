using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 팀 정보 보유 클래스
/// </summary>
public class TeamManager : MonoBehaviour
{

    public static TeamManager instance;

    public TeamInfo PlayerTeamInfo => teams[VariableManager.TEAM_PLAYER];
    public TeamInfo EnemyTeamInfo => teams[VariableManager.TEAM_ENEMY];
    public TeamInfo NeutralTeamInfo => teams[VariableManager.TEAM_NEUTRAL];

    [SerializeField] private TeamInfo[] teams;                  // 팀 정보들

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

    /// <summary>
    /// 팀 정보와 레이어 정보 비교 -> 미션 확인용!
    /// </summary>
    public bool CompareTeam(TeamInfo _teamInfo, int _layer)
    {

        return _teamInfo.TeamLayerNumber == _layer;
    }

    public TeamInfo GetTeamInfo(int _layer)
    {

        int idx = ChkTeamNumber(_layer);

        if (idx == -1) return null;

        return teams[idx];
    }
}