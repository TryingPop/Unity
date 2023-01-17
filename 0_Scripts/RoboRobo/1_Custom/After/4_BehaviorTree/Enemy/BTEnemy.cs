using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        material= GetComponent<MeshRenderer>().material;
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
            agent.isStopped = true;
        }
        
        nowHp += Time.deltaTime * healthRestoreRate;
    }

    /// <summary>
    /// 노드 그려서 해석해보기
    /// </summary>
    private void ConstructBehaviorTree()
    {

        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(availableCovers, playerTrans, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTrans, transform);
        ChaseNode chaseNode = new ChaseNode(playerTrans, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTrans, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTrans, transform);
        ShootNode shootNode = new ShootNode(agent, this);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode, });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvailableNode, goToCoverNode });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, goToCoverSequence });
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

    internal Transform GetBestCoverSpot()
    {

        return bestCoverSpot;
    }
}
