using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Physics2D : MonoBehaviour
{

    // 선언
    // Inspector에서 조정하기 위한 속성
    public float speed = 12.0f;         // 플레이어 캐릭터의 속도
    public float jumpPower = 500.0f;    // 플레이어 캐릭터의 점프시켰을 때의 파워
    public Sprite[] run;                // 플레이어 캐릭터의 달리기 스프라이트
    public Sprite[] jump;               // 플레이어 캐릭터의 점프 스프라이트

    // 내부에서 다루는 변수
    int animIndex;                      // 플레이어 캐릭터 애니메이션 재생 인덱스
    bool grounded;                      // 접지 체크
    bool goalCheck;                     // 골인했는지 체크
    float goalTime;                     // 골인 타임

    // 추가한 함수
    Rigidbody2D rigidbody2D;
    SpriteRenderer image;
    GameObject goCam;

    // 메시지에 대응한 코드

    // 컴포넌트 실행 시작
    void Start()
    {

        // 초기화
        animIndex = 0;
        grounded = false;
        goalCheck = false;

        // 
        rigidbody2D = GetComponent<Rigidbody2D>();
        image = GetComponent<SpriteRenderer>();
        goCam = Camera.main.gameObject;         // Main Camera 태그로 등록된 오브젝트를 가져온다
    }

    // 플레이어 캐릭터에 적용된 충돌 판정 영역에 다른 게임 오브젝트의 충돌 판정 영역이 겹쳤을 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        // 들어왔는지 확인
        if (collision.gameObject.name == "Stage_Gate")
        {

            // 들어왔다
            goalCheck = true;
            goalTime = Time.timeSinceLevelLoad;
        }
    }

    // 프레임 다시 쓰기
    void Update()
    {

        // 들어왔는지 확인
        Transform groundCheck = transform.Find("GroundCheck");
        grounded = (Physics2D.OverlapPoint(groundCheck.position) != null) ? true : false;
        if (grounded)
        {

            // 점프
            if (Input.GetButtonDown("Fire1"))
            {

                // 점프 처리
                rigidbody2D.AddForce(new Vector2(0.0f, jumpPower));

                // 점프 스프라이트 이미지로 변환
                // GetComponent<SpriteRenderer>().sprite = jump[0];
                image.sprite = jump[0];
            }
            else
            {

                // 달리기 처리
                animIndex++;

                if (animIndex >= run.Length)
                {

                    animIndex = 0;
                }
            }

            // 달리기 스프라이트 이미지로 전환
            // GetComponent<SpriteRenderer>().sprite = run[animIndex];
            image.sprite = run[animIndex];
        }

        // 구멍에 빠졌는가?
        if (transform.position.y < -10.0f)
        {

            // 구멍에 빠졌다면 스테이지를 다시 읽어 초기화한다
            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // 프레임 다시 쓰기
    private void FixedUpdate()
    {

        // 이동 계산
        rigidbody2D.velocity = new Vector2(speed, rigidbody2D.velocity.y);

        // 카메라 이동
        // GameObject goCam = GameObject.Find("Main Camera");
        goCam.transform.position = new Vector3(
            transform.position.x + 5.0f, goCam.transform.position.y, goCam.transform.position.z);
    }

    // 유니티 GUI 표시
    private void OnGUI()
    {

        // 디버그 텍스트
        GUI.TextField(new Rect(10, 10, 300, 60),
            "[Unity2D Sample 3-1 A]\n마우스 왼쪽 버튼을 누르면 가속\n놓으면 점프!]");

        if (goalCheck)
        {

            GUI.TextField(new Rect(10, 100, 330, 60),
                string.Format("***** Goal!! *****\nTime {0}", goalTime));
        }

        // 리셋 버튼
        if(GUI.Button(new Rect(10, 80, 100, 20), "리셋"))
        {

            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // 메뉴로 돌아간다
        if(GUI.Button(new Rect(10, 110, 100, 20), "메뉴"))
        {

            // Application.LoadLevel("SelectMenu");
            SceneManager.LoadScene("SelectMenu");
        }
    }
}

