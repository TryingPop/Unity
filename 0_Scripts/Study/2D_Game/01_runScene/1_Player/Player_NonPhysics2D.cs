using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_NonPhysics2D : MonoBehaviour
{

    // 선언
    public float speed = 3.0f;      // 플레이어 캐릭터의 속도
    // public float speed = 15.0f;  // 첫 번째 장애물 거리가 너무 좁아 다음 값으로 수정 했다
    public Sprite[] run;            // 플레이어 캐릭터의 달리기 스프라이트
    public Sprite[] jump;           // 플레이어 캐릭터의 점프 스프라이트

    // 내부에서 다루는 변수
    float jumpVy;                   // 플레이어 캐릭터의 상승 속도
    int animIndex;                  // 플레이어 캐릭터 애니메이션 재생 인덱스
    bool goalCheck;                 // 골인했는지 체크

    GameObject goCam;               // 추가된 변수 메인 카메라

    // 메시지에 대응한 코드

    // 컴포넌트 실행 시작
    void Start()
    {

        // 초기화
        jumpVy = 0;
        animIndex = 0;
        goalCheck = false;

        goCam = GameObject.Find("Main Camera");
    }

    // 플레이어 캐릭터에 적용된 충돌 판정 영역에 다른 게임 오브젝트의 충돌 판정 영역이 겹쳤다
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // 들어왔는지 검사
        if (collision.gameObject.name == "Stage_Gate")
        {

            goalCheck = true;
            return;
        }

        // 골인 지점이 아닌 곳이라면 초기화 해야한다
        // Application.LoadLevel(Application.loadedLevelName);  // 현재 안쓰는 코드
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 프레임 다시 쓰기
    void Update()
    {
        
        // 들어왔는지 검사
        if (goalCheck)
        {

            return; // 들어왔다면 처리를 멈춘다
        }

        // 현재 플레이어 캐리겉가 어느 높이에 있는지 계산
        // float height = transform.position.y + jumpVy;    // 한 번 누르면 20이상 뛰어오를 수 있다
        float height = transform.position.y + jumpVy * Time.deltaTime;

        if (height <= 0.0f)
        {

            // 점프 초기화
            height = 0.0f;
            jumpVy = 0.0f;


            // 점프 확인
            if (Input.GetButtonDown("Fire1"))
            {

                // 점프 처리
                jumpVy = + 7.5f;    // 현 상황에 맞게 값 수정
                // jumpVy = +1.3f;  // 금방 땅에 닿는다

                // 점프 스프라이트 이미지로 전환
                GetComponent<SpriteRenderer>().sprite = jump[0];
            }
            else
            {

                // 달리기 처리
                animIndex++;
                if (animIndex >= run.Length)
                {

                    animIndex = 0;
                }

                // 달리기 스프라이트 이미지로 전환
                GetComponent<SpriteRenderer>().sprite = run[animIndex];
            }
        }
        else
        {

            // 점프 후 떨어지는 도중
            // jumpVy -= 0.2f;  
            jumpVy -= 6.0f * Time.deltaTime;     // 제대로 된 처리
        }

        // 플레이어 캐릭터 이동(좌표 설정)
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, height, 0.0f);
        // 아래와 같이 상대적으로 이동하도록 해도 된다
        // transform.Translate(speed* Time.deltaTime, jumpVy, 0.0f);
        // transform.position += new Vector3(speed* Time.deltaTime, jumpVy, 0.0f);
        // 단 다음과 같은 방법으로는 움직이지 않으므로 주의해야 한다
        // transform.position.Set(transform.position.x + speed * Time.deltaTime, height, 0.0f);

        // 카메라 이동 (좌표를 상대적으로 이동시킴)
        // GameObject goCam = GameObject.Find("Main Camer");    // 매 프레임마다 Find 하는건 연산 많이 잡아 먹어 위에 변수 설정
        goCam.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        // 혹은 카메라 오브젝트에 메인 카메라 태그를 등록한 뒤 클래스 변수를 이용
        // Camera.main.transform.Translate(speed * Time.deltaTime, 0.0f, 0.0f);
        
    }

    // 유니티 GUI 표시
    private void OnGUI()
    {

        // 디버그 텍스트
        GUI.TextField(new Rect(10, 10, 300, 60),
            "[Unity2D Sample 3-1 A]\n마우스 왼쪽 버튼을 누르면 가속\n놓으면 점프!");

        // 리셋 버튼
        if(GUI.Button(new Rect(10, 80, 100, 20), "리셋"))
        {

            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
