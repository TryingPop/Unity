using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private STATE_GAME myState;

    public STATE_GAME MyState => myState;

    [SerializeField] protected List<Mission> playerMissions;
    [SerializeField] protected List<Mission> enemyMissions;

    [SerializeField] protected Text winTxt;
    [SerializeField] protected Text loseTxt;

    [SerializeField] protected Text gameOverText;

    private bool isStop;

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

        set
        {

            isStop = value;
            Time.timeScale = value ? 0f : 1f;
        }

        get
        {

            return isStop;
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

        if (playerMissions.Count > VariableManager.MAX_MISSIONS)
        {

            for (int i = playerMissions.Count - 1; i >= VariableManager.MAX_MISSIONS; i--)
            {

                playerMissions.RemoveAt(i);
            }
        }

        if (enemyMissions.Count > VariableManager.MAX_MISSIONS)
        {

            for (int i = enemyMissions.Count -1; i >= VariableManager.MAX_MISSIONS; i--)
            {

                enemyMissions.RemoveAt(i);
            }
        }
    }

    private void Start()
    {

        Init();
    }

    private void Init()
    {

        isStop = false;
        myState = STATE_GAME.NONE;

        for (int i = 0; i < playerMissions.Count; i++)
        {

            playerMissions[i].Init(this);
        }

        for (int i = 0; i < enemyMissions.Count; i++)
        {

            enemyMissions[i].Init(this);
        }
    }

    /// <summary>
    /// 미션 확인
    /// </summary>
    /// <param name="_unit">확인할 유닛</param>
    /// <param name="_building">확인할 건물</param>
    /// <param name="_idx">몇번째 미션 확인할지</param>
    /// <param name="_missions">확인할 미션번호</param>
    private void ChkMission(Unit _unit, Building _building, List<Mission> _missions)
    {

        for (int i = 0; i <= 0; i++)
        {

            _missions[i].Chk(_unit, _building);
        }
    }

    private bool ChkWin(List<Mission> _missions)
    {

        for (int i = 0; i < _missions.Count; i++)
        {

            if (!_missions[i].IsSucess) return false;
        }

        return true;
    }

    public void Chk(Unit _unit, Building _building)
    {

        if (IsGameOver) return;

        ChkMission(_unit, _building, playerMissions);
        if (ChkWin(playerMissions)) 
        { 

            GameOver(true);
            return;
        }
        ChkMission(_unit, _building, enemyMissions);
        if (ChkWin(enemyMissions)) GameOver(false);
    }

    private void GameOver(bool _isWin)
    {


        if (_isWin) myState = STATE_GAME.WIN;
        else myState = STATE_GAME.LOSE;

        gameOverText.enabled = true;
        gameOverText.text = $"{myState}";
    }

    public void SetMissionObjectText()
    {

        int len = Mathf.Min(2, playerMissions.Count);
        for (int i = 0; i < len; i++)
        {

            winTxt.text = $"{playerMissions[i].GetMissionObjectText()}\n";
        }

        len = Mathf.Min(3, enemyMissions.Count);
        for (int i = 0; i < len; i++)
        {

            loseTxt.text = $"{enemyMissions[i].GetMissionObjectText()}\n";
        }
    }


}
