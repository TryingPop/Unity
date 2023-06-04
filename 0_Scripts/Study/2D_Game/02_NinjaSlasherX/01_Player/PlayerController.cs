using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : BaseCharacterController
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public float initHpMax = 20.0f;
    [Range(0.1f, 100.0f)] public float initSpeed = 12.0f;

    // ���� �Ķ����
    int jumpCount = 0;

    volatile bool atkInputEnabled = false;              // ��Ƽ������ ���α׷����� �����Ϸ��� ���� ���� ó���� �����ϴ� ����ȭ�� ���� �ʴ´ٴ� ��
    volatile bool atkInputNow = false;

    bool breakEnabled = true;
    float groundFriction = 0.0f;

    float comboTimer = 0.0f;

    // �ܺ� �Ķ����
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
    public readonly static int ANISTS_DEAD =
        Animator.StringToHash("Base Layer.Player_Dead");

    // ���� ������ �Ķ����
    public static float nowHpMax = 0;
    public static float nowHp = 0;
    public static int score = 0;

    public static bool checkPointEnabled = false;
    public static string checkPointSceneName = "";
    public static string checkPointLabelName = "";
    public static float checkPointHp = 0;

    public static bool itemKeyA = false;
    public static bool itemKeyB = false;
    public static bool itemKeyC = false;

    // �ܺηκ��� ó���� �����ϱ� ���� �Ķ����
    public static bool initParam = true;
    public static float startFadeTime = 2.0f;

    // �⺻ �Ķ����
    [HideInInspector] public Vector3 enemyActiveZonePointA;
    [HideInInspector] public Vector3 enemyActiveZonePointB;
    [HideInInspector] public float groundY = 0.0f;

    [HideInInspector] public int comboCount = 0;

    // ĳ��
    // GameObject hudHpBar; // ���� ������ ���󰣴�
    // Text hudScore;
    // Text hudCombo;
    LineRenderer hudHpBar;   
    TextMesh hudScore;
    TextMesh hudCombo;

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    protected override void Awake()
    {

        base.Awake();

        // ĳ��
        // hudHpBar = GameObject.Find("HUD_HPBAR");
        // hudScore = GameObject.Find("HUD_Score").GetComponent<Text>();
        // hudCombo = GameObject.Find("HUD_Combo").GetComponent<Text>();
        hudHpBar = GameObject.Find("HUD_HPBar").GetComponent<LineRenderer>();
        hudScore = GameObject.Find("HUD_Score").GetComponent<TextMesh>();
        hudCombo = GameObject.Find("HUD_Combo").GetComponent<TextMesh>();


        // �Ķ���� �ʱ�ȭ
        speed = initSpeed;
        SetHp(initHpMax, initHpMax);

        // BoxCollider2D���� Ȱ�� ������ �����´�
        BoxCollider2D boxCol2D = transform.Find
            ("Collider_EnemyActiveZone").GetComponent<BoxCollider2D>();
        enemyActiveZonePointA = new Vector3
            (boxCol2D.offset.x - boxCol2D.size.x / 2.0f, boxCol2D.offset.y - boxCol2D.size.y / 2.0f);
            // (boxCol2D.center.x - boxCol2D.size.x / 2.0f, boxCol2D.center.y - boxCol2D.size.y / 2.0f);    // ���� �ٲ�鼭 ���ȵȴ�

        enemyActiveZonePointB = new Vector3
            (boxCol2D.offset.x + boxCol2D.size.x / 2.0f, boxCol2D.offset.y + boxCol2D.size.y / 2.0f);

        boxCol2D.transform.gameObject.SetActive(false);

        // �Ķ���� �ʱ�ȭ
        if (initParam)
        {

            SetHp(initHpMax, initHpMax);
            initParam = false;
        }
        if (SetHp(PlayerController.nowHp, PlayerController.nowHpMax))
        {

            // Hp�� ���� ���� 1���� ����
            SetHp(1, initHpMax);
        }

        // üũ �����Ϳ��� �ٽ� ����
        if (checkPointEnabled)
        {

            StageTrigger_CheckPoint[] triggerList = GameObject.Find("Stage").
                GetComponentsInChildren<StageTrigger_CheckPoint>();
            foreach(StageTrigger_CheckPoint trigger in triggerList)
            {

                if (trigger.labelName == checkPointLabelName)
                {

                    transform.position = trigger.transform.position;
                    groundY = transform.position.y;
                    Camera.main.GetComponent<CameraFollow>().SetCamera(trigger.cameraParam);
                    break;
                }
            }
        }

        Camera.main.transform.position = new Vector3(
            transform.position.x, groundY, Camera.main.transform.position.z);

        // VirtualPad, HUD ǥ�� ����� ����
        GameObject.Find("VRPad").SetActive(true);

        // HUD ǥ�� ���¸� ����
        Transform hud = GameObject.FindGameObjectWithTag("SubCamera").transform;
        // hud.Find("Stage_Item_Key_A").GetComponent<SpriteRenderer>().enabled = itemKeyA;
        // hud.Find("Stage_Item_Key_B").GetComponent<SpriteRenderer>().enabled = itemKeyB;
        // hud.Find("Stage_Item_Key_C").GetComponent<SpriteRenderer>().enabled = itemKeyC;
    }

    protected override void Start()
    {
        
        base.Start();

        zFoxFadeFilter.instance.FadeIn(Color.black, startFadeTime);
        startFadeTime = 2.0f;

        // �ִϸ��̼ǿ� �߰�
        seAnimationList = new AudioSource[5];
        seAnimationList[0] = AppSound.instance.SE_ATK_A1;
        seAnimationList[1] = AppSound.instance.SE_ATK_A2;
        seAnimationList[2] = AppSound.instance.SE_ATK_A3;
        seAnimationList[3] = AppSound.instance.SE_ATK_ARIAL;
        seAnimationList[4] = AppSound.instance.SE_MOV_JUMP;
    }

    protected override void Update()
    {

        base.Update();

        // ���� ǥ��
        hudHpBar.transform.localScale = new Vector3(((float)hp / hpMax), 1.0f, 1.0f);
        hudScore.text = string.Format("Score {0, 8}", score);

        if (comboTimer <= 0.0f)
        {

            hudCombo.gameObject.SetActive(false);
            comboCount = 0;
            comboTimer = 0.0f;
        }
        else
        {

            comboTimer -= Time.deltaTime;
            if (comboTimer > 5.0f)
            {

                comboTimer = 5.0f;
            }
            float s = 0.3f + 0.5f * comboTimer;
            hudCombo.gameObject.SetActive(true);
            hudCombo.transform.localScale = new Vector3(s, s, 1.0f);
        }
    }

    protected override void FixedUpdateCharacter()
    {

        // ���� ������Ʈ ��������
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // �ִϸ����Ϳ��� �� ���� �ִ� Default Layer�� ���� �����´�

        // ���� �˻�
        if (jumped)     // ���� ���� ���
        {

            // �� ���� ���鿡 ���� �ʰ� ���� ���鿡 ���� ��� ���� ���� ����
            // ���� ���鿡 �ְ� �������� 1�ʰ� �����ٸ� ������ ������ ����
            if ((grounded && !groundedPrev) || (grounded && Time.fixedTime > jumpStartTime +1.0f))
            {

                animator.SetTrigger("Idle");    // Ż�� �ִϸ��̼�
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
        else            // �ܴ̿� ���鿡 �����Ƿ� �׻� 0
        {

            jumpCount = 0;
            rigidbody2D.gravityScale = gravityScale;
        }
        
        // ���� ������ Ȯ��   
        // nameHash�� �� �̻� ���ȵǰ� fullPathHash �̿��϶�� �Ѵ�
        if(stateInfo.fullPathHash == ANISTS_ATTACK_A ||     
            stateInfo.fullPathHash == ANISTS_ATTACK_B ||
            stateInfo.fullPathHash == ANISTS_ATTACK_C ||
            stateInfo.fullPathHash == ANISTS_ATTACKJUMP_A ||
            stateInfo.fullPathHash == ANISTS_ATTACKJUMP_B)
        {

            // �̵� ����
            speedVx = 0;
        }

        // ĳ���� ����
        transform.localScale = new Vector3(     // ũ�� �������� ĳ���� �ٶ󺸴� ���� ����
            basScaleX * dir, transform.localScale.y, transform.localScale.z);

        // ���� ���߿� ���� �̵� ����
        if (jumped && !grounded && groundCheck_OnMoveObject == null)         // ���� ���� ��
        {

            if (breakEnabled)
            {

                breakEnabled = false;   
                speedVx *= 0.9f;        // ���� �ӵ��� 10% ����
            }
        }

        // �̵� ����(����) ó��
        if (breakEnabled)               // ����� ���� ��ȯ�̳� Ű�Է��� ���� ��
        {

            speedVx *= groundFriction;  // �ٷ� ����
                                        // velocity�� �̵� �����Ƿ� ���� ���� ������ �����ؾ��Ѵ�
                                        // ���Ǳ��� ǥ���Ϸ��� 0.5 ���� 1 ���� ��� ������ �ϸ�ȴ�
        }

        // ī�޶�
        // Camera.main.transform.position = transform.position - Vector3.forward;   // ���� ī�޶� �±׷� ��ϵ� ī�޶��� ��ġ�� ĳ���Ϳ� -1 ��ǥ ��ġ
        // Camera.main.transform.position = transform.position + Vector3.back;      // ���⼭�� �Ⱦ���
    }

    // �ڵ� (�ִϸ��̼� �̺�Ʈ�� �ڵ�)
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

        
        // �߰��� �ڵ�
        atkInputEnabled = false;    // �չ� ���ݿ��� �ȷο� ���� �� ���� ��ư�� ������
                                    // ���� ���� �� 2�� ������ ������ �̸� �����ϰ��� �߰�
    }

    // �ڵ� (�⺻ �׼�)
    public override void ActionMove(float n)
    {

        if (!activeSts)
        {

            return;
        }

        // �ʱ�ȭ
        float dirOld = dir;
        breakEnabled = false;

        // �ִϸ��̼� ����
        float moveSpeed = Mathf.Clamp(Mathf.Abs(n), -1.0f, 1.0f);   // -1, 1 ������ �� ����
        animator.SetFloat("MoveSpeed", moveSpeed);  // movespeed ��ġ �� ������ �޸���� �ȱ� ǥ��
        // animator.speed = 1.0f + moveSpeed;       // ���� �ȴ� ����� ���� �ٴ°� ǥ��?

        // �̵� �˻�
        if (n != 0.0f)
        {

            // �̵�
            dir = Mathf.Sign(n);
            moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;    // ���� �� �ӵ��� �ִ� 25%��, �޸� ���� 100% ����
            speedVx = initSpeed * moveSpeed * dir;
        }
        else
        {

            // �̵� ����
            breakEnabled = true;
        }

        // �� �������� ���ƺ��� �˻�
        if(dirOld != dir)
        {

            // ���� ��ȯ Ȯ��
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

                case 0:     // ù ������ ���
                    if (grounded)
                    {

                        animator.SetTrigger("Jump");                // �ִϸ��̼� ����
                        rigidbody2D.velocity = Vector2.up * 30.0f;  // ���� 30 �ӵ� �̵�
                        jumpStartTime = Time.fixedTime;             // ���� ���� �ð� ����
                        jumped = true;                              // ���� ���� 
                        jumpCount++;                                // ���� ��� �������� Ȯ��
                    }
                    break;

                case 1:     // �̴� ���� �ϴ��� üũ
                    if (!grounded)
                    {

                        animator.Play("Player_Jump", 0, 0.0f);       // ���� �ִϸ��̼� ����
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20.0f);  // ���� 20��ŭ��
                        jumped = true;                              // ���� ���� true
                        jumpCount++;                                // ���� ī��Ʈ ��
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

    public void Actiondamage(float damage)
    {

        if (!activeSts)
        {

            return;
        }

        animator.SetTrigger("DMG_A");
        speedVx = 0;
        rigidbody2D.gravityScale = gravityScale;

        if (jumped)
        {

            damage *= 1.5f;
        }

        if (SetHp(hp - damage, hpMax))
        {

            Dead(true);     // ���
        }
    }

    public void ActionEtc()
    {

        Collider2D[] otherAll = Physics2D.OverlapPointAll(groundCheck_C.position);
        foreach(Collider2D other in otherAll)
        {

            if (other.tag == "EventTrigger")
            {

                StageTrigger_Link link = other.GetComponent<StageTrigger_Link>();
                if (link != null)
                {

                    link.Jump();
                }
            }
            else if (other.tag == "KeyDoor")
            {

                StageObject_KeyDoor keydoor = other.GetComponent<StageObject_KeyDoor>();
                keydoor.OpenDoor();
            }
        }
    }

    // �ڵ� (���� �Լ�)   - Player ���� ������Ʈ�� �ݵ�� ������� �ְ� ���� �ϳ��ۿ� ���ٴ� ���� ������ �Ѵ�
    public static GameObject GetGameObject()
    {

        return GameObject.FindGameObjectWithTag("Player");
    }

    public static Transform GetTransform()
    {

        return GameObject.FindGameObjectWithTag("Player").transform;
    }

    public static PlayerController GetController()
    {

        return GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public static Animator GetAnimator()
    {

        return GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    // �ڵ� (�� ��)
    public override void Dead(bool gameOver)
    {

        // ��� ó���ص� �Ǵ°�?
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!activeSts || stateInfo.fullPathHash == ANISTS_DEAD)
        {

            return;
        }

        base.Dead(gameOver);

        zFoxFadeFilter.instance.FadeOut(Color.black, 2.0f);

        if (gameOver)
        {
            SetHp(0, hpMax);
            Invoke("GameOver", 3.0f);
        }
        else
        {

            SetHp(hp / 2, hpMax);
            Invoke("GameReset", 3.0f);
        }

        // TextMesh�� MeshRenderer�� ��Ȱ��ȭ�ؾ� �ؽ�Ʈ ǥ�ð� �ȵȴ�
        GameObject.Find("HUD_Dead").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("HUD_DeadShadow").GetComponent<MeshRenderer>().enabled = true;
        if (GameObject.Find("VRPad") != null)
        {

            GameObject.Find("VRPad").SetActive(false);
        }
    }

    public void GameOver()
    {

        PlayerController.score = 0;
        PlayerController.nowHp = PlayerController.checkPointHp;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameReset()
    {

        PlayerController.score = 0;
        PlayerController.nowHp = PlayerController.checkPointHp;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public override bool SetHp(float _hp, float _hpMax)
    {
        if (_hp > _hpMax)
        {

            _hp = _hpMax;
        }

        nowHp = _hp;
        nowHpMax = _hpMax;

        return base.SetHp(_hp, _hpMax);
    }

    public void AddCombo()
    {

        comboCount++;
        comboTimer += 1.0f;
        hudCombo.text = string.Format("Combo {0, 3}", comboCount);
    }
}
