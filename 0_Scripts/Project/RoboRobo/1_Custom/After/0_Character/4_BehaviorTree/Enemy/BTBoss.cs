using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBoss : MonoBehaviour
{

    public enum Phase { first, second }

    public static Transform playerTrans;

    public MeleeWeapon weapon;
    public EnemyAnimation anim;

    public GameObject[] missiles;   // 원거리 공격 투사체
    public GameObject[] summoners;    // 소환수 목록

    public NavMeshAgent agent;
    public Transform targetTrans;

    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    public Phase phase;

    public string targetTag;

    private int _nowHp;
    public int maxHp;

    private bool deadBool;

    public bool nowIdleBool;
    private bool beforIdleBool;

    public int nowHp 
    { 
        get { return _nowHp; } 
        set 
        {

            _nowHp = value;
            if (_nowHp < 0)
            {

                _nowHp = 0;
            }
            else if (_nowHp > maxHp)
            {

                _nowHp = maxHp;

                // 사망 
                deadBool = true;
            }
        } 
    }

    [SerializeField] private int phaseHp = 6;

    // 추적
    [SerializeField] private float _chaseRadius = 10;
    [SerializeField] private float _chaseAngle = 90;

    public float chaseRadius { get { return _chaseRadius; } }
    public float chaseAngle { get { return _chaseAngle; } }

    // 밀리 공격
    [SerializeField] private int _meleeAtk;
    [SerializeField] private float _meleeAtkRadius = 3;
    [SerializeField] private float _meleeAtkAngle = 360;
    [SerializeField] private float _meleeAtkTime = 0.3f;
    public int meleeAtk { get { return _meleeAtk; } }
    public float meleeRadius { get { return _meleeAtkRadius; } }
    public float meleeAngle { get { return _meleeAtkAngle; } }
    public float meleeAtkTime { get { return _meleeAtkTime; } }

    // 원거리 공격
    [SerializeField] private int _rangeAtk = 5;
    [SerializeField] private float _rangeAtkRadius = 6;
    [SerializeField] private float _rangeAtkAngle = 60;
    public int bulletNum = 6;

    public int rangeAtk { get { return _rangeAtk; } }
    public float rangeRadius { get { return _rangeAtkRadius; } }
    public float rangeAngle { get { return _rangeAtkAngle; } }

    private Node topNode;

    private void Awake()
    {
        if (playerTrans == null) playerTrans = GameObject.FindWithTag("Player")?.transform;

        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (weapon == null) weapon = GetComponentInChildren<MeleeWeapon>();

        SetHp();
        ConstructBehaviorTree();
    }

    private void Start()
    {
        StartCoroutine(Action());
    }


    private void SetHp()
    {

        _nowHp = maxHp;
    }

    private void ConstructBehaviorTree()
    {

        IdleNode idleNode = new IdleNode(this);

        FindNode meleeFind = new FindNode(this, meleeRadius, meleeAngle);
        MeleeAtkNode meleeAtkNode = new MeleeAtkNode(this, meleeAtk, targetTag, meleeAtkTime);

        FindNode chaseFind = new FindNode(this, chaseRadius, chaseAngle);
        ChaseNode chaseTarget = new ChaseNode(this);

        HealthNode chkPhase = new HealthNode(this, Phase.second);
        
        FindNode rangeFind = new FindNode(this, rangeRadius, rangeAngle);
        RangeAtkNode rangeAtkNode = new RangeAtkNode(this, rangeAtk);

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
        while (true)
        {

            ResetIdleBool();
            topNode.Evaluate();
            yield return new WaitForSeconds(0.3f);
        }
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

    public void OnDamaged(int atk)
    {

        nowHp -= atk;

        if (nowHp < phaseHp)
        {

            phase = Phase.second;
        }
        else
        {

            phase = Phase.first;
        }
    }
}

