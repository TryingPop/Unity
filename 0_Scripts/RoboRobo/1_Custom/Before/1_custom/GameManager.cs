using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // GameManager 싱글톤
    public static GameManager instance;
    
    public int enemyNum;

    #region ResultUI
    [Header("결과 창")]
    [SerializeField, Tooltip("결과창 UI")] private GameObject resultUI; 
    [SerializeField, Tooltip("상황 설명 텍스트")] private Text informText; 
    [SerializeField, Tooltip("버튼 텍스트")] private Text btnText;

    #endregion

    [SerializeField, Tooltip("스테이지 정보")] Stage[] stageInfo;

    [SerializeField, Tooltip("기본 브금")] private AudioClip bgmSnd;
    [SerializeField, Tooltip("치트 모드 활성화 시 사용할 노래")] private AudioClip[] cheatSnd;
    [SerializeField, Tooltip("플레이어 애니메이터")] private PlayerController controller;

    [Tooltip("사냥 미션")] public HuntingMission huntingMission;

    [SerializeField] private AudioScript myAS;

    public event EventHandler otherReset;

    /// <summary>
    /// 게임 상태
    /// </summary>
    public enum GAMESTATE 
    { 
        Play, // 진행 중
        Gameover // 게임 종료
    }
    
    public GAMESTATE state;     // 현재 게임 상태


    public bool accBool;        // 가속 시간

    public bool uiBool;         // ui 상태 변수

    private bool beCheat;       // 치트 사용 유무

    private int stageNum;       // 현재 스테이지

    private void Awake()
    {

        // 싱글톤 할당
        // is null과 == null 차이 주의하기!
        if (instance == null) 
        {

            instance = this;
        }
        else
        {

            // 뒤에 추가되는 건 게임매니저가 사라짐
            Destroy(gameObject); 
        }

        if (huntingMission == null) huntingMission = FindObjectOfType<HuntingMission>(); 
        if (controller == null) controller = FindObjectOfType<PlayerController>();
        if (myAS == null) myAS = GetComponent<AudioScript>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 초기 상태 
        Reset();

        beCheat = false;

        myAS?.SetSnd(bgmSnd);
        myAS?.GetSnd(true);
    }

    private void Update()
    {

        // esc를 누르면 일시 정지! 만들기
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {

            // 상태 체크
            ChkState(); 
        }
    }

    /// <summary>
    /// 일시 정지 해야하는지 재개해야하는지 판별
    /// </summary>
    private void ChkState() 
    {

        // 게임 상태에 따라 상황 결정
        switch (state)
        {

            // 진행 중이면 일시 정지 및 일시 정지 ui 세팅
            case GAMESTATE.Play:
                ChkUI();
                PauseText();
                break;

            // 게임 오버면 게임 오버 ui 세팅
            case GAMESTATE.Gameover:
                ChkUI();
                break;
            
            default:
                break;
        }
    }

    /// <summary>
    /// 일시 정지 인지 확인
    /// </summary>
    private void ChkUI()
    {

        // 마우스 커서
        SetCursor();

        uiBool = !uiBool;

        // result ui uiBool에 맞게 세팅
        resultUI.SetActive(uiBool);
    }

    /// <summary>
    /// 마우스 커서 세팅
    /// </summary>
    private void SetCursor()
    {

        if (stageNum == -1) return;


        if (uiBool)
        {

            // 마우스 커서 안보이고 중앙에 고정 
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {

            // 마우스 커서 보이고 화면 밖에 못나가도록만 수정
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

    }

    /// <summary>
    /// 일시 정지 ui 텍스트 설정 메소드
    /// </summary>
    private void PauseText()
    {

        // 일시 정지 텍스트 및 버튼 이름 설정 및 멈춤 효과
        if (uiBool)
        {

            Time.timeScale = 0f;
            btnText.text = "Resume";
            informText.text = "PAUSE";
        }
        else
        {

            // 게임 속도 1로 재개
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// 게임 오버 ui 텍스트 설정 메소드
    /// </summary>
    /// <param name="winBool">승패 확인용 변수</param>
    private void GameOverText(bool winBool)
    {

        // 버튼 이름 재시작
        btnText.text = "Restart";

        // 이긴 경우
        if (winBool)
        {

            // 승리 
            informText.text = "WIN";
        }
        else
        {

            // 패배
            informText.text = "LOSE";
        }
    }

    /// <summary>
    /// 게임 오버 메소드
    /// </summary>
    /// <param name="winBool">승패 확인</param>
    public void GameOver(bool winBool)
    {

        // 마우스 커서 보이고 화면 밖에 못나가게 제한
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // 게임 오버 상태
        state = GAMESTATE.Gameover;

        // 게임 속도 1/10으로 설정
        // accBool = 10f;
        Time.timeScale = 0.1f;

        if (winBool)
        {

            AddStageNum();
        }

        // resultUI 가 있으면 게임 오버 ui 띄우고 없으면 바로 다시 시작
        if (resultUI != null)
        {

            GameOverText(winBool);

            ChkUI();
        }
        else
        {

            // 현재 씬 재시작
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// 커스텀 로보로보 게임 스타트 메소드
    /// </summary>
    public void GameStart() 
    {
        if (state == GAMESTATE.Play)
        {

            // 게임 진행 중이면 일시 정지
            ChkUI();
            PauseText();
        }
        else if (state == GAMESTATE.Gameover)
        {


            Reset();

            // 커스텀 로보로보 스타트
            SceneManager.LoadScene(GetStageName());


            otherReset(this, EventArgs.Empty);
            controller.Init();

            // accTime = 1f;
        }
    }

    private void Reset()
    {

        state = GAMESTATE.Play;
        Time.timeScale = 1.0f;
        SetMission();

        if (uiBool)
        {

            ChkUI();
        }
    }

    private void AddStageNum()
    {

        stageNum++;

        if (stageNum >= stageInfo.Length)
        {

            stageNum = -1;
        }
    }

    private string GetStageName()
    {

        if (stageNum == -1)
        {

            return "0_title";
        }
        else
        {

            return stageInfo[stageNum].stageName;
        }
    }

    private void SetMission()
    {

        if (stageNum != -1)
        {

            huntingMission.SetTargetNum(stageInfo[stageNum].targetNum);
        }
        else
        {

            huntingMission.SetTargetNum(0);
        }
    }

    /// <summary>
    /// 적 죽을 때 마다 적 카운트 감소 
    /// 이벤트 메소드
    /// </summary>
    public void ChkWin()
    {

        huntingMission.ChangeDestroyCnt();
        huntingMission.ChkRemainEnemyCnt();

        // 적 수가 0 이하면 게임 승리 메소드 실행
        if (huntingMission.ChkWin() && state != GAMESTATE.Gameover)
        {

            // 게임 승리
            GameOver(true);
        }
    }

    public void SetAudio()
    {
        if (!beCheat)
        {

            beCheat = true;
            myAS?.SetSnd(cheatSnd[UnityEngine.Random.Range(0, cheatSnd.Length)]);
            myAS?.GetSnd(true);
        }
    }

    
}


[Serializable]
public class Stage
{

    public string stageName;
    public int targetNum;
}