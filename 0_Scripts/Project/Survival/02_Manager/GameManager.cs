using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private STATE_GAME myState;

    public STATE_GAME MyState => myState;

    
    [SerializeField] protected List<Mission> playerMissions;        // 플레이어 승리 미션
    [SerializeField] protected List<Mission> enemyMissions;         // 플레이어 패배 미션

    [SerializeField] protected Text winTxt;                         // 승리 조건용 텍스트
    [SerializeField] protected Text loseTxt;                        // 패배 조건용 텍스트

    [SerializeField] protected Text gameOverText;                   // 게임 끝났음을 알리는 텍스트

    private bool isStop;                                            // 메뉴 활성화 중?

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

        // 승리 조건은 최대 2개!
        if (playerMissions.Count > VarianceManager.MAX_MISSIONS)
        {

            for (int i = playerMissions.Count - 1; i >= VarianceManager.MAX_MISSIONS; i--)
            {

                playerMissions.RemoveAt(i);
            }
        }


        // 패배 조건은 최대 2개!
        if (enemyMissions.Count > VarianceManager.MAX_MISSIONS)
        {

            for (int i = enemyMissions.Count -1; i >= VarianceManager.MAX_MISSIONS; i--)
            {

                enemyMissions.RemoveAt(i);
            }
        }

        //마우스 화면 밖 못 나가게 설정
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {

        Init();
    }

    /// <summary>
    /// 미션 시작 시 행동 시작
    /// 시작에서 잡아야할 유닛을 생성한다!
    /// </summary>
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

    /// <summary>
    /// 승리 확인
    /// </summary>
    private bool ChkWin(List<Mission> _missions)
    {

        for (int i = 0; i < _missions.Count; i++)
        {

            if (!_missions[i].IsSucess) return false;
        }

        return true;
    }

    /// <summary>
    /// 조건 달성했는지 확인
    /// </summary>
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

    /// <summary>
    /// 게임 끝?
    /// </summary>
    private void GameOver(bool _isWin)
    {


        if (_isWin) myState = STATE_GAME.WIN;
        else myState = STATE_GAME.LOSE;

        gameOverText.enabled = true;
        gameOverText.text = $"{myState}";
    }

    /// <summary>
    /// 일시 정지에서 미션 오브젝트 키면 나오는 문구 설정
    /// </summary>
    public void SetMissionObjectText()
    {

        int len = Mathf.Min(2, playerMissions.Count);
        for (int i = 0; i < len; i++)
        {

            winTxt.text = $"{playerMissions[i].GetMissionObjectText(true)}\n";
        }

        len = Mathf.Min(3, enemyMissions.Count);
        for (int i = 0; i < len; i++)
        {

            loseTxt.text = $"{enemyMissions[i].GetMissionObjectText(false)}\n";
        }
    }


}
