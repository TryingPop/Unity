using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBoss : Stat
{

    public enum Phase { first, second }

    public static Transform playerTrans;

    public Transform missileTransform;

    public EnemyAnimation anim;

    public GameObject[] missiles;   // 원거리 공격 투사체
    public GameObject[] summoners;    // 소환수 목록

    [SerializeField] private GameObject atkZone;

    public NavMeshAgent agent;
    public Transform targetTrans;

    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    public Phase phase;

    public string targetTag;

    public bool nowIdleBool;
    private bool beforIdleBool;

    public int NowHp { 
        get 
        { 

            return nowHp; 
        } 
        set 
        { 

            nowHp += value;
            if (nowHp < 0) { nowHp = 0; }
            else if (nowHp > status.Hp) { nowHp = status.Hp; }
        } 
    }

    [SerializeField] private int phaseHp = 6;

    [SerializeField] private RangedStatus chase;
    [SerializeField] private RangedStatus melee;
    [SerializeField] private RangedStatus ranged;

    public int bulletNum = 6;       // 탄약 수

    private Node topNode;

    private void Awake()
    {
        if (playerTrans == null) playerTrans = GameObject.FindWithTag("Player")?.transform;
        if (anim == null) anim = GetComponent<EnemyAnimation>();
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        GetComp();
        
        myWC.Attack += Attack;
        Init();

        ConstructBehaviorTree();
    }

    private void Start()
    {
        StartCoroutine(Action());
    }
    
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

    private IEnumerator Action()
    {
        while (!deadBool)
        {

            ResetIdleBool();
            topNode.Evaluate();
            yield return new WaitForSeconds(0.3f);
        }
    }

    protected override IEnumerator Attack()
    {

        atkBool = true;
        atkZone.SetActive(true);
        // 1초간 공격 범위 보여주기
        yield return atkWaitTime;
        yield return atkWaitTime;

        myWC.AtkColActive(true);
        
        // 1초동안 데미지 콜라이더 활성화
        yield return atkWaitTime;
        yield return atkWaitTime;
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

    public void WalkAnim(bool start)
    {

        agent.enabled = start;
        anim?.ChkAnimation(1, start);
    }

    public void AtkAnim(bool start)
    {

        anim?.ChkAnimation(2, start);
    }

    public override void OnDamaged(int atk)
    {

        base.OnDamaged(atk);

        if (nowHp < phaseHp)
        {

            phase = Phase.second;
        }
        else
        {

            phase = Phase.first;
        }

        ChkDead();
    }

    public void ActiveWeapon()
    {

        if (!atkBool) 
        {

            WalkAnim(false);
            StartCoroutine(Attack());
            AtkAnim(true);
        }
    }

    protected override void Attack(object sender, Collider other)
    {

        other.GetComponent<Stat>().OnDamaged(status.Atk);
        atkZone.SetActive(false);
        base.Attack(sender, other);
    }

    public bool ChkHeal()
    {

        if (NowHp < status.Hp)
        {

            return true;
        }

        return false;
    }
}

