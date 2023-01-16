using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTEnemy : MonoBehaviour
{

    [SerializeField] private float maxHp;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform playerTrans;
    [SerializeField] private Cover[] availableCovers;


    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;


    private float _nowHp;

    public float nowHp
    {
        get { return _nowHp; }
        set { _nowHp = Mathf.Clamp(value, 0, maxHp); }
    }

    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        material= GetComponent<Material>();
    }


    void Start()
    {

        nowHp = maxHp;

        ConstructBehaviorTree();
    }

    void Update()
    {
        topNode.Evaluate();

        if (topNode.NodeState == NodeState.FAILURE)
        {

            SetColor(Color.red);
        }
        nowHp += Time.deltaTime * healthRestoreRate;
    }

    private void ConstructBehaviorTree()
    {

        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTrans, this);
        GoToCoverNode gotoCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTrans, transform);
        ChaseNode chaseNode = new ChaseNode(playerTrans, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTrans, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTrans, transform);
        ShootNode shootNode = new ShootNode(agent, this);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode, });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, gotoCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseNode });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });
    }

    public void SetColor(Color color)
    {

        material.color = color;
    }

    public void SetBestCover(Transform bestCoverSpot)
    {

        this.bestCoverSpot = bestCoverSpot;
    }

    internal Transform GetBestCover()
    {

        return bestCoverSpot;
    }
}
