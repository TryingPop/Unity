using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 20f;
    public float jumpPower = 15;

    private float hAxis;    // 캐릭터 이동 좌우 방향 키
    private float vAxis;    // 캐릭터 이동 상하 방향 키

    private bool wDown;     // 캐릭터 걷기 좌측 쉬프트 키 입력
    private bool jDown;     // 캐릭터 점프 스페이스바 키

    private bool isJump;    // 현재 점프 상태 체크
    private bool isDodge;   // 현재 회피 상태 체크

    private Vector3 moveVec;    // 이동용 벡터3
    private Vector3 dodgeVec;   // 닷지용 벡터3
    private Animator anim;
    public Rigidbody rigid;


    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    private void GetInput()
    {

        // GetAxis는 부드럽게 받아오는 반면,   -1f ~ 1f
        // GetAxisRaw는 즉시 받아온다          -1, 0, 1로 받아온다
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // ProjectSettings의 InputManager에서 Walk를 추가해줘야한다
        wDown = Input.GetButton("Walk");        // 여기서는 left shift
        jDown = Input.GetButtonDown("Jump");    // 여기서는 스페이스 바

    }

    private void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;      // 크기 1로 맞춘다

        if (isDodge)
        {

            moveVec = dodgeVec;
        }
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    private void Turn()
    {

        // 해당 좌표로 바라본다
        // 그래서 현재 위치 + moveVec을 해준다
        // 이동 전이나 이동 후나 코드는 똑같다
        // 만약 아래와 같이변수를 할당할 경우
        // Vector3 destination = transform.position + moveVec
        // LookAt은 이동 전으로 가야한다
        transform.LookAt(transform.position + moveVec);
    }

    private void Jump()
    {

        if (jDown && !isJump && moveVec == Vector3.zero && !isDodge)
        {

            // 순간적인 힘을 주는 모드인 Impulse
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    private void Dodge()
    {

        if (jDown && !isJump && moveVec != Vector3.zero && !isDodge)
        {

            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            // 해당 이름의 메소드를 0.4초 뒤에 실행
            Invoke("DodgeOut", 0.4f);
        }
    }

    private void DodgeOut()
    {

        isDodge = false;
        speed *= 0.5f;
    }

    // 착지 판정
    private void OnCollisionEnter(Collision collision)
    {
        
        // 태그로 지면 판정
        // Floor 태그 생성 및 지면에 Floor 태그 추가
        if (collision.gameObject.tag == "Floor")
        {

            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
