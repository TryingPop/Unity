using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private STATE_GAME myState;

    public STATE_GAME MyState => myState;

    [SerializeField] protected List<Mission> playerMissions;
    [SerializeField] protected List<Mission> enemyMissions;

    public bool IsGameOver
    {

        get
        {

            return myState == STATE_GAME.WIN
                || myState == STATE_GAME.LOSE;
        }
    }

    public bool IsStop
    {

        get
        {

            return myState == STATE_GAME.PAUSE;
        }
    }



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

    private void ChkMission(Unit _unit, Building _building, int _idx, List<Mission> _missions)
    {

        if (_idx == -1)
        {

            for (int i = _missions.Count - 1; i >= 0; i--)
            {

                _missions[i].Chk(_unit, _building);
                if (_missions[i].IsSucess) _missions.RemoveAt(i);
            }

            return;
        }

        if (_idx < 0 || _missions.Count <= _idx) return;
        _missions[_idx].Chk(_unit, _building);
        if (_missions[_idx].IsSucess) _missions.RemoveAt(_idx);
    }

    private bool ChkWin(List<Mission> _missions)
    {

        if (_missions.Count == 0) return true;
        return false;
    }

    public void Chk(Unit _unit, Building _building, int _idx = -1, bool isPlayer = true)
    {

        if (isPlayer)
        {

            ChkMission(_unit, _building, _idx, playerMissions);
            if (ChkWin(playerMissions)) GameOver(true);
        }
        else
        {

            ChkMission(_unit, _building, _idx, enemyMissions);
            if (ChkWin(enemyMissions)) GameOver(false);
        }
    }

    private void GameOver(bool _isWin)
    {

        if (_isWin) myState = STATE_GAME.WIN;
        else myState = STATE_GAME.LOSE;
    }
}
