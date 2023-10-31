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
    }

    /// <summary>
    /// ���̾�� ��ȣ ã���ִ� �޼���
    /// </summary>
    public int ChkTeamNumber(int _layer)
    {

        if (_layer == VarianceManager.LAYER_PLAYER) return VarianceManager.TEAM_PLAYER;
        else if (_layer == VarianceManager.LAYER_ENEMY) return VarianceManager.TEAM_ENEMY;
        else if (_layer == VarianceManager.LAYER_NEUTRAL) return VarianceManager.TEAM_NEUTRAL;
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