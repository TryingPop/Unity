using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class _1_Player : MonoBehaviour
{

    private Vector2 inputVec;
    private Rigidbody2D rigid;

    [SerializeField] private float speed;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
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


}
