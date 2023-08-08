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

    protected Camera cam;

    protected Vector3 destination;

    public LayerMask moveLayer;

    protected Coroutine pathRoot;

    [SerializeField] 
    protected MeshRenderer point;

    public bool isRun;
    public float walkSpeed;
    public float runSpeed;

    protected Queue<Vector3> destinations;

    protected virtual void Awake()
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


    protected virtual void FixedUpdate()
    {

        Move();
    }

    /// <summary>
    /// ������ ���� �� �̵�
    /// </summary>
    /// <param name="_destination">������</param>
    public virtual void SetDestination(Vector3 _destination)
    {

        destination = _destination;

        myAgent.SetDestination(destination);

        if (pathRoot != null) StopCoroutine(pathRoot);
        pathRoot = StartCoroutine(DrawPath());
        point.transform.position = destination;
    }

    /// <summary>
    /// �������� �̵��Ѵ�
    /// </summary>
    public virtual void Move()
    {

        // if (Vector3.Distance(destination, transform.position) < 1f)
        if (myAgent.remainingDistance < 0.1f)     // ���ο��� �����ϴ� ����
        {

            if (pathRoot != null)
                StopCoroutine(pathRoot);

            myLineRenderer.enabled = false;

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
    public virtual void MoveStop()
    {

        myAgent.SetDestination(transform.position);
        myAnimator.SetFloat("Move", 0f);
    }

    /// <summary>
    /// ��� �׸���
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DrawPath()
    {

        myLineRenderer.enabled = true;
        point.enabled = true;

        yield return null;

        while (true)
        {

            Vector3[] paths = myAgent.path.corners;
            int cnt = paths.Length;
            myLineRenderer.positionCount = cnt;

            for (int i = 0; i < cnt; i++)
            {

                myLineRenderer.SetPosition(i, paths[i]);
            }

            yield return null;
        }
    }

    public virtual void SetRun(bool _isRun)
    {

        isRun = _isRun;
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