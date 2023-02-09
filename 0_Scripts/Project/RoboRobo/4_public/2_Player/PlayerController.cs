using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Unit
{

    [SerializeField] private PlayerAnimator animator;           // 캐릭터 애니메이터를 관리하는 스크립트
    [SerializeField] private Hidden hidden;             // 히든 능력을 보유한 스크립트

    [SerializeField] private AudioClip atkSnd;             // 공격 사운드
    [SerializeField] private GameObject hammerObj;          // 해머 오브젝트

    [SerializeField] private Transform chrTrans;           // 캐릭터 좌표
    [SerializeField] private Transform cameraBoxTrans;     // 카메라 좌표

    [SerializeField] private float runSpd;                          // 이동 속도
    [SerializeField] private float jumpForce;                       // 점프 파워
    [SerializeField] private float lookSensitivity;                 // 카메라 민감도
    [SerializeField] private float maxStamina;                      // 최대 스테미너

    [SerializeField] private float minFallPow;                      // 낙하 데미지 주는 최소 힘
    [SerializeField] private float fallDmgRatio;                    // 낙하 데미지 비율

    public PlayerColor playerColor;                                 // 플레이어 색상

    private CapsuleCollider myCollider;                             // 지면 체크를 위한 콜라이더

    private bool groundBool;        // 지면에 닿았는가?
    private bool runBool;           // 달리는 상태인가?
    private bool moveBool;          // 이동 중인가?
    private bool staminaBool;       // 스테미나 회복 가능한 상태인가?
    private bool chkBool;           // 변화 감지용 변수

    private bool ladderBool;        // 사다리에 있는지?

    private float nowStamina;       // 현재 스테미나
    private float applySpd;         // 적용 속도


    private void Awake()
    {

        GetComp();

        hidden.SetAbility(0);

        myWC.Attack += Attack;
    }

    private void Start()
    {

        Init();
    }

    private void Update()
    {

        Action();
    }



    /// <summary>
    /// 가져올 컴포넌트 메소드
    /// </summary>
    protected override void GetComp()
    {

        base.GetComp();

        playerColor = GetComponent<PlayerColor>();
        myCollider = GetComponent<CapsuleCollider>();
        hidden = GetComponent<Hidden>();
        animator = GetComponent<PlayerAnimator>();
    }

    /// <summary>
    /// 초기화 메소드
    /// </summary>
    public override void Init()
    {

        // 시작지점과 방향 초기화
        transform.position = Vector3.zero;
        transform.forward = Vector3.forward;
        chrTrans.forward = Vector3.forward;
        cameraBoxTrans.forward = Vector3.forward;

        // 각종 수치 초기화
        base.Init();
        nowStamina = maxStamina;
        applySpd = status.MoveSpd;

        // 애니메이터 새로
        animator.Reset();

        // 사망하거나 승리하면 hammerObj 비활성화되므로 활성화
        hammerObj.SetActive(true);

        // StatsUI 초기화
        StatsUI.instance.SetHp(nowHp);
        StatsUI.instance.SetStamina(nowStamina);
        StatsUI.instance.SetAtk(hidden.ChkAtk(status.Atk));
    }

    /// <summary>
    /// 행동과 상태를 체크한다
    /// </summary>
    private void Action()
    {

        if (deadBool) return;

        ChkGround();

        ChkFallDmg();

        ChkRun();

        ChkMove();

        ChkJump();

        ChkCamera();

        ChkAtk();

        ChkStamina();
    }

    /// <summary>
    /// 지면 체크
    /// </summary>
    private void ChkGround()
    {

        // healthMan인 경우 무한 점프 가능하게 설정
        if (hidden.ChkAbility(Hidden.Ability.HealthMan)) return;

        // 낙하 데미지 용도로 쓰인다
        chkBool = groundBool;

        // 아래로 레이저를 쏴서 지면에 닿았는지 확인
        groundBool = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }

    /// <summary>
    /// 낙하 데미지 체크
    /// </summary>
    private void ChkFallDmg()
    {

        // healthMan인 경우 낙하 데미지 없음
        if (hidden.ChkAbility(Hidden.Ability.HealthMan)) return;

        // 점프 후 지면에 닿는 경우
        if (chkBool != groundBool && groundBool)
        {

            // 낙하 데미지
            ChkFallDamaged();
        }
    }

    /// <summary>
    /// 달리기 체크
    /// </summary>
    private void ChkRun()
    {

        // 걷다가 달리는지 변화 하는 상태 체크
        chkBool = runBool;

        // 달리기 키를 누르고 현재 스테미나가 0이상일 때
        if (Input.GetKey(KeyCode.LeftShift) && nowStamina > 0)
        {

            // 달리는 상태
            runBool = true;

            applySpd = runSpd;

            // HealthMan이면 이속 2배
            if (hidden.ChkAbility(Hidden.Ability.HealthMan))
            {

                applySpd *= 2f;
            }
        }
        // 달릴 수 없는 경우
        else
        {

            runBool = false;
            applySpd = status.MoveSpd;
        }

        // 상태 변화가 일어났으므로 animator 속도 변경
        if (chkBool != runBool)
        {

            animator.SetRun(runBool);
        }
    }

    /// <summary>
    /// 이동 확인
    /// </summary>
    private void ChkMove()
    {

        // 애니메이션 무브를 실행 해야하는가 용도로 쓰임
        chkBool = moveBool;

        // 방향 벡터
        Vector2 _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveBool = (_moveInput != Vector2.zero);

        if (moveBool)
        {

            Vector3 _lookForward = SetDirXZ(cameraBoxTrans.forward);
            Vector3 _lookRight = SetDirXZ(cameraBoxTrans.right);

            moveDir = (_lookForward * _moveInput.y + _lookRight * _moveInput.x).normalized;

            if (moveDir != Vector3.zero) chrTrans.forward = moveDir;

            Move(applySpd * hidden.ChkTime());
        }

        if (chkBool != moveBool)
        {

            animator.SetMove(moveBool);
        }

    }

    /// <summary>
    /// 점프 확인
    /// </summary>
    private void ChkJump()
    {

        // 지면에 닿았고 점프 키를 누른 경우
        if ((groundBool || ladderBool) && Input.GetKeyDown(KeyCode.Space))
        {

            myRd.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 카메라 확인
    /// </summary>
    private void ChkCamera()
    {

        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * lookSensitivity;
        Vector3 camAngle = cameraBoxTrans.rotation.eulerAngles;

        mouseDelta *= Time.deltaTime * 100f;

        float x = camAngle.x - mouseDelta.x;

        if (x < 180f)
        {

            x = Mathf.Clamp(x, -1f, 90f);
        }
        else
        {

            x = Mathf.Clamp(x, 330f, 361f);
        }

        cameraBoxTrans.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.y, camAngle.z);
    }

    /// <summary>
    /// 공격 확인
    /// </summary>
    private void ChkAtk()
    {

        // 마우스 왼쪽을 누르고 공격이 가능한 경우 공격
        if (Input.GetMouseButton(0) && !atkBool)
        {

            SetHammerSnd();

            if (hidden.ChkAbility(Hidden.Ability.ContinuousAttacker))
            {

                myWC.AtkColActive(true);
                animator.SetAtk();
            }
            else
            {

                StartCoroutine(Attack());
            }
        }
        else if (hidden.ChkAbility(Hidden.Ability.ContinuousAttacker))
        {

            myWC.AtkColActive(false);
        }
    }

    /// <summary>
    /// 스테미나 채울지 확인
    /// </summary>
    private void ChkStamina()
    {

        // 스테미나 변경 있는가 없는가 용도로 쓰인다
        chkBool = true;

        // 스테미나 채울 조건
        // 이동 중이거나 점프 중이면 채울 수 없고 이외는 채울 수 있다
        if (moveBool || !groundBool)
        {

            staminaBool = false;
        }
        else
        {

            staminaBool = true;
        }

        // 달리면서 이동 중일 때만 스테미나를 깎는다
        if (moveBool && runBool)
        {

            nowStamina -= Time.deltaTime;

            if (nowStamina < 0)
            {

                nowStamina = 0;
            }
        }
        // 스테미나 채우는 부분
        else if (staminaBool)
        {

            if (nowStamina < maxStamina)
            {

                nowStamina += Time.deltaTime;

                if (nowStamina > maxStamina)
                {

                    nowStamina = maxStamina;
                }
            }
        }
        // 스테미나 변동이 없다
        else
        {

            chkBool = false;
        }


        if (chkBool)
        {

            StatsUI.instance.SetStamina(nowStamina);
        }
    }

    /// <summary>
    /// 낙하 데미지
    /// </summary>
    private void ChkFallDamaged()
    {
        int _velocity = -(int)myRd.velocity.y;

        if (_velocity >= minFallPow)
        {

            OnDamaged((int)(_velocity * fallDmgRatio));
        }
    }

    /// <summary>
    /// 공격에서 실행할 메소드
    /// </summary>
    /// <param name="other">target Collider</param>
    protected override void Attack(object sender, Collider other)
    {

        SetTargetStats(other.gameObject);
        SetAtkParticle();
        hidden.ChkAtkSnd();
        targetStats?.OnDamaged(hidden.ChkAtk(status.Atk));
        hidden.ChkKnockBack(targetStats.myRd);
        if (!hidden.ChkAbility(Hidden.Ability.boomAttacker))
        {

            base.Attack(sender, other);
        }
    }

    /// <summary>
    /// 일정 시간동안 콜라이더 활성화
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Attack()
    {

        atkBool = true;
        myWC.AtkColActive(true);

        animator.SetAtk();

        yield return atkWaitTime;

        atkBool = false;
        myWC.AtkColActive(false);
    }

    /// <summary>
    /// 공격 파티클 생성
    /// </summary>
    protected override void SetAtkParticle()
    {

        Instantiate(hidden.ChkParticle(atkParticle),
            targetStats.transform.position + targetStats.transform.up, Quaternion.identity);
    }

    protected virtual void SetHammerSnd()
    {

        myAS.SetSnd(atkSnd);
        myAS.GetSnd(true);
    }

    /// <summary>
    /// 피격 메소드
    /// </summary>
    /// <param name="atk">공격한 대상의 공격력</param>
    public override void OnDamaged(int atk)
    {

        base.OnDamaged(atk);

        animator.SetDmg();

        StatsUI.instance.SetHp(nowHp);

        if (!hidden.ChkAbility(Hidden.Ability.Immortality) 
            && GameManager.instance.state == GameManager.GAMESTATE.Play)
        {

            ChkDead();
        }
    }

    protected override void Dead()
    {

        base.Dead();

        hammerObj.SetActive(false);
        animator.SetDead();
        GameManager.instance.GameOver(false);
    }

    public void SetUIAtk()
    {

        StatsUI.instance.SetAtk(hidden.ChkAtk(status.Atk));
    }

    public void ChkLadder(bool chkBool)
    {
        ladderBool = chkBool;
    }
}