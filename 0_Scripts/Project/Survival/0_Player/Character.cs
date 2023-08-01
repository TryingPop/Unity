using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovable
{

    protected Rigidbody myRigid;
    protected Animator myAnimator;
    protected Collider myCollider;
    protected NavMeshAgent myAgent;

    private Camera cam;

    private Vector3 destination;

    public LayerMask moveLayer;

    public bool isMove { get; set; }

    protected void Awake()
    {

        myRigid = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<Collider>();
        myAgent = GetComponent<NavMeshAgent>();

        cam = Camera.main;
    }

    protected void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {

            // 마우스 포지션을 기준으로 레이를 쏜다
            // 방향은 카메라가 바라보는 방향
            // 이 방법 시 
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 5000f, moveLayer))
            {

                destination = hit.point;
                myAgent.SetDestination(destination);
            }
            else
            {

                destination = transform.position;
            }

            // destination = cam.ScreenToWorldPoint(Input.mousePosition);   // 마우스가 현재 카메라를 기준으로 World 좌표 위치를 나타낸다
            // destination = cam.ScreenToViewportPoint(Input.mousePosition);   // 카메라의 Viewport 좌표로 표현된다 Viewport는 (0, 0) ~ (1, 1) 값을 갖는다
            Debug.Log(destination);
            myAgent.isStopped = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {


            myAgent.isStopped = isMove;         // isStopped 로 멈출 시 destination이 그대로 있어 다시 활성화되면 해당 위치로 간다!
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            isMove = !isMove;
        }
    }



    protected void FixedUpdate()
    {
        
        Move();
    }

    public void Move()
    {

        if (Vector3.Distance(destination, transform.position) < 0.5f)
        {

            myAgent.isStopped = true;
        }
    }
}
