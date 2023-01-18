using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤

    private AudioSource audioSource;

    public Button restartBtn; // 재시작 버튼
    public Text scoreText; // 점수 텍스트
    public Image gameoverImg; // 게임오버 이미지
    public Text bestscoreText; // 최고 점수 텍스트

    public bool isGameover; // 게임오버 다른 곳에서도 확인
    public static int score; // 점수
    public static int bestscore; // 최고기록
    public const int lvlup = 15; // 15 점마다 난이도 상승!
                                 // const이므로 static
    void Awake()
    {
        if (instance == null) // 인스턴스가 없는 경우 생성
        {
            instance = this; // 현재꺼를 인스턴스에 넣는다
        }
        else // 인스턴스가 이미 존재하는 경우
        {
            Destroy(gameObject); // 뒤에 넣은 대상은 파괴
        }
    }

    private void Start()
    {
        score = 0; // 스코어 0으로 초기화
                   // static으로 변수이므로 별도로 초기화가 없으면 값이 계속 이어서 간다.

        audioSource = GetComponent<AudioSource>(); // 이 스크립트를 포함하는 오브젝트에서 배경음 컴포넌트 자동으로 찾기
        if (audioSource != null) // 배경음 예외 처리
        {
            audioSource.Play(); // 배경음 재생
        }
        else
        {
            Debug.Log("게임 매니저에 오디오 소스가 없습니다."); // 없으면 디버그 로그로 보여주기
        }
        
    }

    public void Gameover() // 게임 오버
    {
        // 에외 처리 구문
        if (score > bestscore) // 현재 스코어가 최고 스코어보다 높은 경우
        {
            bestscore = score; // 최고 스코어를 현재 스코어로 갱신
        }

        isGameover = true; // 기둥 이동 및 까마귀, 기둥 생성 비활성화
        
        // 예외 처리 구문
        if (restartBtn != null) // 리스타트 버튼이 없는 경우
        {
            restartBtn.gameObject.SetActive(true); // 재시작 버튼 활성화
        }
        else // 없는 경우
        {
            Debug.Log("게임 매니저에 리스타트 버튼이 없습니다."); // 없다고 디버그 띄우기
            Restart(); // 강제 재실행
        }

        // 예외 처리 구문
        if (gameoverImg != null) // 게임 오버 이미지가 있는 경우
        {
            gameoverImg.gameObject.SetActive(true); // 게임오버 이미지 활성화
        }
        else
        {
            Debug.Log("게임 매니저에 게임오버 이미지가 없습니다."); // 게임 오버 이미지 없다고 디버그 띄우기
        }

        // 예외 처리 구문
        if (bestscoreText != null) // 최고 점수 텍스트가 있을 시
        {
            bestscoreText.gameObject.SetActive(true); // 활성화
            bestscoreText.text = $"Best Score : {bestscore}"; // 최고 점수 띄우기
        }
        else // 없는 경우
        {
            Debug.Log("게임 매니저에 최고점수 텍스트가 없습니다."); // 최고 점수 텍스트 없다고 디버그 띄우기
        }
    }

    public void Restart() // 리스타트
                          // restartbtn에서 onclick 이벤트에 쓰인다
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재씬 다시 불러오기
    }

    public void AddScore() // 스코어 추가
    {
        score++; // 스코어 추가
        // 예외 처리구문
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}"; // 스코어 메모
        }
        else
        {
            Debug.Log("게임 매니저에 스코어 텍스트가 없습니다.");
        }
    }
}
