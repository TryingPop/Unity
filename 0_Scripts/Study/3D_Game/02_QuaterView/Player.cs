using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 20f;
    public float jumpPower = 15;

    private float hAxis;    // ĳ���� �̵� �¿� ���� Ű
    private float vAxis;    // ĳ���� �̵� ���� ���� Ű

    private bool wDown;     // ĳ���� �ȱ� ���� ����Ʈ Ű �Է�
    private bool jDown;     // ĳ���� ���� �����̽��� Ű

    private bool isJump;    // ���� ���� ���� üũ
    private bool isDodge;   // ���� ȸ�� ���� üũ

    private Vector3 moveVec;    // �̵��� ����3
    private Vector3 dodgeVec;   // ������ ����3
    private Animator anim;
    public Rigidbody rigid;


    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    private void GetInput()
    {

        // GetAxis�� �ε巴�� �޾ƿ��� �ݸ�,   -1f ~ 1f
        // GetAxisRaw�� ��� �޾ƿ´�          -1, 0, 1�� �޾ƿ´�
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // ProjectSettings�� InputManager���� Walk�� �߰�������Ѵ�
        wDown = Input.GetButton("Walk");        // ���⼭�� left shift
        jDown = Input.GetButtonDown("Jump");    // ���⼭�� �����̽� ��

    }

    private void Move()
    {

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;      // ũ�� 1�� �����

        if (isDodge)
        {

            moveVec = dodgeVec;
        }
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    private void Turn()
    {

        // �ش� ��ǥ�� �ٶ󺻴�
        // �׷��� ���� ��ġ + moveVec�� ���ش�
        // �̵� ���̳� �̵� �ĳ� �ڵ�� �Ȱ���
        // ���� �Ʒ��� ���̺����� �Ҵ��� ���
        // Vector3 destination = transform.position + moveVec
        // LookAt�� �̵� ������ �����Ѵ�
        transform.LookAt(transform.position + moveVec);
    }

    private void Jump()
    {

        if (jDown && !isJump && moveVec == Vector3.zero && !isDodge)
        {

            // �������� ���� �ִ� ����� Impulse
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    private void Dodge()
    {

        if (jDown && !isJump && moveVec != Vector3.zero && !isDodge)
        {

            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            // �ش� �̸��� �޼ҵ带 0.4�� �ڿ� ����
            Invoke("DodgeOut", 0.4f);
        }
    }

    private void DodgeOut()
    {

        isDodge = false;
        speed *= 0.5f;
    }

    // ���� ����
    private void OnCollisionEnter(Collision collision)
    {
        
        // �±׷� ���� ����
        // Floor �±� ���� �� ���鿡 Floor �±� �߰�
        if (collision.gameObject.tag == "Floor")
        {

            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
