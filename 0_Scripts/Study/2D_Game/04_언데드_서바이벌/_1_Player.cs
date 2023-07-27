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

        // GetAxis로하면 미끄러지는 현상이 있다
        // Raw로 붙이면 -1, 0, 1로 된다
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {

        // 힘을 준다
        // rigid.AddForce(inputVec);

        // 속도 제어
        // rigid.velocity = inputVec;

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        // 위치 이동
        rigid.MovePosition(rigid.position + nextVec);  // rigid의 위치 + 이동할 방향
    }


}
