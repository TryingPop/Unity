using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Stats
{
    
    [SerializeField] private Hidden             hidden;
    [SerializeField] private PlayerAnimator     animator;

    [SerializeField] private AudioClip          atkSnd;
    [SerializeField] private GameObject         hammerObj;

    [SerializeField] private Transform          chrTrans;
    [SerializeField] private Transform          cameraBoxTrans;

    [SerializeField] private float runSpd;
    [SerializeField] private float jumpForce;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float maxStamina;

    [SerializeField] private float minFallPow;
    [SerializeField] private float fallDmgRatio;

    public PlayerColor playerColor;
    
    private CapsuleCollider myCollider;


    private bool groundBool;        // 지면에 닿았는가?
    private bool runBool;           // 달리는 상태인가?
    private bool moveBool;          // 이동 중인가?
    private bool staminaBool;       // 스테미나 회복 가능한 상태인가?
    private bool chkBool;           // 변화 감지용 변수

    private float nowStamina;
    private float applySpd;


    private void Awake()
    {
        GetComp();
        
        Init();

        hidden.SetAbility(0);

        myWC.Attack += Attack;
    }

    private void Update()
    {

        Action();
    }

    /// <summary>
    /// 초기화 메소드
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Reset(object sender, EventArgs e)
    {
        
        Init();
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
    protected override void Init()
    {
       
        base.Init();

        transform.position = Vector3.zero;

        animator.Reset();

        nowStamina = maxStamina;
        applySpd = status.MoveSpd;

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

        if (hidden.ChkAbility(Hidden.Ability.HealthMan)) return;

        chkBool = groundBool;
        groundBool = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }

    /// <summary>
    /// 낙하 데미지 체크
    /// </summary>
    private void ChkFallDmg()
    {
        if (hidden.ChkAbility(Hidden.Ability.HealthMan)) return;

        if (chkBool != groundBool)
        {

            if (groundBool)
            {
                int _velocity = - (int)myRd.velocity.y;

                if (_velocity >= minFallPow)
                {

                    Damaged((int)(_velocity * fallDmgRatio));
                }
            }
        }
    }

    /// <summary>
    /// 달리기 체크
    /// </summary>
    private void ChkRun()
    {
        chkBool = runBool;

        if (Input.GetKey(KeyCode.LeftShift) && nowStamina > 0)
        {

            runBool = true;

            if (hidden.ability == Hidden.Ability.HealthMan)
            {

                applySpd = runSpd * 2f;
            }
            else
            {

                applySpd = runSpd;
            }
            chkBool = true;
        }
        else
        {

            runBool = false;
            applySpd = status.MoveSpd;
        }

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
        chkBool = moveBool;
        
        Vector2 _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveBool = (_moveInput != Vector2.zero);

        if (moveBool)
        {

            Vector3 _lookForward = SetDirXZ(cameraBoxTrans.forward);
            Vector3 _lookRight = SetDirXZ(cameraBoxTrans.right);

            moveDir = (_lookForward * _moveInput.y + _lookRight * _moveInput.x).normalized;

            chrTrans.forward = moveDir;

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

        if (groundBool && Input.GetKeyDown(KeyCode.Space))
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
    }

    /// <summary>
    /// 스테미나 채울지 확인
    /// </summary>
    private void ChkStamina()
    {

        chkBool = false;

        // 스테미나 채울 조건
        if (moveBool || !groundBool)
        {
           
            staminaBool = false;
        }
        else
        {
            
            staminaBool = true;
        }


        // 스테미나 채울 조건 넣기
        if (moveBool && runBool && !staminaBool)
        {
            
            nowStamina -= Time.deltaTime;

            if (nowStamina < 0)
            {

                nowStamina = 0;
            }

            chkBool = true;
        }

        else if (staminaBool)
        {

            if (nowStamina < maxStamina)
            {

                nowStamina += Time.deltaTime;

                if (nowStamina > maxStamina)
                {

                    nowStamina = maxStamina;
                }

                chkBool = true;
            }
        }

        if (chkBool)
        {

            StatsUI.instance.SetStamina(nowStamina);
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
        targetStatus?.Damaged(hidden.ChkAtk(status.Atk));
        hidden.ChkKnockBack(targetStatus.myRd);
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
            targetStatus.transform.position + targetStatus.transform.up, Quaternion.identity);
    }

    protected virtual void SetHammerSnd()
    {

        myAS.SetSnd(atkSnd);
        myAS.GetSnd(true);
    }

    /// <summary>
    /// 피격 메소드
    /// </summary>
    /// <param name="atk"></param>
    public override void Damaged(int atk)
    {

        base.Damaged(atk);

        animator.SetDmg();

        StatsUI.instance.SetHp(nowHp);

        if (!hidden.ChkAbility(Hidden.Ability.Immortality))
        {

            ChkDead();
        }
    }

    public void SetUIAtk()
    {

        StatsUI.instance.SetAtk(hidden.ChkAtk(status.Atk));
    }
}