using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacterController
{

    // 외부 파라미터 (Inspector 표시)
    public float initHpMax = 20.0f;
    [Range(0.1f, 100.0f)] public float initSpeed = 12.0f;

    // 내부 파라미터
    int jumpCount = 0;

    volatile bool atkInputEnabled = false;              // 멀티스레드 프로그램에서 컴파일러로 변수 대입 처리를 생략하는 최적화를 하지 않는다는 것
    volatile bool atkInputNow = false;

    bool breakEnabled = true;
    float groundFriction = 0.0f;

    // 외부 파라미터
    public readonly static int ANISTS_Idle =
        Animator.StringToHash("Base Layer.Player_Idle");
    public readonly static int ANISTS_Walk =
        Animator.StringToHash("Base Layer.Player_Walk");
    public readonly static int ANISTS_Run =
        Animator.StringToHash("Base Layer.Player_Run");
    public readonly static int ANISTS_Jump =
        Animator.StringToHash("Base Layer.Player_Jump");
    public readonly static int ANISTS_ATTACK_A =
        Animator.StringToHash("Base Layer.Player_ATK_A");
    public readonly static int ANISTS_ATTACK_B =
        Animator.StringToHash("Base Layer.Player_ATK_B");
    public readonly static int ANISTS_ATTACK_C =
        Animator.StringToHash("Base Layer.Player_ATK_C");
    public readonly static int ANISTS_ATTACKJUMP_A =
        Animator.StringToHash("Base Layer.Player_ATKJUMP_A");
    public readonly static int ANISTS_ATTACKJUMP_B =
        Animator.StringToHash("Base Layer.Player_ATKJUMP_B");

    // 코드 (MonoBehaviour 기본 기능 구현)
    protected override void Awake()
    {
        base.Awake();

        // 파라미터 초기화
        speed = initSpeed;
        SetHp(initHpMax, initHpMax);
    }

    protected override void FixedUpdateCharacter()
    {

        // 현재 스테이트 가져오기
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // 애니메이터에서 맨 위에 있는 Default Layer의 정보 가져온다

        // 착지 검사
        if (jumped)     // 점프 중인 경우
        {

            // 앞 번에 지면에 닿지 않고 현재 지면에 닿은 경우 점프 상태 해제
            // 현재 지면에 있고 점프한지 1초가 지났다면 강제로 접지로 판정
            if ((grounded && !groundedPrev) || (grounded && Time.fixedTime > jumpStartTime +1.0f))
            {

                animator.SetTrigger("Idle");    // 탈출 애니메이션
                jumped = false;
                jumpCount = 0;
                rigidbody2D.gravityScale = gravityScale;
            }
            if (Time.fixedTime > jumpStartTime + 1.0f)
            {

                if (stateInfo.fullPathHash == ANISTS_Idle ||
                    stateInfo.fullPathHash == ANISTS_Walk ||
                    stateInfo.fullPathHash == ANISTS_Run ||
                    stateInfo.fullPathHash == ANISTS_Jump)
                {

                    rigidbody2D.gravityScale = gravityScale;
                }
            }
        }
        // if (!jumped)
        else            // 이외는 지면에 있으므로 항상 0
        {

            jumpCount = 0;
            rigidbody2D.gravityScale = gravityScale;
        }
        
        // 공격 중인지 확인   
        // nameHash는 더 이상 사용안되고 fullPathHash 이용하라고 한다
        if(stateInfo.fullPathHash == ANISTS_ATTACK_A ||     
            stateInfo.fullPathHash == ANISTS_ATTACK_B ||
            stateInfo.fullPathHash == ANISTS_ATTACK_C ||
            stateInfo.fullPathHash == ANISTS_ATTACKJUMP_A ||
            stateInfo.fullPathHash == ANISTS_ATTACKJUMP_B)
        {

            // 이동 정지
            speedVx = 0;
        }

        // 캐릭터 방향
        transform.localScale = new Vector3(     // 크기 설정으로 캐릭터 바라보는 방향 설정
            basScaleX * dir, transform.localScale.y, transform.localScale.z);

        // 점프 도중에 가로 이동 감속
        if (jumped && !grounded && groundCheck_OnMoveObject == null)         // 점프 시작 시
        {

            if (breakEnabled)
            {

                breakEnabled = false;   
                speedVx *= 0.9f;        // 현재 속도의 10% 감소
            }
        }

        // 이동 정지(감속) 처리
        if (breakEnabled)               // 현재는 방향 전환이나 키입력이 없을 때
        {

            speedVx *= groundFriction;  // 바로 멈춤
                                        // velocity로 이동 했으므로 직접 물리 연산을 구현해야한다
                                        // 빙판길을 표현하려면 0.5 같이 1 이하 양수 값으로 하면된다
        }

        // 카메라
        Camera.main.transform.position = transform.position - Vector3.forward;  // 메인 카메라 태그로 등록된 카메라의 위치는 캐릭터에 -1 좌표 위치
    }

    // 코드 (애니메이션 이벤트용 코드)
    public void EnableAttackInput()
    {

        atkInputEnabled = true;
    }

    public void SetNextAttack(string name)
    {

        if (atkInputNow == true)
        {

            atkInputNow = false;
            animator.Play(name);
        }

        
        // 추가한 코드
        atkInputEnabled = false;    // 앞번 공격에서 팔로우 스루 때 공격 버튼을 누르면
                                    // 다음 공격 시 2단 공격이 나가서 이를 방지하고자 추가
    }

    // 코드 (기본 액션)
    public override void ActionMove(float n)
    {

        if (!activeSts)
        {

            return;
        }

        // 초기화
        float dirOld = dir;
        breakEnabled = false;

        // 애니메이션 지정
        float moveSpeed = Mathf.Clamp(Mathf.Abs(n), -1.0f, 1.0f);   // -1, 1 사이의 값 조절
        animator.SetFloat("MoveSpeed", moveSpeed);  // movespeed 수치 값 조절로 달리기와 걷기 표현
        // animator.speed = 1.0f + moveSpeed;       // 빠른 걷는 모션을 통해 뛰는걸 표현?

        // 이동 검사
        if (n != 0.0f)
        {

            // 이동
            dir = Mathf.Sign(n);
            moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;    // 걸을 땐 속도의 최대 25%만, 달릴 때는 100% 적용
            speedVx = initSpeed * moveSpeed * dir;
        }
        else
        {

            // 이동 정지
            breakEnabled = true;
        }

        // 그 시점에서 돌아보기 검사
        if(dirOld != dir)
        {

            // 방향 전환 확인
            breakEnabled = true;
        }
    }

    public void ActionJump()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == ANISTS_Idle ||
            stateInfo.fullPathHash == ANISTS_Walk ||
            stateInfo.fullPathHash == ANISTS_Run ||
            (stateInfo.fullPathHash == ANISTS_Jump &&
            rigidbody2D.gravityScale >= gravityScale))
        {
            switch (jumpCount)
            {

                case 0:     // 첫 점프인 경우
                    if (grounded)
                    {

                        animator.SetTrigger("Jump");                // 애니메이션 실행
                        rigidbody2D.velocity = Vector2.up * 30.0f;  // 위로 30 속도 이동
                        jumpStartTime = Time.fixedTime;             // 점프 시작 시간 저장
                        jumped = true;                              // 점프 상태 
                        jumpCount++;                                // 현재 몇단 점프인지 확인
                    }
                    break;

                case 1:     // 이단 점프 하는지 체크
                    if (!grounded)
                    {

                        animator.Play("Player_Jump", 0, 0.0f);       // 점프 애니메이션 시작
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20.0f);  // 위로 20만큼만
                        jumped = true;                              // 점프 상태 true
                        jumpCount++;                                // 점프 카운트 업
                    }
                    break;
            }
        }
    }

    public void ActionAttack()
    {

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.fullPathHash == ANISTS_Idle ||
            stateInfo.fullPathHash == ANISTS_Walk ||
            stateInfo.fullPathHash == ANISTS_Run ||
            stateInfo.fullPathHash == ANISTS_Jump ||
            stateInfo.fullPathHash == ANISTS_ATTACK_C)
        {

            animator.SetTrigger("Attack_A");
            if (stateInfo.fullPathHash == ANISTS_Jump ||
                stateInfo.fullPathHash == ANISTS_ATTACK_C)
            {

                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.gravityScale = 0.1f;
            }
        }
        else
        {

            if (atkInputEnabled)
            {

                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
    }

    public void ActionAttackJump()
    {

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (grounded &&
            (stateInfo.fullPathHash == ANISTS_Idle ||
            stateInfo.fullPathHash == ANISTS_Walk ||
            stateInfo.fullPathHash == ANISTS_Run ||
            stateInfo.fullPathHash == ANISTS_ATTACK_A ||
            stateInfo.fullPathHash == ANISTS_ATTACK_B))
        {

            animator.SetTrigger("Attack_C");
            jumpCount = 2;
        }
        else
        {

            if (atkInputEnabled)
            {

                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
    }
}
