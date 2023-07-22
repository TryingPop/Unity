using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 20f;

    private float hAxis;
    private float vAxis;

    private bool wDown;

    private Vector3 moveVec;

    private Animator anim;

    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {

        // GetAxis�� �ε巴�� �޾ƿ��� �ݸ�,   -1f ~ 1f
        // GetAxisRaw�� ��� �޾ƿ´�          -1, 0, 1�� �޾ƿ´�
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // ProjectSettings�� InputManager���� Walk�� �߰�������Ѵ�
        wDown = Input.GetButton("Walk");        // ���⼭�� left shift

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;      // ũ�� 1�� �����

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        // �ش� ��ǥ�� �ٶ󺻴�
        // �׷��� ���� ��ġ + moveVec�� ���ش�
        // �̵� ���̳� �̵� �ĳ� �ڵ�� �Ȱ���
        // ���� �Ʒ��� ���̺����� �Ҵ��� ���
        // Vector3 destination = transform.position + moveVec
        // LookAt�� �̵� ������ �����Ѵ�
        transform.LookAt(transform.position + moveVec);
    }
}
