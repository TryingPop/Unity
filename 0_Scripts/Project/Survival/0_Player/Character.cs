using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovable
{

    protected Animator myAnimator;
    protected Collider myCollider;
    protected NavMeshAgent myAgent;
    protected Rigidbody myRigid;
    protected LineRenderer myLineRenderer;

    private Camera cam;

    private Vector3 destination;

    public LayerMask moveLayer;

    private Coroutine pathRoot;

    [SerializeField] private MeshRenderer point;

    public bool isMove { get; protected set; }

    protected void Awake()
    {

        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();
        myRigid = GetComponent<Rigidbody>();
        cam = Camera.main;

        myLineRenderer = GetComponent<LineRenderer>();

        myLineRenderer.startWidth = 0.1f;
        myLineRenderer.endWidth = 0.1f;
        myLineRenderer.material.color = Color.red;
    }

    protected void Update()
    {


        if (Input.GetKeyDown(KeyCode.W))
        {

            isMove = !isMove;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            myAgent.updateRotation = !myAgent.updateRotation;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

            myAgent.updatePosition = !myAgent.updatePosition;       // 캐릭터만 이동안할 뿐 에이전트는 이동한다 그래서
        }
    }



    protected void FixedUpdate()
    {

        Move();
    }

    public void SetDestination(Vector3 _destination)
    {

        destination = _destination;

        myAgent.SetDestination(destination);

        if (pathRoot != null) StopCoroutine(pathRoot);
        pathRoot = StartCoroutine(DrawPath());
        point.transform.position = destination;
    }

    public void Move()
    {

        // if (Vector3.Distance(destination, transform.position) < 1f)
        if (myAgent.remainingDistance < 1f)     // 내부에서 제공하는 길이
        {

            MoveStop();
            if (pathRoot != null)
                StopCoroutine(pathRoot);

            myLineRenderer.enabled = false;
            point.enabled = false;
        }
    }

    public void MoveStop()
    {

        myAgent.SetDestination(transform.position);
    }

    IEnumerator DrawPath()
    {

        myLineRenderer.enabled = true;
        point.enabled = true;

        yield return null;

        while (true)
        {

            int cnt = myAgent.path.corners.Length;
            myLineRenderer.positionCount = cnt;

            for (int i = 0; i < cnt; i++)
            {

                myLineRenderer.SetPosition(i, myAgent.path.corners[i]);
            }

            yield return null;
        }
    }
}
