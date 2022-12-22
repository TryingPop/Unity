// #define InUnity

using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static BossEnemy;

public class NormalEnemy : MonoBehaviourPunCallbacks, EnemyDamaged
{
    /// <summary>
    /// Idle, Tracking, Attack, Damaged, Dead
    /// </summary>
    public enum NormalState 
    {
        Idle,        // 대기
        Tracking,    // 추적
        Attack,      // 공격
        Damaged,     // 피격
        Dead         // 사망
    }

    #region Variable

    //private PhotonView PV;

    // 체력, 공격력, 방어력, 이속, 행동 간격, 상태 변환용 추적 거리 및 반경, 공격 거리 및 반경
    [SerializeField] protected NormalEnemyStats stats;
    [SerializeField] protected NormalEnemyAnimator animator;    // 일반 좀비 애니메이터
    [SerializeField] protected NormalState state;               // 현재 상태(Idle, tracking, Attack, Damaged, Dead )
    [SerializeField] protected CharacterController cc;          // 캐릭터 컨트롤러
    [SerializeField] protected NavMeshAgent agent;              // 길찾기 AI
    [SerializeField] protected Transform targetTrans;           // 타겟 위치
    [SerializeField] protected string playerTag;                // 적 태그
    [SerializeField] protected LayerMask targetMask;            // 적 레이어 
    [SerializeField] protected LayerMask obstacleMask;          // 건물 레이어

    protected int currentHp;                                    // 현재 체력
    protected bool isDelay;                                     // 공격, 피격 대기 시간

    protected Vector3 lookDir;                                  // 연산용 벡터
    protected WaitForSeconds waitTime;                          // 매번 생성 X

#if InUnity
    // 확인용 변수
    float lookingAngle;
    Vector3 leftDir;
    Vector3 rightDir;
    Vector3 lookDir;
    Vector3 leftatkDir;
    Vector3 rightatkDir;
#endif

    #endregion Variable

    #region Unity Method

    /// <summary>
    /// 컴포넌트 초기화
    /// </summary>
    protected void Awake()
    {
        //PV = GetComponent<PhotonView>();

        // 캐릭터 컨트롤러 받아오기
        cc = GetComponent<CharacterController>();

        // 이동 ai 받아오기
        agent = GetComponent<NavMeshAgent>();

        // 애니메이터 없으면 받아오기
        if (animator == null) 
        { 
            animator = GetComponent<NormalEnemyAnimator>();
        }

        // 행동 타임 0 이하면 0.2로 설정해버린다
        if (stats.actionTime <= 0) 
        {
            stats.actionTime = 0.2f;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();

        state = NormalState.Idle;
        currentHp = stats.maxHp;
        agent.speed = stats.moveSpd;

        // 행동 간격 설정
        waitTime = new WaitForSeconds(stats.actionTime); 

        // 상태 별 행동 체크 시작
        StartCoroutine(Action());
    }


    #endregion Unity Method

    #region 충돌 감지
    /// <summary>
    /// 구의 부채꼴 범위와 충돌하는 객체 찾기
    /// </summary>
    /// <param name="Radius">거리</param>
    /// <param name="Angle">각도</param>
    /// <returns>있으면 true, 없으면 false</returns>
    private bool FindTarget(float Radius, float Angle) 
    {

        // 구형 targetMask를 기준으로 찾는다
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, Radius, targetMask);

        // 적어도 1개 이상 찾는 경우
        if (cols.Length > 0) 
        {

            // 한개씩 검사
            foreach (var item in cols) 
            {

                // 현재 객체와 같은 객체인지 확인 후 true일 경우 패스한다.
                if (item.gameObject == gameObject) 
                {
                    continue; 
                }

                // 초반 연산용
                lookDir = item.gameObject.transform.position - transform.position; 

                // 각도 안에 있는지 확인
                if (Vector3.Angle(lookDir, transform.forward) < 0.5f * Angle) 
                {

                    // 중간에 장애물 있는지 확인
                    RaycastHit hit;
                    Physics.Raycast(cc.center + transform.position, lookDir, out hit, Radius, targetMask 
                        | obstacleMask
                        );

                    // 플레이어와 충돌했을 때 실행
                    if (hit.transform?.tag == playerTag)
                    {
                        targetTrans = item.gameObject.transform;

                        // 바라볼 방향으로 설정
                        lookDir.y = 0;
                        lookDir = lookDir.normalized;

                        // 만족하는 애가 있어 true로 탈출
                        return true; 
                    }
                }
            }
        }

        // 만족하는 애가 없는 경우 여기로 오기에 타겟 좌표 X 및 반환값 false
        targetTrans = null;
        return false; 
    }
    #endregion  충돌 감지

    #region FSM
    /// <summary>
    /// 행동 메소드
    /// </summary>
    /// <returns>앞에서 설정한 시간만큼 대기!</returns>
    private IEnumerator Action()
    {
        while (true)
        {

            // 상태 확인
            ChkAction();

            yield return waitTime;
        }
    }

    /// <summary>
    /// FSM 메소드
    /// 현재 상태에 따라 행동 메소드 실행
    /// </summary>
    private void ChkAction()
    {

        // 현재 상태를 기준
        switch (state)
        {

            // 경계 범위 안에 올 때 까지 가만히 있는다
            case NormalState.Idle: 
                StateIdle();
                break;

            // 대상을 쫓아가고 공격 범위 판정
            case NormalState.Tracking: 
                                       
                StateTracking();       
                break;

            // 공격 모션 및 공격 범위 판정
            case NormalState.Attack: 
                StateAttack();
                break;

            // 피격 및 사망 상태에서는 아무것도 안한다
            case NormalState.Damaged: 
            case NormalState.Dead: 
                break;
        }
    }

    /// <summary>
    /// 대기 상태에서 행동 하는 메소드
    /// </summary>
    private void StateIdle() 
    {

        // 경계 범위 판정
        if (FindTarget(stats.trackingRadius, stats.trackingAngle)) 
        {

            // 추적 상태 변경 및 이동 상태 시작
            state = NormalState.Tracking; 
            animator.IdleToMove(); 
        }
    }

    /// <summary>
    /// 추적 상태에서 행동 하는 메소드
    /// </summary>
    private void StateTracking() 
    {

        // 피격 이나 공격 상태 탈출 시 실행되는 구문
        // 1턴간 대기
        if (isDelay) 
        {

            isDelay = false;
        }
        // 이외 경우
        else
        {

            // agent 일단 키고 본다
            if (!agent.enabled) 
            {

                agent.enabled = true;
            }

            // 대상이 있는 경우 이동
            if (targetTrans != null) 
            {

                agent.destination = targetTrans.position;
            }

            // 공격 상태로 변해야 하는지 체크
            if (FindTarget(stats.atkRadius, stats.atkAngle)) 
            {

                state = NormalState.Attack; 
                animator.MoveToAttack(); 

                return;
            }


            // 대기 상태로 변해야 하는지 체크
            if (!FindTarget(stats.trackingRadius, stats.trackingAngle)) 
            {

                state = NormalState.Idle; 
                agent.enabled = false; 
                animator.MoveToIdle(); 

                return;
            }
        }
    }

    /// <summary>
    /// 공격 상태에서 행동 하는 메소드
    /// </summary>
    private void StateAttack()
    {

        // agent를 끈다
        if (agent.enabled)
        {
            agent.enabled = false;
        }

        // 공격 범위 밖인지 체크
        if (!FindTarget(stats.atkRadius, stats.atkAngle))
        {

            // 잠깐의 대기 텀 준다
            isDelay = true; 
            state = NormalState.Tracking;
            animator.AttackToMove();

            return;
        }

        // 공격 상태에서는 대상을 바라보게 한다
        transform.LookAt(transform.position + lookDir); 
    }

    #endregion

    #region 애니메이터 이벤트 메소드

    /// <summary>
    /// 1인 타겟감안한 직선 공격
    /// </summary>
    public void AttackRay() 
    {
        // 직선에 적이 있는지 판별
        RaycastHit hit;
        if (Physics.Raycast(transform.position + cc.center, lookDir, out hit, stats.atkRadius, targetMask)) 
        {
            // 타겟에 피격 함수 넣으면 된다
            hit.transform.gameObject.GetComponent<StatusController>()?.DecreaseHP(stats.atk); 
        }
    }

    /// <summary>
    /// 부채꼴 판정 공격 메소드
    /// </summary>
    public void AttackSector()
    {

        // targetMask에 설정된 적이 구형 반경안에 들어왔는지 확인
        Collider[] cols = Physics.OverlapSphere(transform.position + cc.center, stats.atkRadius, targetMask);


        // 들어온 애가 있는지 확인
        if (cols.Length > 0) 
        {
            foreach (var item in cols)
            {

                // 자기자신이나 캡슐 콜라이더가 있는지 체크
                if (item as CapsuleCollider == null || item.gameObject == gameObject)
                {                                                                    
                    continue;
                }

                // 공격 반경 안에 있는 경우
                if (Vector3.Angle(item.transform.position - transform.position, transform.forward) < stats.atkAngle * 0.5f) 
                {
                    // 대상과 적 사이에 장애물이 있는지 체크
                    // 있으면 데미지 안줌!
                    // 예를 들어 플레이어와 적 사이에 벽이 있는데 데미지 들어가는 경우 방지 
                    RaycastHit hit;

                    // 적의 위치에서 플레이어 방향으로 레이저를 쏜다
                    if (Physics.Raycast(transform.position + cc.center, item.transform.position - transform.position, out hit, stats.atkRadius, targetMask | obstacleMask)) 
                    {

                        // hit의 태그를 비교
                        if (hit.transform.tag == playerTag) 
                        {

                            // 데미지를 주는 구간
                            item.gameObject.GetComponent<StatusController>()?.DecreaseHP(stats.atk); 
                        }
                    }
                }
            }
        }
    }



    /// <summary>
    /// 피격 상태 탈출에 쓸 메소드
    /// 추적 상태로 변경
    /// </summary>
    public void EscapeDamaged()
    {

        state = NormalState.Tracking;
    }

    /// <summary>
    /// 사망 상태 탈출 메소드
    /// 현재는 비활성화로 돌린다 - 파괴하려면 Destroy!
    /// </summary>
    public void EscapeDie()
    {

        gameObject.SetActive(false);
    }
    #endregion

    #region 외부와 상호작용할 메소드
    /// <summary>
    /// 데미지 계산 및 피격 or 사망 판정
    /// </summary>
    /// <param name="attack">공격 대상의 공격력</param>
    public void OnDamagedProcess(int attack = 0) 
    {

        // 체력 감소
        currentHp -= CalcDmg(attack);

        // hp를 기준으로 사망인지 피격인지 판별
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
    /// 데미지 계산 식 최소 데미지 1 보정
    /// 공격력 - 방어력 = 데미지  
    /// </summary>
    /// <param name="attack">적의 공격력</param>
    /// <returns>1 이상의 데미지</returns>
    private int CalcDmg(int attack)
    {

        int dmg = attack - stats.def; 
        if (dmg < 1) 
        {
            dmg = 1;
        }

        return dmg;  
    }

    /// <summary>
    /// 피격 시 취하는 메소드
    /// </summary>
    private void ChkDamaged() 
    {
        // 피격 모션 진입 조건
        // 공격 상태가 아니고 피격 상태가 아니고 갓 탈출한 상태가 아닐 때 실행
        if (!isDelay
            && state != NormalState.Damaged 
            && state != NormalState.Attack) 
        {

            isDelay = true; 
            state = NormalState.Damaged; 
            animator.SetDamaged(); 
        }
    }

    /// <summary>
    /// 사망 시 취하는 메소드
    /// </summary>
    private void ChkDie()
    {

        // 음수값 방지용
        currentHp = 0;

        // 사망 모션 1회만!
        if (state != NormalState.Dead)
        {
            state = NormalState.Dead; 
            animator.SetDead();
        }
    }

    /// <summary>
    /// 게임 끝날 때 실행할 메소드
    /// 혹시 몰라서 일단 만들어 놓음
    /// </summary>
    private void GameOver() 
    {

        // 모든 행동 멈춤 
        StopAllCoroutines(); 
    }
    #endregion
}


