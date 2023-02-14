using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBoss : Stat
{

    public enum Phase { first, second }                     // 보스 페이즈

    public static Transform playerTrans;                    // 플레이어 위치

    public Transform missileTransform;                      // 미사일 생성 위치

    public EnemyAnimation anim;                             // 적 애니메이션

    public GameObject[] missiles;                           // 원거리 공격 투사체들
    public GameObject[] summoners;                          // 소환수들

    [SerializeField] private GameObject atkZone;            // 공격 존 보여주는 오브젝트
    [SerializeField] private GameObject damagedText;        // 데미지 수치 UI

    [SerializeField] private AudioClip damagedSnd;          // 피격 사운드
    [SerializeField] protected AudioScript myAS;            // 소리 컨트롤러        

    public NavMeshAgent agent;                              
    public Transform targetTrans;                           // 타겟 위치

    public LayerMask targetLayer;                           // 찾을 Layer
    public LayerMask obstacleLayer;                         // 벽으로 인식할 레이어

    public string targetTag;

    public Phase phase;         // 현재 페이즈

    public bool nowIdleBool;    // 현재 대기 상태 확인
    private bool beforIdleBool; // 이전에 대기 상태 확인

    public bool damagedBool;    // 피격 상태 확인

    public int NowHp {          // 체력 프로퍼티
        get 
        { 

            return nowHp; 
        } 
        set 
        { 

            nowHp = value;
            if (nowHp < 0) { nowHp = 0; }
            else if (nowHp > status.Hp) { nowHp = status.Hp; }
        } 
    }

    [SerializeField] private int phaseHp = 60;      // 페이즈 진입 hp

    [SerializeField] private RangedStatus chase;    // 추적 범위와 각도 담은 스크립터블 오브젝트
    [SerializeField] private RangedStatus melee;    // 근접 공격 범위와 각도 담은 스크립터블 오브젝트
    [SerializeField] private RangedStatus ranged;   // 장거리 공격 범위와 각도 담은 스크립터블 오브젝트

    public int bulletNum = 6;       // 탄약 수

    private Node topNode;           // 실행할 노드

    private void Awake()
    {

        // 필요한 컴포넌트 비어있으면 가져오기
        if (playerTrans == null) playerTrans = GameObject.FindWithTag("Player")?.transform;
        if (anim == null) anim = GetComponent<EnemyAnimation>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        GetComp();
        
        // 무기에 충돌 시 실행할 함수 엮기
        myWC.Attack += Attack;

        // 체력 및 초기화
        Init();

        // 행동 설정
        ConstructBehaviorTree();
    }

    private void Start()
    {
        
        // 코루틴으로 행동 시작
        StartCoroutine(Action());
    }
    
    /// <summary>
    /// 노드 설정
    /// 자세한 것은 pdf 이미지 참고
    /// </summary>
    private void ConstructBehaviorTree()
    {

        IdleNode idleNode = new IdleNode(this);

        FindNode meleeFind = new FindNode(this, this.melee.RangeRadius, this.melee.RangeAngle);
        MeleeAtkNode meleeAtkNode = new MeleeAtkNode(this, status.Atk);

        FindNode chaseFind = new FindNode(this, this.chase.RangeRadius, this.chase.RangeAngle);
        ChaseNode chaseTarget = new ChaseNode(this);

        HealthNode chkPhase = new HealthNode(this, Phase.second);
        
        FindNode rangeFind = new FindNode(this, ranged.RangeRadius, ranged.RangeAngle);
        RangeAtkNode rangeAtkNode = new RangeAtkNode(this, status.Atk);

        Sequence melee = new Sequence(new List<Node> { meleeFind, meleeAtkNode });
        Sequence range = new Sequence(new List<Node> { rangeFind, rangeAtkNode });
        Sequence chase = new Sequence(new List<Node> { chaseFind, chaseTarget });

        Selector action1 = new Selector(new List<Node> { melee, chase, idleNode });
        Selector action2 = new Selector(new List<Node> { melee, range, new ChaseNode(this) });

        Sequence phase2 = new Sequence(new List<Node> { chkPhase, action2 });

        topNode = new Selector(new List<Node> { phase2, action1 });
    }

    /// <summary>
    /// 행동 시작
    /// </summary>
    /// <returns></returns>
    private IEnumerator Action()
    {

        while (!deadBool)
        {

            // 대기 상태 Cnt 조건용
            ResetIdleBool();

            // 노드 체크 시작
            topNode.Evaluate();
            yield return new WaitForSeconds(0.3f);

            // 피격 상태 탈출
            damagedBool = false;
        }
    }

    /// <summary>
    /// 공격
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Attack()
    {

        atkBool = true;

        // 공격 시행 전 1초간 공격 범위 보여주기
        atkZone.SetActive(true);
        yield return atkWaitTime;
        yield return atkWaitTime;

        // 1초동안 데미지 콜라이더 활성화
        myWC.AtkColActive(true);
        
        yield return atkWaitTime;
        yield return atkWaitTime;

        // 공격 존과 공격 범위 비활성화
        atkZone.SetActive(false);
        myWC.AtkColActive(false);
        yield return atkWaitTime;

        atkBool = false;
    }


    /// <summary>
    /// idle 체크
    /// </summary>
    private void ResetIdleBool()
    {

        beforIdleBool = nowIdleBool;
        nowIdleBool = false;
    }



    /// <summary>
    /// idle에 첫 진입인지 확인
    /// </summary>
    /// <returns>첫 진입 여부</returns>
    public bool ChkIdle()
    {

        if (!beforIdleBool && nowIdleBool)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// 걷기 애니메이션과 이동
    /// </summary>
    /// <param name="start"></param>
    public void WalkAnim(bool start)
    {

        agent.enabled = start;
        anim?.ChkAnimation(1, start);
    }

    /// <summary>
    /// 공격 애니메이션
    /// </summary>
    /// <param name="start"></param>
    public void AtkAnim(bool start)
    {

        anim?.ChkAnimation(2, start);
    }

    /// <summary>
    /// 피격 시 실행할 메소드
    /// </summary>
    /// <param name="atk">적의 공격력</param>
    public override void OnDamaged(int atk)
    {

        damagedBool = true;

        // 피격 소리
        myAS.SetSnd(damagedSnd);
        myAS.GetSnd(false);

        // hp 깎는거
        base.OnDamaged(atk);

        // 체력 UI 표시
        float hp = (float)nowHp / status.Hp;
        StatsUI.instance.SetEnemyHp(hp);

        // 페이즈 체크
        if (nowHp < phaseHp)
        {

            phase = Phase.second;
        }
        else
        {

            phase = Phase.first;
        }

        // 데미지 수치 띄우기
        if (damagedText != null)
        {

            GameObject obj = Instantiate(damagedText, transform);
            obj.GetComponent<DamageScript>()?.SetTxt(atk.ToString());
        }

        // 사망 체크
        ChkDead();
    }

    /// <summary>
    /// 사망
    /// </summary>
    protected override void Dead()
    {
        // 상태 변경
        base.Dead();
            
        // 승리
        GameManager.instance.ChkWin();
        
        // 모든 코루틴 중지
        StopAllCoroutines();
    }

    /// <summary>
    /// 무기 활성화 및 모션
    /// </summary>
    public void ActiveWeapon()
    {

        // 공격 상태가 아닌 경우
        if (!atkBool) 
        {
           
            WalkAnim(false);
            StartCoroutine(Attack());
            AtkAnim(true);
        }
    }

    /// <summary>
    /// 공격 콜라이더 충돌 시 활성화될 메소드
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
    protected override void Attack(object sender, Collider other)
    {

        // 데미지 주기
        other.GetComponent<Stat>().OnDamaged(status.Atk);
        
        // 공격 범위 끄기
        atkZone.SetActive(false);
        base.Attack(sender, other);
    }

    /// <summary>
    /// 체력 회복해야 할지 체크
    /// </summary>
    /// <returns></returns>
    public bool ChkHeal()
    {

        if (NowHp < status.Hp)
        {

            return true;
        }

        return false;
    }
}

