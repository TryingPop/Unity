using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBoss : MonoBehaviour
{

    public enum Phase { first, second }

    public static Transform playerTrans;

    public MeleeWeapon weapon;

    public GameObject[] missiles;

    public NavMeshAgent agent;
    public Transform targetTrans;

    public LayerMask targetLayer;
    public LayerMask obstacleLayer;

    public Phase phase;

    public string targetTag;

    private int _nowHp;
    public int maxHp;

    public int nowHp { get { return _nowHp; } set { _nowHp = value; } }

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
        if (playerTrans == null) playerTrans = GameObject.FindWithTag("Player").transform;

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

        IdleNode idleNode = new IdleNode();

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

        Selector action1 = new Selector(new List<Node> { melee, chase});
        Selector action2 = new Selector(new List<Node> {  melee, range, new ChaseNode(this) });

        Sequence phase2 = new Sequence(new List<Node> { chkPhase, action2 });

        topNode = new Selector(new List<Node> { phase2, action1 });
    }

    private IEnumerator Action()
    {
        while (true)
        {

            topNode.Evaluate();
            yield return new WaitForSeconds(0.3f);
        }
    }
}

