using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ThirdPersonController : Stats
{
    #region Component or Object
    
    [Header("컴포넌트 및 오브젝트")]

    [Tooltip("캐릭터 몸체 오브젝트")] [SerializeField]
    private Transform chrBody; 

    [Tooltip("카메라 상자")] [SerializeField]
    private Transform cameraBox;
    
    [Tooltip("캐릭터 애니메이터")]
    public Animator chrAnimator;

    [Tooltip("공격 범위 콜라이더")] [SerializeField]
    private BoxCollider atkCollider;

    [Tooltip("색상")] [SerializeField]
    private SkinnedMeshRenderer chrMesh;

    [Tooltip("Hammer")]
    public GameObject hammerObj;
    #endregion Component or Object


    #region Convertible Variable

    [Header("변수")]
    [Tooltip("캐릭터 이동 속도")] [SerializeField]
    private float moveSpeed;

    [Tooltip("캐릭터 달리기 속도")] [SerializeField]
    private float runSpeed;

    [Tooltip("캐릭터 점프 파워")] [SerializeField]
    private float jumpForce; 

    [Tooltip("카메라 민감도")] [SerializeField]
    private float lookSensitivity;

    [Tooltip("스테미나(단위 초)")] [SerializeField]
    private float maxStamina;
    #endregion Convertible Variable

    // 뛰는 상태
    private bool runBool;

    // 지면에 닿았는지 확인하는 변수
    private bool groundBool;

    // 움직임 중?
    private bool activeBool;

    // 버튼 누름 확인
    private bool pushBool;

    // 스테미너 사용 가능한 상태 확인
    private bool staminaBool;


    // 적용 속도
    private float applySpeed;

    // 현재 스테미너
    private float nowStamina;

    // 강체
    private Rigidbody playerRigidbody;

    // 플레이어 캡슐 콜라이더
    private CapsuleCollider playerCollider;

    public float forcePow = 10f;

    private void Start()
    {

        // 컴포넌트 가져오기
        playerRigidbody = GetComponent<Rigidbody>(); 
        playerCollider = GetComponent<CapsuleCollider>(); 

        // 피격 시 변환 시킬 마테리얼
        if (chrMesh == null)
        {
            chrMesh = GetComponentInChildren<SkinnedMeshRenderer>(); 
        }

        // hp 설정
        SetHp();

        // 초기 스테미너 세팅
        nowStamina = maxStamina;

        // ui 초기화
        StatsUI.instance.SetHp(nowHp);
        StatsUI.instance.SetStamina(nowStamina);
        StatsUI.instance.SetAtk(atk);

        // 초기 애니메이션 속도 세팅
        chrAnimator.speed = 2.0f;

        // 초기 이동속도 세팅
        applySpeed = moveSpeed;

        // 히든 초기화
        hidden = Hidden.None;
        
    }

    void Update()
    {
        // 죽지 않은 상태에서만 실행 가능
        if (!deadBool 
            // && !GameManager.instance.uiBool
            ) 
        {
            // 키입력 상태 체크
            IsGround(); // 지면 체크
            RunChk(); // 달리기 체크

            // 이동 관리
            Move(); // 이동
            Jump(); // 점프

            // 시야 관리
            LookAround(); // 시야 확인

            // 공격
            Attack(); // 공격

            // 행동 후 상태 체크 
            StaminaChk(); // 스테미너 상태 체크

            if (Input.GetKeyDown(KeyCode.G))
            {
                StartSquat();
            }
        }
    }

    void StartSquat()
    {
        chrAnimator.SetTrigger("GG");
    }


    void IsGround() // 지면 체크 
    {
        groundBool = Physics.Raycast(transform.position, Vector3.down, playerCollider.bounds.extents.y + 0.1f);
        // 현재는 레이케스트를 통해 플레이어 지면 밑에 콜라이더의 크기 절반 + 0.1 만큼 거리 체크
    }

    void RunChk() // 달리기 체크
    {
        if (Input.GetKey(KeyCode.LeftShift) && nowStamina > 0) // 왼쪽 쉬프트를 달리기 속도 적용
        {
            runBool = true; // 스테미너 깎을 때 쓸 용도

            if (hidden != Hidden.HealthMan)
            {
                applySpeed = runSpeed; // 달리기 속도로 적용
            }
            else
            {
                applySpeed = runSpeed * 2f; // 달리기 속도 2배
            }
            staminaBool = true; // 스테미나 사용 상태
        }
        else
        {
            runBool = false; // 달리기 상태 X
            applySpeed = moveSpeed; // 걷는 속도 적용
            staminaBool = false; // 스테미너 사용상태 X
        }
    }

    void Move() // 이동
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); // 상하좌우 키 입력값 보관
        pushBool = (moveInput != Vector2.zero); // 키입력 누른지 안누른지 확인할 때 쓰는 용도

        if (pushBool) // 이동
        {
            // 구간 묶음 세팅 방법
            #region animator // 애니메이터 변환에 한 번만 적용
            if (!activeBool) // 애니메이션 한 번만 적용
            {
                activeBool = true;
                chrAnimator.SetBool("runChk", true); // 걷는 모션
            }

            if (runBool)
            {
                chrAnimator.speed = 3.0f; // 애니메이션 속도 빠르게 
            }
            else
            {
                chrAnimator.speed = 2.0f; // 애니메이션 속도 느리게
            }

            if (hidden == Hidden.TimeConqueror)
            {
                chrAnimator.speed *= GameManager.instance.accTime;
            }
            
            #endregion animator


            Vector3 lookForward = new Vector3(cameraBox.forward.x, 0f, cameraBox.forward.z).normalized; // 정면
            Vector3 lookRight = new Vector3(cameraBox.right.x, 0f, cameraBox.right.z).normalized; // 옆면
            Vector3 moveDir = (lookForward * moveInput.y + lookRight * moveInput.x).normalized; // 이동 방향 

            // chrBody.forward = lookForward; // 게처럼 옆으로 걷는거
            chrBody.forward = moveDir; // 캐릭터가 이동하는 방향으로 바라보기
            // transform.position += moveDir * Time.deltaTime * moveSpeed;

            if (hidden != Hidden.TimeConqueror)
            {
                playerRigidbody.MovePosition(transform.position + moveDir * applySpeed * Time.deltaTime); // 이동
            }
            else
            {
                playerRigidbody.MovePosition(transform.position + moveDir * applySpeed * 2 * GameManager.instance.accTime * Time.deltaTime); // 이동
            }

        }
        else 
        {
            if (activeBool) // 활동 중인데 키 입력이 없으면 비활성화 모션으로 들어가기
            {
                activeBool = false; // 중복으로 들어오는거 막기
                chrAnimator.SetBool("runChk", false); // idle 모션 활성화
            }
        }
    }

    void Jump() // 점프
    {
        if (groundBool && Input.GetKeyDown(KeyCode.Space)) // 지면에 닫고 스페이스바를 누른 경우
        {

            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 위로 점프
        }
    }

    void LookAround() // 카메라 이동
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * lookSensitivity; // 각각 x, y 축 회전 값
        Vector3 camAngle = cameraBox.rotation.eulerAngles; // 현재 카메라 회전 앵글
        if (hidden != Hidden.TimeConqueror)
        {
            mouseDelta *= Time.deltaTime * 100f;
        }
        else
        {
            mouseDelta *= Time.unscaledDeltaTime * 100f;
        }
        float x = camAngle.x - mouseDelta.x; // 값 제한을 두기위해 변수로 받아옴

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 90f); // 위 방향 90도 제한
            // -1f 해야지 밑으로 이동 가능
        }
        else
        {
            x = Mathf.Clamp(x, 330f, 361f); // 아래방향 90도 제한
            // 361f 해야지 위쪽으로 넘어가기 가능
        }
        
        cameraBox.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.y, camAngle.z);// 카메라 회전
    }

    void Attack() // 공격
    {
        if (Input.GetMouseButtonDown(0) && !chrAnimator.GetCurrentAnimatorStateInfo(0).IsName("1_attack")) // 공격 애니메이션 비활성화 & 공격 버튼 누른 경우
        {

            chrAnimator.SetBool("attackChk", true); // 공격 모션
            dmgCol.enabled = true;
        }
    }

    void StaminaChk() // 스테미나 체크
    {
        if (staminaBool && pushBool) // 스테미나 사용 가능한 상태고 버튼 눌렀을 때,
        {
            if (hidden != Hidden.HealthMan) // HealthMan 능력 갖고 있으면 무한 스테미나!
            {
                nowStamina -= Time.deltaTime; // 스테미너 감소
                
                if (nowStamina < 0) // 스테미나가 음수면 0으로
                {
                    nowStamina = 0; // 스테미나 0
                }

                StatsUI.instance.SetStamina(nowStamina);
            }
            
        }

        else if (!pushBool) // 이동 키 입력이 없을 시
        {
            if (nowStamina < maxStamina) // 스테미나가 풀이 아닌 경우
            {
                nowStamina += Time.deltaTime; // 스테미나 상승

                if (nowStamina > maxStamina) // 최대값 넘어가면 최대값으로 설정
                {
                    nowStamina = maxStamina;
                }

                StatsUI.instance.SetStamina(nowStamina);
            }
        }

    }

    /// <summary>
    /// 피격 메소드
    /// </summary>
    /// <param name="_damage">데미지</param>
    public override void Damaged(int _damage)
    {
        chrAnimator.SetBool("damageChk", true); // 데미지 상태 표시
        base.Damaged(_damage); // 데미지 주는 함수 최소값 1 보정
                               // 및 사망 확인
        StatsUI.instance.SetHp(nowHp);
    }

    public void ChangeColor(Color color)
    {
        chrMesh.material.color = color;
    }

    protected override void Dead()
    {
        base.Dead();

        GameManager.instance.GameOver(false);
    }

    /// <summary>
    /// 특성 X
    /// </summary>
    public void SetNone()
    {
        hidden = Hidden.None;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// 죽지 않는 특성
    /// </summary>
    public void SetImmortality()
    {
        hidden = Hidden.Immortality;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// 스테미너 무한
    /// </summary>
    public void SetHealthMan()
    {
        hidden = Hidden.HealthMan;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// 엄청 안좋은 특성
    /// </summary>
    public void SetTimeConqueror()
    {
        hidden = Hidden.TimeConqueror;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// 폭발 어태커
    /// </summary>
    public void SetNuclearAttacker()
    {
        hidden = Hidden.NuclearAttacker;
        StatsUI.instance.SetAtk(nuclearAtk);
    }

    /// <summary>
    /// 연속 어태커
    /// </summary>
    public void SetContinuousAttacker()
    {
        hidden = Hidden.ContinuousAttacker;
        StatsUI.instance.SetAtk(atk);
    }

    /// <summary>
    /// 넉백
    /// </summary>
    public void SetHomeRun()
    {
        hidden = Hidden.HomeRun;
        StatsUI.instance.SetAtk(atk);
    }
}