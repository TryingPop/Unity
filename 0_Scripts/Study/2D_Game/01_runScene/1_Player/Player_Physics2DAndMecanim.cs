using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Physics2DAndMecanim : MonoBehaviour
{

    // 선언
    // Inspector에서 조정하기 위한 속성
    public float speed = 12.0f;             // 플레이어 캐릭터의 속도
    public float jumpPower = 1600.0f;       // 플레이어 캐릭터를 점프시켰을 때의 힘

    // 내부에서 다루는 변수
    bool grounded;                          // 접지 확인
    bool goalCheck;                         // 들어왔는지 확인
    float goalTime;                         // 들어온 시간

    Rigidbody2D rigidbody2D;
    Transform groundCheck;
    Animator anim;
    GameObject goCam;

    // 메시지에 대응한 코드
    // 컴포넌트 실행 시작
    void Start()
    {

        // 초기화
        grounded = false;
        goalCheck = false;

        rigidbody2D= GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        goCam = GameObject.Find("Main Camera");
        anim = GetComponent<Animator>();
    }

    // 플레이어 캐릭터에 적용된 충돌 판정 영역에 다른 게임 오브젝트의 충돌 판정 영역이 겹쳤다
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // 들어왔는지 확인
        if(collision.gameObject.name == "Stage_Gate")
        {

            // 들어왔다
            goalCheck = true;
            goalTime = Time.timeSinceLevelLoad;
            
            anim.enabled = false;       // 애니메이션 컴포넌트 비활성화해서 애니메이션 종료
        }
    }

    // 프레임 다시 쓰기
    void Update()
    {

        // 지면에 닿았는지 확인
        // Transform groundCheck = transform.Find("GroundCheck");
        grounded = (Physics2D.OverlapPoint(groundCheck.position) != null) ? true : false;

        if (grounded)
        {

            // 점프 버튼 확인
            if (Input.GetButtonDown("Fire1"))
            {

                // 점프 처리
                rigidbody2D.AddForce(new Vector2(0.0f, jumpPower));
            }

            // 달리기 애니메이션 설정
            // GetComponent<Animator>().SetTrigger("Run");
            anim.SetTrigger("Run");
        }
        else
        {

            // GetComponent<Animator>().SetTrigger("Jump");
            anim.SetTrigger("Jump");
        }

        // 구멍에 빠졌는가?
        if (transform.position.y < -10.0f)
        {

            // 구멍에 빠졌다면 스테이지를 다시 읽어들여서 초기화한다
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
        goCam.transform.position = new Vector3(transform.position.x + 5.0f,
            goCam.transform.position.y, goCam.transform.position.z);
    }

    // 유니티 GUI 표시
    private void OnGUI()
    {

        // 디버그 텍스트
        GUI.TextField(new Rect(10, 10, 300, 60),
            "[Unity2D Sample 3-1 C]\n마우스 왼쪽 버튼을 누르면 점프!");

        if (goalCheck)
        {

            GUI.TextField(new Rect(10, 100, 330, 60),
                string.Format("***** Goal!! *****\nTime {0}", goalTime));
        }

        // 초기화하는 리셋 버튼
        if (GUI.Button(new Rect(10, 80, 100, 20), "리셋"))
        {

            // Application.LoadLevel(Application.loadedLevelName);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /*
        // 메뉴로 돌아간다
        if (GUI.Button(new Rect(10, 110, 100, 20), "메뉴"))
        {

            // Application.LoadLevel("SelectMenu");
            SceneManager.LoadScene("SelectMenu");
        }
        */
    }
}
