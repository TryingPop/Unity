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

        // ���콺�� �̿��� �̵�
        if (Input.GetMouseButtonDown(1))
        {

            // ���콺 �������� �������� ���̸� ���
            // ������ ī�޶� �ٶ󺸴� ����
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, moveLayer))
            {

                // ����� �浹�� ��� �ش� �������� �̵�
                destination = hit.point;
                myAgent.SetDestination(destination);
            }
        }

        // ���� ����
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

            myAgent.updatePosition = !myAgent.updatePosition;       // ĳ���͸� �̵����� �� ������Ʈ�� �̵��Ѵ� �׷���
                                                                    // �ش� ���� Ż�� �� �����̵� �Ѵ�
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
