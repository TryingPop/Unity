using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �� ���� ���� Ŭ����
/// </summary>
public class TeamManager : MonoBehaviour
{

    public static TeamManager instance;

    public TeamInfo PlayerTeamInfo => teams[VarianceManager.TEAM_PLAYER];
    public TeamInfo EnemyTeamInfo => teams[VarianceManager.TEAM_ENEMY];
    public TeamInfo NeutralTeamInfo => teams[VarianceManager.TEAM_NEUTRAL];
    public TeamInfo AllyTeamInfo => teams[VarianceManager.TEAM_ALLY];

    [SerializeField] private TeamInfo[] teams;                  // �� ������

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

        for (int i = 0; i < teams.Length; i++) 
        {

            teams[i].Init();
        }
    }

    /// <summary>
    /// ���̾�� ��ȣ ã���ִ� �޼���
    /// </summary>
    public int ChkTeamNumber(int _layer)
    {

        if (_layer == VarianceManager.LAYER_PLAYER) return VarianceManager.TEAM_PLAYER;
        else if (_layer == VarianceManager.LAYER_ENEMY) return VarianceManager.TEAM_ENEMY;
        else if (_layer == VarianceManager.LAYER_NEUTRAL) return VarianceManager.TEAM_NEUTRAL;
        else if (_layer == VarianceManager.LAYER_ALLY) return VarianceManager.TEAM_ALLY;
        else return -1;
    }

    /// <summary>
    /// �� ������ ���̾� ���� �� -> �̼� Ȯ�ο�!
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