using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private STATE_GAME myState;

    public STATE_GAME MyState => myState;

    [SerializeField] protected MissionManager missions;
    public MissionManager Missions { set { missions = value; } }

    // [SerializeField] protected List<Mission> playerMissions;        // 플레이어 승리 미션
    // [SerializeField] protected List<Mission> enemyMissions;         // 플레이어 패배 미션

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

        missions.Init();
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

    public void ChkMissions()
    {

        if (missions.IsSuccess(false))
        {

            GameOver(false);
        }
        else if (missions.IsSuccess(true))
        {

            GameOver(true);
        }
    }

    /// <summary>
    /// 일시 정지에서 미션 오브젝트 키면 나오는 문구 설정
    /// </summary>
    public void SetMissionObjectText()
    {

        missions.SetMissionObjectText(winTxt, true);
        missions.SetMissionObjectText(loseTxt, false);
    }
}
