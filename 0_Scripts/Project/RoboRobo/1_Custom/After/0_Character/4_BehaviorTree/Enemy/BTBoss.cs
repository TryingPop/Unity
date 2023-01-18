using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTBoss : MonoBehaviour
{

    private static Transform playerTrans;

    [SerializeField] private GameObject[] missiles;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform targetTrans;

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private string targetTag;

    private int _nowHp;
    public int maxHp;

    public int nowHp { get { return _nowHp; } }


    // 추적
    private float _chaseRadius;
    private float _chaseAngle;

    public float chaseRadius { get { return _chaseRadius; } }
    public float chaseAngle { get { return _chaseAngle; } }


    // 밀리 공격
    private int _meleeAtk;
    private float _meleeAtkRadius;
    private float _meleeAtkAngle;

    public int meleeAtk { get { return _meleeAtk; } }
    public float meleeRadius { get { return _meleeAtkRadius; } }
    public float meleeAngle { get { return _meleeAtkAngle; } }


    // 원거리 공격
    private int _rangeAtk;
    private float _rangeAtkRadius;
    private float _rangeAtkAngle;
    public int bulletNum;

    public int rangeAtk { get { return _rangeAtk; } }
    public float rangeRadius { get { return _rangeAtkRadius; } }
    public float rangeAngle { get { return _rangeAtkAngle; } }

    private float phaseHp;


    private Node topNode;

    private void Awake()
    {
        if (playerTrans == null) playerTrans = GameObject.FindWithTag("Player").transform;
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        SetHp();
        ConstructBehaviorTree();
    }


    private void SetHp()
    {

        _nowHp = maxHp;
    }

    private void ConstructBehaviorTree()
    {
        
        FindNode meleeFind = new FindNode(meleeRadius, meleeAngle, this.transform, ref targetTrans, targetLayer, obstacleLayer, targetTag);
        // MeleeAtkNode meleeAtk = new MeleeAtkNode();

        FindNode chaseFind = new FindNode(chaseRadius, chaseAngle, this.transform, ref targetTrans, targetLayer, obstacleLayer, targetTag);
        ChaseNode chaseTarget = new ChaseNode(targetTrans, agent);

        HealthNode chkPhase = new HealthNode(this, phaseHp);
        
        FindNode rangeFind = new FindNode(rangeRadius, rangeAngle, this.transform, ref targetTrans, targetLayer, obstacleLayer, targetTag);
        RangeAtkNode rangeAtkNode = new RangeAtkNode(transform, missiles, rangeAtk, ref bulletNum, targetTag);

        Sequence melee = new Sequence(new List<Node> { meleeFind,  });
        Sequence range = new Sequence(new List<Node> { rangeFind, rangeAtkNode });
        Sequence chase = new Sequence(new List<Node> { chaseFind, chaseTarget });

        // Selector action1 = new Selector(new List<Node> { meleeAtk, chase});
        // Selector action2 = new Selector(new List<Node> { range, meleeAtk, new ChaseNode(playerTrans, agent) });

        // Sequence phase2 = new Sequence(new List<Node> { chkPhase, action2 });

        // topNode = new Selector(new List<Node> { phase2, action1 });
    }
}
