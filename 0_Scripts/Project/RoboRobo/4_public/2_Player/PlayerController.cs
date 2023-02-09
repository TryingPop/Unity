using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Unit
{

    [SerializeField] private PlayerAnimator animator;           // ĳ���� �ִϸ����͸� �����ϴ� ��ũ��Ʈ
    [SerializeField] private Hidden hidden;             // ���� �ɷ��� ������ ��ũ��Ʈ

    [SerializeField] private AudioClip atkSnd;             // ���� ����
    [SerializeField] private GameObject hammerObj;          // �ظ� ������Ʈ

    [SerializeField] private Transform chrTrans;           // ĳ���� ��ǥ
    [SerializeField] private Transform cameraBoxTrans;     // ī�޶� ��ǥ

    [SerializeField] private float runSpd;                          // �̵� �ӵ�
    [SerializeField] private float jumpForce;                       // ���� �Ŀ�
    [SerializeField] private float lookSensitivity;                 // ī�޶� �ΰ���
    [SerializeField] private float maxStamina;                      // �ִ� ���׹̳�

    [SerializeField] private float minFallPow;                      // ���� ������ �ִ� �ּ� ��
    [SerializeField] private float fallDmgRatio;                    // ���� ������ ����

    public PlayerColor playerColor;                                 // �÷��̾� ����

    private CapsuleCollider myCollider;                             // ���� üũ�� ���� �ݶ��̴�

    private bool groundBool;        // ���鿡 ��Ҵ°�?
    private bool runBool;           // �޸��� �����ΰ�?
    private bool moveBool;          // �̵� ���ΰ�?
    private bool staminaBool;       // ���׹̳� ȸ�� ������ �����ΰ�?
    private bool chkBool;           // ��ȭ ������ ����

    private bool ladderBool;        // ��ٸ��� �ִ���?

    private float nowStamina;       // ���� ���׹̳�
    private float applySpd;         // ���� �ӵ�


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
    public override void Init()
    {

        // ���������� ���� �ʱ�ȭ
        transform.position = Vector3.zero;
        transform.forward = Vector3.forward;
        chrTrans.forward = Vector3.forward;
        cameraBoxTrans.forward = Vector3.forward;

        // ���� ��ġ �ʱ�ȭ
        base.Init();
        nowStamina = maxStamina;
        applySpd = status.MoveSpd;

        // �ִϸ����� ����
        animator.Reset();

        // ����ϰų� �¸��ϸ� hammerObj ��Ȱ��ȭ�ǹǷ� Ȱ��ȭ
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

        // healthMan�� ��� ���� ���� �����ϰ� ����
        if (hidden.ChkAbility(Hidden.Ability.HealthMan)) return;

        // ���� ������ �뵵�� ���δ�
        chkBool = groundBool;

        // �Ʒ��� �������� ���� ���鿡 ��Ҵ��� Ȯ��
        groundBool = Physics.Raycast(transform.position, Vector3.down, myCollider.bounds.extents.y + 0.1f);
    }

    /// <summary>
    /// ���� ������ üũ
    /// </summary>
    private void ChkFallDmg()
    {

        // healthMan�� ��� ���� ������ ����
        if (hidden.ChkAbility(Hidden.Ability.HealthMan)) return;

        // ���� �� ���鿡 ��� ���
        if (chkBool != groundBool && groundBool)
        {

            // ���� ������
            ChkFallDamaged();
        }
    }

    /// <summary>
    /// �޸��� üũ
    /// </summary>
    private void ChkRun()
    {

        // �ȴٰ� �޸����� ��ȭ �ϴ� ���� üũ
        chkBool = runBool;

        // �޸��� Ű�� ������ ���� ���׹̳��� 0�̻��� ��
        if (Input.GetKey(KeyCode.LeftShift) && nowStamina > 0)
        {

            // �޸��� ����
            runBool = true;

            applySpd = runSpd;

            // HealthMan�̸� �̼� 2��
            if (hidden.ChkAbility(Hidden.Ability.HealthMan))
            {

                applySpd *= 2f;
            }
        }
        // �޸� �� ���� ���
        else
        {

            runBool = false;
            applySpd = status.MoveSpd;
        }

        // ���� ��ȭ�� �Ͼ���Ƿ� animator �ӵ� ����
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

        // �ִϸ��̼� ���긦 ���� �ؾ��ϴ°� �뵵�� ����
        chkBool = moveBool;

        // ���� ����
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
    /// ���� Ȯ��
    /// </summary>
    private void ChkJump()
    {

        // ���鿡 ��Ұ� ���� Ű�� ���� ���
        if ((groundBool || ladderBool) && Input.GetKeyDown(KeyCode.Space))
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

        // ���콺 ������ ������ ������ ������ ��� ����
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
    /// ���׹̳� ä���� Ȯ��
    /// </summary>
    private void ChkStamina()
    {

        // ���׹̳� ���� �ִ°� ���°� �뵵�� ���δ�
        chkBool = true;

        // ���׹̳� ä�� ����
        // �̵� ���̰ų� ���� ���̸� ä�� �� ���� �ܴ̿� ä�� �� �ִ�
        if (moveBool || !groundBool)
        {

            staminaBool = false;
        }
        else
        {

            staminaBool = true;
        }

        // �޸��鼭 �̵� ���� ���� ���׹̳��� ��´�
        if (moveBool && runBool)
        {

            nowStamina -= Time.deltaTime;

            if (nowStamina < 0)
            {

                nowStamina = 0;
            }
        }
        // ���׹̳� ä��� �κ�
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
        // ���׹̳� ������ ����
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
    /// ���� ������
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
    /// ���ݿ��� ������ �޼ҵ�
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
            targetStats.transform.position + targetStats.transform.up, Quaternion.identity);
    }

    protected virtual void SetHammerSnd()
    {

        myAS.SetSnd(atkSnd);
        myAS.GetSnd(true);
    }

    /// <summary>
    /// �ǰ� �޼ҵ�
    /// </summary>
    /// <param name="atk">������ ����� ���ݷ�</param>
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