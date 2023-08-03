using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class _1_Player : MonoBehaviour
{

    public Vector2 inputVec;
    private Rigidbody2D rigid;
    private SpriteRenderer spriter;
    private Animator anim;

    public _10_Scanner scanner;

    public float speed;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        scanner = GetComponent<_10_Scanner>();
    }

    private void Update()
    {

        // GetAxis���ϸ� �̲������� ������ �ִ�
        // Raw�� ���̸� -1, 0, 1�� �ȴ�
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {

        // ���� �ش�
        // rigid.AddForce(inputVec);

        // �ӵ� ����
        // rigid.velocity = inputVec;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // ��ġ �̵�
        rigid.MovePosition(rigid.position + nextVec);  // rigid�� ��ġ + �̵��� ����
    }

    /*
    // ��ǲ �ý����� �̿��� �̵�
    // �̷� �͵� �ִٰ� ������ �ɰ� ����
    private void OnMove(InputValue value)
    {

        inputVec = value.Get<Vector2>();
    }
    */

    private void LateUpdate()
    {

        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {

            spriter.flipX = inputVec.x < 0;
        }
    }
}
