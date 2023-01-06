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


    private bool groundBool;        // ���鿡 ��Ҵ°�?
    private bool runBool;           // �޸��� �����ΰ�?
    private bool moveBool;          // �̵� ���ΰ�?
    private bool staminaBool;       // ���׹̳� ȸ�� ������ �����ΰ�?
    private bool chkBool;           // ��ȭ ������ ����

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
    /// �ʱ�ȭ �޼ҵ�
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Reset(object sender, EventArgs e)
    {
        
        Init();
    }

    /// <summary>
    /// ������ ������Ʈ �޼ҵ�
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
    /// �ʱ�ȭ �޼ҵ�
    /// </summary>
    protected override void Init()
    {
       
        base.Init();

        transform.position = Vector3.zero;

        animator.Reset();

        nowStamina = maxStamina;
        applySpd = status.MoveSpd;

        hammerObj.SetActive(true);

        // StatsUI �ʱ�ȭ
        StatsUI.instance.SetHp(nowHp);
        StatsUI.instance.SetStamina(nowStamina);
        StatsUI.instance.SetAtk(hidden.ChkAtk(status.Atk));
    }

    /// <summary>
    /// �ൿ�� ���¸� üũ�Ѵ�
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
    /// ���� üũ
    /// </summary>
    private void ChkGround()
    {

        if (hidden.ChkAbility(Hidden.Ability.HealthMan)) return;

        chkBool = groundBool;
        groundBool = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }

    /// <summary>
    /// ���� ������ üũ
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
    /// �޸��� üũ
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
    /// �̵� Ȯ��
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
    /// ���� Ȯ��
    /// </summary>
    private void ChkJump()
    {

        if (groundBool && Input.GetKeyDown(KeyCode.Space))
        {

            myRd.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// ī�޶� Ȯ��
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
    /// ���� Ȯ��
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
    /// ���׹̳� ä���� Ȯ��
    /// </summary>
    private void ChkStamina()
    {

        chkBool = false;

        // ���׹̳� ä�� ����
        if (moveBool || !groundBool)
        {
           
            staminaBool = false;
        }
        else
        {
            
            staminaBool = true;
        }


        // ���׹̳� ä�� ���� �ֱ�
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
    /// ���ݿ��� ������ �޼ҵ�
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
    /// ���� �ð����� �ݶ��̴� Ȱ��ȭ
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
    /// ���� ��ƼŬ ����
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
    /// �ǰ� �޼ҵ�
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