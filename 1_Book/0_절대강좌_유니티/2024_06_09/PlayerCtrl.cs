using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    // 미리 변수에 담아두고 해당 변수에 접근하는 방식이 미세하지만 빠르다
    // 캐시처리란 스크립트에서 접근해야 할 컴포넌트를 awake 함수나 Start 함수에서
    // 미리 변수에 할당한 후에 그 변수를 통해 접근하는 것을 말한다
    private Transform tr;

    public float moveSpeed = 10.0f;     // 이동 속도
    public float turnSpeed = 80.0f;     // 회전 속도의 변수


    private void Start()
    {

        tr = GetComponent<Transform>();
    }


    void Update()
    {

        float h = Input.GetAxis("Horizontal");      // -1.0f ~ 1.0f
        float v = Input.GetAxis("Vertical");        // -1.0f ~ 1.0f
        float r = Input.GetAxis("Mouse X");

        // Debug.Log("h = " + h);
        // Debug.Log("v = " + v);

        // Transform 컴포넌트의 위치를변경
        // transform.position += new Vector3(0, 0, 1);

        // 정규화 벡터를 사용한 코드
        // transform.position += Vector3.forward * 1;
        // tr.Translate(Vector3.forward * 1.0f);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * Time.deltaTime * v * moveSpeed);

        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);
    }
}
