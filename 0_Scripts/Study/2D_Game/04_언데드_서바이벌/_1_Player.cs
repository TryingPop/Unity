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
    // InputSystem 으로 대체
    // 사용되지 않는다
    private void Update()
    {

        // GetAxis로하면 미끄러지는 현상이 있다
        // Raw로 붙이면 -1, 0, 1로 된다
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
    */

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

    /// <summary>
    /// 인풋 시스템을 이용한 이동
    /// </summary>
    /// <param name="value">Input System에서 설정한 value</param>
    private void OnMove(InputValue value)
    {

        inputVec = value.Get<Vector2>();
    }
}
