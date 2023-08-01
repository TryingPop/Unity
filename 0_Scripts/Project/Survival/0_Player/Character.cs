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

    private Camera cam;

    private Vector3 destination;

    public LayerMask moveLayer;

    public bool isMove { get; protected set; }

    protected void Awake()
    {

        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();
        myRigid = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    protected void Update()
    {

        // 마우스를 이용한 이동
        if (Input.GetMouseButtonDown(1))
        {

            // 마우스 포지션을 기준으로 레이를 쏜다
            // 방향은 카메라가 바라보는 방향
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, moveLayer))
            {

                // 지면과 충돌한 경우 해당 지점으로 이동
                destination = hit.point;
                myAgent.SetDestination(destination);
            }
        }

        // 강제 멈춤
        if (Input.GetKeyDown(KeyCode.S))
        {

            MoveStop();
        }

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
                                                                    // 해당 상태 탈출 시 순간이동 한다
        }
    }



    protected void FixedUpdate()
    {
        

    }

    public void Move()
    {

        if (Vector3.Distance(destination, transform.position) < 1f)
        {

            MoveStop();
        }
    }

    public void MoveStop()
    {

        myAgent.SetDestination(transform.position);
    }
}
