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

        // GetAxis는 부드럽게 받아오는 반면,   -1f ~ 1f
        // GetAxisRaw는 즉시 받아온다          -1, 0, 1로 받아온다
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        // ProjectSettings의 InputManager에서 Walk를 추가해줘야한다
        wDown = Input.GetButton("Walk");        // 여기서는 left shift

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;      // 크기 1로 맞춘다

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        // 해당 좌표로 바라본다
        // 그래서 현재 위치 + moveVec을 해준다
        // 이동 전이나 이동 후나 코드는 똑같다
        // 만약 아래와 같이변수를 할당할 경우
        // Vector3 destination = transform.position + moveVec
        // LookAt은 이동 전으로 가야한다
        transform.LookAt(transform.position + moveVec);
    }
}
