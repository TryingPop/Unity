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
    // 테스트용도
    protected void Update()
    {
        
        // 회전을 막는다
        if (Input.GetKeyDown(KeyCode.A))
        {

            myAgent.updateRotation = !myAgent.updateRotation;
        }
        
        // 이동을 막는데 해제하면 에이전트로 이동한다
        if (Input.GetKeyDown(KeyCode.Q))
        {

            myAgent.updatePosition = !myAgent.updatePosition;       // 캐릭터만 이동안할 뿐 에이전트는 이동한다 그래서
        }
    }
    */


    protected virtual void FixedUpdate()
    {

        Move();
    }

    /// <summary>
    /// 목적지 설정 및 이동
    /// </summary>
    /// <param name="_destination">목적지</param>
    public virtual void SetDestination(Vector3 _destination)
    {

        destination = _destination;

        myAgent.SetDestination(destination);

        if (pathRoot != null) StopCoroutine(pathRoot);
        pathRoot = StartCoroutine(DrawPath());
        point.transform.position = destination;
    }

    /// <summary>
    /// 목적지로 이동한다
    /// </summary>
    public virtual void Move()
    {

        // if (Vector3.Distance(destination, transform.position) < 1f)
        if (myAgent.remainingDistance < 0.1f)     // 내부에서 제공하는 길이
        {

            if (pathRoot != null)
                StopCoroutine(pathRoot);

            myLineRenderer.enabled = false;

            // 이동 모션
            myAnimator.SetFloat("Move", 0f);
        }
        else
        {

            if (isRun)
            {

                // 달리기인 경우
                myAnimator.SetFloat("Move", 1.0f);
            }
            else
            {

                // 걷는 경우
                myAnimator.SetFloat("Move", 0.5f);
            }
        }
    }

    /// <summary>
    /// 제자리 멈춤
    /// </summary>
    public virtual void MoveStop()
    {

        myAgent.SetDestination(transform.position);
        myAnimator.SetFloat("Move", 0f);
    }

    /// <summary>
    /// 경로 그리기
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