using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    float comboTimer = 0.0f;

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
    public readonly static int ANISTS_DEAD =
        Animator.StringToHash("Base Layer.Player_Dead");

    // 저장 데이터 파라미터
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

    // 외부로부터 처리를 조작하기 위한 파라미터
    public static bool initParam = true;
    public static float startFadeTime = 2.0f;

    // 기본 파라미터
    [HideInInspector] public Vector3 enemyActiveZonePointA;
    [HideInInspector] public Vector3 enemyActiveZonePointB;
    [HideInInspector] public float groundY = 0.0f;

    [HideInInspector] public int comboCount = 0;

    // 캐시
    // GameObject hudHpBar; // 교재 방법대로 따라간다
    // Text hudScore;
    // Text hudCombo;
    LineRenderer hudHpBar;   
    TextMesh hudScore;
    TextMesh hudCombo;

    // 코드 (MonoBehaviour 기본 기능 구현)
    protected override void Awake()
    {

        base.Awake();

        // 캐시
        // hudHpBar = GameObject.Find("HUD_HPBAR");
        // hudScore = GameObject.Find("HUD_Score").GetComponent<Text>();
        // hudCombo = GameObject.Find("HUD_Combo").GetComponent<Text>();
        hudHpBar = GameObject.Find("HUD_HPBar").GetComponent<LineRenderer>();
        hudScore = GameObject.Find("HUD_Score").GetComponent<TextMesh>();
        hudCombo = GameObject.Find("HUD_Combo").GetComponent<TextMesh>();


        // 파라미터 초기화
        speed = initSpeed;
        SetHp(initHpMax, initHpMax);

        // BoxCollider2D에서 활성 영역을 가져온다
        BoxCollider2D boxCol2D = transform.Find
            ("Collider_EnemyActiveZone").GetComponent<BoxCollider2D>();
        enemyActiveZonePointA = new Vector3
            (boxCol2D.offset.x - boxCol2D.size.x / 2.0f, boxCol2D.offset.y - boxCol2D.size.y / 2.0f);
            // (boxCol2D.center.x - boxCol2D.size.x / 2.0f, boxCol2D.center.y - boxCol2D.size.y / 2.0f);    // 버전 바뀌면서 사용안된다

        enemyActiveZonePointB = new Vector3
            (boxCol2D.offset.x + boxCol2D.size.x / 2.0f, boxCol2D.offset.y + boxCol2D.size.y / 2.0f);

        boxCol2D.transform.gameObject.SetActive(false);

        // 파라미터 초기화
        if (initParam)
        {

            SetHp(initHpMax, initHpMax);
            initParam = false;
        }
        if (SetHp(PlayerController.nowHp, PlayerController.nowHpMax))
        {

            // Hp가 없을 때는 1부터 시작
            SetHp(1, initHpMax);
        }

        // 체크 포인터에서 다시 시작
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

        // VirtualPad, HUD 표시 기능을 설정
        GameObject.Find("VRPad").SetActive(true);

        // HUD 표시 상태를 설정
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

        // 애니메이션에 추가
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

        // 상태 표시
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
        // Camera.main.transform.position = transform.position - Vector3.forward;   // 메인 카메라 태그로 등록된 카메라의 위치는 캐릭터에 -1 좌표 위치
        // Camera.main.transform.position = transform.position + Vector3.back;      // 여기서는 안쓴다
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

            Dead(true);     // 사망
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

    // 코드 (지원 함수)   - Player 게임 오브젝트가 반드시 생성됭어 있고 씬에 하나밖에 없다는 것을 전제로 한다
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

    // 코드 (그 외)
    public override void Dead(bool gameOver)
    {

        // 사망 처리해도 되는가?
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

        // TextMesh는 MeshRenderer를 비활성화해야 텍스트 표시가 안된다
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
