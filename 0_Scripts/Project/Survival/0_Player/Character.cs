using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    [SerializeField] 
    private MeshRenderer point;

    public bool isRun;
    public float walkSpeed;
    public float runSpeed;


    protected void Awake()
    {

        myAnimator = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();
        myRigid = GetComponent<Rigidbody>();
        cam = Camera.main;

        myLineRenderer = GetComponent<LineRenderer>();

        myLineRenderer.startWidth = 0.1f;
        myLineRenderer.endWidth = 0.1f;
        myLineRenderer.material.color = Color.red;

        myAgent.speed = walkSpeed;
    }

    /*
    // �׽�Ʈ�뵵
    protected void Update()
    {
        
        // ȸ���� ���´�
        if (Input.GetKeyDown(KeyCode.A))
        {

            myAgent.updateRotation = !myAgent.updateRotation;
        }
        
        // �̵��� ���µ� �����ϸ� ������Ʈ�� �̵��Ѵ�
        if (Input.GetKeyDown(KeyCode.Q))
        {

            myAgent.updatePosition = !myAgent.updatePosition;       // ĳ���͸� �̵����� �� ������Ʈ�� �̵��Ѵ� �׷���
        }
    }
    */


    protected void FixedUpdate()
    {

        Move();
    }

    /// <summary>
    /// ������ ���� �� �̵�
    /// </summary>
    /// <param name="_destination">������</param>
    public void SetDestination(Vector3 _destination)
    {

        myAgent.velocity = Vector3.zero;

        destination = _destination;

        myAgent.SetDestination(destination);

        if (pathRoot != null) StopCoroutine(pathRoot);
        pathRoot = StartCoroutine(DrawPath());
        point.transform.position = destination;
    }

    /// <summary>
    /// �������� �̵��Ѵ�
    /// </summary>
    public void Move()
    {

        // if (Vector3.Distance(destination, transform.position) < 1f)
        if (myAgent.remainingDistance < 1f)     // ���ο��� �����ϴ� ����
        {

            MoveStop();
            if (pathRoot != null)
                StopCoroutine(pathRoot);

            myLineRenderer.enabled = false;
            point.enabled = false;

            // �̵� ���
            myAnimator.SetFloat("Move", 0f);
        }
        else
        {

            if (isRun)
            {

                // �޸����� ���
                myAnimator.SetFloat("Move", 1.0f);
            }
            else
            {

                // �ȴ� ���
                myAnimator.SetFloat("Move", 0.5f);
            }
        }
    }

    /// <summary>
    /// ���ڸ� ����
    /// </summary>
    public void MoveStop()
    {

        myAgent.velocity = Vector3.zero;
        myAgent.SetDestination(transform.position);
    }

    /// <summary>
    /// ��� �׸���
    /// </summary>
    /// <returns></returns>
    private IEnumerator DrawPath()
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

    public void SetRun()
    {

        isRun = !isRun;
        if (isRun)
        {

            myAgent.speed = runSpeed;
        }
        else
        {

            myAgent.speed = walkSpeed;
        }
    }
}
