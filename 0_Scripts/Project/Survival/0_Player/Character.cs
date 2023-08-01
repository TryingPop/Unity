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

            // ���콺 �������� �������� ���̸� ���
            // ������ ī�޶� �ٶ󺸴� ����
            // �� ��� �� 
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

            // destination = cam.ScreenToWorldPoint(Input.mousePosition);   // ���콺�� ���� ī�޶� �������� World ��ǥ ��ġ�� ��Ÿ����
            // destination = cam.ScreenToViewportPoint(Input.mousePosition);   // ī�޶��� Viewport ��ǥ�� ǥ���ȴ� Viewport�� (0, 0) ~ (1, 1) ���� ���´�
            Debug.Log(destination);
            myAgent.isStopped = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {


            myAgent.isStopped = isMove;         // isStopped �� ���� �� destination�� �״�� �־� �ٽ� Ȱ��ȭ�Ǹ� �ش� ��ġ�� ����!
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
