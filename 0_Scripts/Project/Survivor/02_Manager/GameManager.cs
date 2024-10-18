using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] protected GameObject restartBtn;               // 재시작 버튼
    [SerializeField] protected GameObject nextBtn;                  // 다음 판으로 버튼
    private bool isStop;                                            // 메뉴 활성화 중?
    [SerializeField] private bool isNextBtn;

    [SerializeField] private AudioClip winSnd;
    [SerializeField] private AudioClip loseSnd;

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
    public void GameOver(bool _isWin)
    {

        if (_isWin) 
        { 
            
            myState = STATE_GAME.WIN;
            missions.Clear();

            // 마지막 판 같은 경우 다음판이 없기에 재시작 버튼을 그냥 살려둔다
            if (isNextBtn)
            {

                restartBtn.SetActive(false);
                nextBtn.SetActive(true);
            }

        }
        else myState = STATE_GAME.LOSE;

        SetGameOverSnd(_isWin);

        gameOverText.enabled = true;
        gameOverText.text = $"{myState}";
    }

    /// <summary>
    /// 소리 세팅
    /// </summary>
    /// <param name="_isWin">승리 소리 여부</param>
    private void SetGameOverSnd(bool _isWin)
    {

        if (_isWin)
        {

            SoundManager.Instance.SetSE(winSnd);
        }
        else
        {

            SoundManager.Instance.SetSE(loseSnd);
        }
    }

    public void AddMission(Mission _mission)
    {

        missions.AddMission(_mission);
    }

    public void RemoveMission(Mission _mission)
    {

        missions.RemoveMission(_mission);
        
    }

    /// <summary>
    /// 일시 정지에서 미션 오브젝트 키면 나오는 문구 설정
    /// </summary>
    public void SetMissionObjectText()
    {

        missions.SetMissionObjectText(winTxt, loseTxt);
    }
}