// #define ExampleUnity

using Photon.Pun;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviourPun, EnemyDamaged
{
    /// <summary>
    /// 적의 상태 종류
    /// 상태에 따라 행동 메소드 실행
    /// </summary>
    public enum EnemyState { Idle,                        // 대기 상태
                            Move,                         // 이동 상태
                            Attack,                       // 공격 상태
                            Damaged,                      // 맞은 상태
                            Dead                          // 사망 상태
                            }           

    /// <summary>
    /// 1페이즈 공격 패턴 2개
    /// 2페이즈 공격 패턴 4개 데미지 업
    /// </summary>
    public enum EnemyPhase 
    {
        first,                                            // 1페이즈
        second                                            // 2페이즈
    }

    [Header("적 변수")]
    [SerializeField] private EnemyState enemyState;       // 현재 상태를 표현할 변수
    [SerializeField] private EnemyPhase enemyPhase;       // 현재 페이즈를 표현할 변수
    [SerializeField] public int maxHp;                    // 최대 체력
    [SerializeField] public int atk;                      // 공격력
    [SerializeField] public int def;                      // 방어력
    [SerializeField] private float moveSpeed;             // 걷는 속도
    [SerializeField] private float runSpeed;              // 달리기 속도
    [SerializeField] private float sightRadius;           // 시야 거리
    [SerializeField] private float sightAngle;            // 시야 반경
    [SerializeField] private float atkRadius;             // 공격 거리
    [SerializeField] private float atkAngle;              // 공격 반경
    [SerializeField] private float actionTime = 0.2f;     // 행동 간격
    [SerializeField] private CharacterController cc;      // 계단 언덕 오를 때 이용되는 컴포넌트
    [SerializeField] private Animator anim;               // 모션 컴포넌트
    [SerializeField] private NavMeshAgent agent;          // 길 찾기용? 컴포넌트
    [SerializeField] private MeshRenderer weaponMesh;     // 무기 색상
    [SerializeField] private Transform targetTrans;       // 타겟 위치
    [SerializeField] private string playerTag = "Player"; // 적으로 인식할 태그
    [SerializeField] private LayerMask targetMask;        // 적으로 인식할 레이어
    [SerializeField] private LayerMask obstacleMask;      // 장애물로 인식할 레이어 


    [SerializeField] [Range(1, 5)] private int phaseNum1; // 1페이즈 공격 패턴 갯수
    [SerializeField] [Range(1, 5)] private int phaseNum2; // 2페이즈 공격 패턴 갯수

    private WaitForSeconds waitTime;
    private Vector3 _dir;                                 // 계산용 방향
                                                          // 플레이어 위치 - 적 위치 
    private int currentHp;                                // 현재 체력
    private Vector3 dir;                                  // 시야 방향
                                                          // dir에서 y 값 = 0인 길이가 1인 벡터
    private bool isDelay;                                 // 한턴 대기 


#if ExampleUnity
    // 확인용 변수
    float lookingAngle;
    Vector3 leftDir;
    Vector3 rightDir;
    Vector3 lookDir;
    Vector3 leftatkDir;
    Vector3 rightatkDir;
#endif

    // 적 AI의 초기 스펙을 결정하는 셋업 메서드
    [PunRPC]
    public void Setup(int maxHp, int atk, int def, float moveSpeed)
    {
        Debug.Log("Setup 메서드 들어옴");

        // 체력 설정
        this.maxHp = maxHp;
        
        SetHp(); // currentHp <= 0 이면 바로 죽음 상태
        
        weaponMesh.material.color = Color.white; // 원래 색상 변형

        // 내비메쉬 에이전트의 이동 속도 설정
        // this.runSpeed = runSpeed;
        this.moveSpeed = moveSpeed;

        // 데미지
        this.atk = atk;

        // 방어력
        this.def = def;

        if (currentHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// cc랑 agent 1번만 불러와도 충분하다 생각해서 여기로 빼놓음
    /// </summary>
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        weaponMesh = GetComponentInChildren<MeshRenderer>();
    }

    /// <summary>
    /// 부활 시 할 행동 hp 초기화, cc, agent 체크, 이동 속도 설정 그리고 행동 시작
    /// </summary>
    private void OnEnable() // 오브젝트 풀링 기법 고려해서 OnEnable 
    {
        SetHp();

        // 이동 방법이 없는 경우 비활성화
        if (cc == null || agent == null) 
        {

            gameObject.SetActive(false); 
        }

        // 애니메이터 가져오기
        anim = GetComponent<Animator>(); 
        anim?.SetBool("runBool", false);
        SetAtkNum();

        // 초기는 걷는 속도
        agent.speed = moveSpeed;

        // 행동 간격 설정
        if (waitTime == null)
        {
            waitTime = new WaitForSeconds(actionTime); 
        }

        // 행동 시작 !!
        StartCoroutine(Action()); 
    }

    /// <summary>
    /// hp 세팅 0이하면 사망 상태로 만들어 줌
    /// </summary>
    private void SetHp()
    {

        // 0보다 크면 대기 상태, 작으면 사망 상태 및 1페이즈
        currentHp = maxHp;
        enemyState = currentHp > 0 ? EnemyState.Idle : EnemyState.Dead; 
        enemyPhase = EnemyPhase.first; 
        
    }


    private void OnDisable() 
    {
        // 아이템 생성?
    }

    /// <summary>
    /// FSM 알고리즘
    /// </summary>
    /// <returns>대기할 시간</returns>
    private IEnumerator Action() 
    {
        while (true)
        {
#if ExampleUnity
            // Gizmos로 확인
            lookingAngle = transform.eulerAngles.y;
            rightDir = AngleToDir(lookingAngle + sightAngle * 0.5f);
            leftDir = AngleToDir(lookingAngle - sightAngle * 0.5f);
            lookDir = AngleToDir(lookingAngle);
            rightatkDir = AngleToDir(lookingAngle + atkAngle * 0.5f);
            leftatkDir = AngleToDir(lookingAngle - atkAngle * 0.5f);
#endif


            // 상태에 따라 행동 
            ChkAction(); 
     
            if (enemyState == EnemyState.Dead)
            {
                Debug.Log("코루틴 종료");
                yield break;
            }

            // 앞에서 설정한 시간만큼 대기
            yield return waitTime; 
        }
    }

#if ExampleUnity
    /// <summary>
    /// 시계 방향으로 각도만큼 회전한 xz평면과 평행한 단위 벡터를 반환
    /// </summary>
    /// <param name="angle">각도( 단위 : 도 )</param>
    /// <returns>벡터</returns>
    Vector3 AngleToDir(float angle) // 방향 설정 메소드
    {
        float radian = angle * Mathf.Deg2Rad; // 라디안 단위로 변환
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian)); // xz 축의 회전 정도
    }

    /// <summary>
    /// 시야 표현 메소드
    /// </summary>
    private void OnDrawGizmos()
    {
        // 시야 범위
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Debug.DrawRay(transform.position, rightDir * sightRadius, Color.blue);
        Debug.DrawRay(transform.position, leftDir * sightRadius, Color.blue);
        Debug.DrawRay(transform.position, lookDir * sightRadius, Color.cyan);
        
        // 공격 범위
        Gizmos.DrawWireSphere(transform.position, atkRadius);
        Debug.DrawRay(transform.position, rightatkDir * atkRadius, Color.red);
        Debug.DrawRay(transform.position, leftatkDir * atkRadius, Color.red);
    }
#endif


    #region 상태 관련 메소드
    /// <summary>
    /// FSM 대기 상태면 대기 행동 메소드 사망이면 사망 상태 메소드 실행
    /// </summary>
    private void ChkAction() 
    {
        switch (enemyState) 
        {

            // 경계 범위 안에 올 때까지 가만히 있음 
            case EnemyState.Idle: 
                StateIdle(); 
                break;

            // 경계 범위에서 벗어났는지 혹은 공격 범위에 들어왔는지 체크하고 이동
            case EnemyState.Move: 
                StateMove(); 
                break;

            // 공격 행동 취함 특정 타이밍에 공격 범위 안에 있으면 공격한다
            case EnemyState.Attack: 
                StateAttack(); 
                              
                break;

            // 맞거나 사망상태면 아무것도 안함
            case EnemyState.Damaged: 
            case EnemyState.Dead:    
                break;
        }

        return;
    }

    /// <summary>
    /// 대기 행동 메소드 - 경계만 선다.
    /// </summary>
    private void StateIdle() 
    {

        // 경계 범위에 들어왔으면
        if (FindTarget(sightRadius, sightAngle)) 
        {

            // 이동 상태 변환 애니메이션 있으면 이동 시작!
            enemyState = EnemyState.Move; 
            anim?.SetBool("walkBool", true); 

            return;
        }
    }

    /// <summary>
    /// 이동 행동 메소드 - 경계로 돌아갈지 공격체크하고 타겟으로 이동하는 메소드
    /// </summary>
    private void StateMove() 
    {

        // 딜레이 상태일 때
        if (isDelay) 
        {
            isDelay = false;
        }
        // 딜레이 상태가 아닐 시
        else
        {
            // 이동 이외의 대부분의 상태에서는 agent를 끄기에 여기서 꺼져있는가 체크
            if (!agent.enabled) 
            {
                agent.enabled = true;
            }

            if (targetTrans != null)
            {
                // 목적지는 타겟의 장소
                agent.destination = targetTrans.position;
                // Debug.Log("이동 중...");
            }

            // 공격 범위 안이면 공격상태        
            if (FindTarget(atkRadius, atkAngle)) 
            {
                enemyState = EnemyState.Attack; 
                anim?.SetBool("runBool", false);
                anim?.SetBool("atkBool", true); 

                Debug.Log("공격");

                return;
            }

            // 경계 범위 벗어 났는지 체크 벗어나면 대기 상태
            if (!FindTarget(sightRadius, sightAngle)) 
            {
                enemyState = EnemyState.Idle; 
                targetTrans = null; 
                agent.enabled = false; 
                anim?.SetBool("walkBool", false); 
                Debug.Log("대기 상태 변경");

                return;
            }
        }

    }

    /// <summary>
    /// 공격 행동 - 공격 모션 취하고, 타겟의 방향으로 서서히 바라본다.
    /// </summary>
    public void StateAttack() 
    {

        // agent 끄기 제자리 공격 이라서 끈다 이동형 공격 이면 안끈다
        if (agent.enabled)
        {

            agent.enabled = false;
        }

        // 먼저 공격 범위 안에 있는지 체크 없으면 이동 상태 변경
        if (!FindTarget(atkRadius, atkAngle)) 
        {
            isDelay = true;
            enemyState = EnemyState.Move; 
            anim?.SetBool("atkBool", false); 
            anim?.SetBool("walkBool", true); 

            Debug.Log("이동 상태 변경");

            return;
        }

        transform.LookAt(targetTrans);
    }

    #endregion


    #region Aniamtor Event 메소드
    /// <summary>
    /// 부채꼴 판정 공격 메소드
    /// </summary>
    public void AttackSector()
    {

        // targetMask에 설정된 적이 구형 반경안에 들어왔는지 확인
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, atkRadius, targetMask);

        // 들어온 애가 있는지 확인
        if (cols.Length > 0) 
        {

            foreach (var item in cols)
            {

                // cc, capsuleCollider 2개 판별해서 하나만 판별하게 캐스팅함
                if (item as CapsuleCollider == null || item.gameObject == gameObject) 
                {                                                                       

                    continue;
                }

                _dir = (item.gameObject.transform.position - transform.position).normalized;
                dir = _dir;
                dir.y = 0;
                dir = dir.normalized;

                // 공격 반경 안에 있는 경우
                if (Vector3.Angle(_dir, transform.forward) < atkAngle * 0.5f) 
                {

                    // 대상과 적 사이에 장애물이 있는지 체크
                    // 있으면 데미지 안줌!
                    // 예를 들어 플레이어와 적 사이에 벽이 있는데 데미지 들어가는 경우 방지 
                    RaycastHit hit;

                    // 적의 위치에서 플레이어 방향으로 레이저를 쏜다
                    if (Physics.Raycast(transform.position + cc.center, _dir, out hit, atkRadius, targetMask | obstacleMask)) 
                    {

                        // hit의 태그를 비교
                        if (hit.transform.tag == playerTag) 
                        {
                            // 딜 추가! 페이즈 2면 데미지도 추가
                            int dmg = enemyPhase == EnemyPhase.second ? 5 + atk : atk ; 

                            // 데미지 주는 구간
                            item.gameObject.GetComponent<StatusController>()?.DecreaseHP(dmg); 
                        }
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 내려찍기 공격에 쓸 데미지 판정법
    /// </summary>
    public void AttackRay()
    {

        // 직선에 적이 있는지 판별
        RaycastHit hit;
        if(Physics.Raycast(transform.position + cc.center, dir, out hit, atkRadius, targetMask))
        {

            // 페이즈에 따라 딜 증가
            int dmg = enemyPhase == EnemyPhase.second ? 5 + atk : atk; 

            hit.transform.gameObject.GetComponent<StatusController>()?.DecreaseHP(dmg);
        }
    }

    /// <summary>
    /// 다음 공격 패턴 설정
    /// </summary>
    public void SetAtkNum()
    {
        // 페이즈 2부터 회전과 3콤보 공격 실행
        if (enemyPhase == EnemyPhase.first) 
        {
            anim?.SetInteger("atkInt", UnityEngine.Random.Range(0, phaseNum1));
        }
        else
        {
            anim?.SetInteger("atkInt", UnityEngine.Random.Range(0, phaseNum2));
        }
    }

    /// <summary>
    /// 맞는 모션 탈출 메소드
    /// </summary>
    public void EscapeDamaged()
    {

        enemyState = EnemyState.Move; 
    }

    /// <summary>
    /// 사망 모션 탈출 메소드
    /// </summary>
    public void EscapeDie()
    {
        gameObject.SetActive(false);
    }
    #endregion

    /// <summary>
    /// 범위안에 있는지 판별하는 메소드
    /// </summary>
    /// <param name="Radius">거리</param>
    /// <param name="Angle">각도</param>
    /// <returns>있으면 true,  없으면 false이고 타겟의 transform을 담는다</returns>
    private bool FindTarget(float Radius, float Angle)
    {

        // 구형 범위 안에 적이 있는지 체크
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, Radius, targetMask);

        // 초기화
        dir = Vector3.zero; 

        if (cols.Length > 0)
        {
            foreach(var item in cols)
            {

                // 자기자신만 아니게 확인
                if (item.gameObject == gameObject) 
                    continue;

                _dir = (item.gameObject.transform.position - transform.position);
                dir = _dir;
                dir.y = 0;
                dir = dir.normalized;

                // 각도안에 적이 잇는가?
                if (Vector3.Angle(_dir, transform.forward) < Angle * 0.5f)
                {

                    // 대상 사이에 벽이나 장애물이 있는지 관통 X 코드
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + cc.center, _dir, out hit, Radius, targetMask | obstacleMask))
                    {

                        // Player 태그를 가진 적이 존재하면
                        if (hit.transform.tag == playerTag)
                        {

                            // 대기 상태에서만 상대방의 transform을 담는다
                            if (enemyState == EnemyState.Idle) 
                            {

                                // 대상의 transform 참조
                                targetTrans = item.gameObject.transform; 
                            }

                            return true;
                        }
                    }
                }
            }
        }


        // 못찾았으므로 죽었을 수도 있어 null 및 반환값 false
        targetTrans = null;
        return false;
    }


    #region 외부와 상호작용할 메소드
    /// <summary>
    /// 피격 메소드
    /// </summary>
    /// <param name="attack">공격한 대상의 공격력!</param>
    public void OnDamagedProcess(int attack = 0)
    {
        // 최소 데미지 1 보정 데미지 계산식 
        // 공격력 - 방어력만큼 깎는다
        currentHp -= CalcDmg(attack);

        // 페이즈 체크
        if (enemyPhase == EnemyPhase.first)
        {
            ChkPhase(); 
        }

        // hp에 맞춰 모션 실행
        if (currentHp > 0) 
        {
            ChkDamaged(); 
        }
        else 
        {
            ChkDie(); 
        }

        return;
    }

    /// <summary>
    /// 데미지 계산식 최소데미지 1 보정!
    /// </summary>
    /// <param name="attack">적의 공격력</param>
    /// <returns>공격력 - 방어력, 최소 데미지 1보정</returns>
    private int CalcDmg(int attack)
    {

        int dmg = attack - this.def;
        if (dmg < 1) dmg = 1;

        return dmg;
    }

    /// <summary>
    /// 70% 미만이면 페이즈 변경 
    /// </summary>
    private void ChkPhase() 
    {

        // 무기 빨갛게 달리기 모션 실행
        if (((10 *currentHp) / maxHp )  <= 6) 
        {

            enemyPhase = EnemyPhase.second; 
            weaponMesh.material.color = Color.red; 
            anim?.SetBool("runBool", true);
            agent.speed = runSpeed;
        }
    }

    /// <summary>
    /// 피격 모션 취하게 하는 메소드
    /// </summary>
    private void ChkDamaged()
    {
        // 피격 모션 가능한 상태인지 판별
        if (enemyState != EnemyState.Damaged
            && enemyState != EnemyState.Attack
            && !isDelay)
        {
            isDelay = true;
            enemyState = EnemyState.Damaged;
            anim?.SetTrigger("dmgTrigger");
        }
        return;
    }


    /// <summary>
    /// 사망 모션 취하는 메소드
    /// </summary>
    private void ChkDie()
    {

        // 음수 값 보정 식
        currentHp = 0; 
        if (enemyState != EnemyState.Dead)
        {
            enemyState = EnemyState.Dead; 
            anim?.SetTrigger("dieTrigger");
        }

        return;
    }
    #endregion
}
