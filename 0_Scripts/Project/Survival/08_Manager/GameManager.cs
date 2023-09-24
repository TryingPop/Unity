using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
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

        if (_layer == VariableManager.LAYER_PLAYER) return 0;
        else if (_layer == VariableManager.LAYER_ENEMY) return 1;
        else return -1;
    }

    public TeamInfo GetTeamInfo(int _layer)
    {

        int teamNum = ChkTeamNumber(_layer);

        if (teamNum == -1) return null;

        return teams[teamNum];
    }
}