using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class _1_Player : MonoBehaviour
{

    private Vector2 inputVec;
    private Rigidbody2D rigid;

    [SerializeField] private float speed;

    private void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();
    }

    /*
    // InputSystem ���� ��ü
    // ������ �ʴ´�
    private void Update()
    {

        // GetAxis���ϸ� �̲������� ������ �ִ�
        // Raw�� ���̸� -1, 0, 1�� �ȴ�
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
    */

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

    /// <summary>
    /// ��ǲ �ý����� �̿��� �̵�
    /// </summary>
    /// <param name="value">Input System���� ������ value</param>
    private void OnMove(InputValue value)
    {

        inputVec = value.Get<Vector2>();
    }
}
