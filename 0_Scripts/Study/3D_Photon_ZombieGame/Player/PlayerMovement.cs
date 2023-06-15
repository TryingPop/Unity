using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 입력에 따라 플레이어 캐릭터를 움직이는 스크립트
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;        // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f;    // 좌우 회전 속도

    private PlayerInput playerInput;    // 플레이어 입력을 알려주는 컴포넌트

    private Rigidbody playerRigidbody;  // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator;    // 플레이어 캐릭터의 애니메이터

    private void Start()
    {
        
        // 사용할 컴포넌트들의 참조 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 진행
        // 회전 실행
        Rotate();

        // 움직임 실행
        Move();

        // 입력값에 따라 애닡메이터의 Move 파라미터값 변경
        playerAnimator.SetFloat("Move", playerInput.move);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move()
    {

        Vector3 moveDistance =
            playerInput.move * transform.forward * moveSpeed * Time.fixedDeltaTime;
        // 유니티에서는 개발자의 편의를 위해 fixedUpdate안에서는 Time.deltaTime은 Time.fixedDeltaTime의 값을 이용한다
        // 그래도 여기서는 fixedUpdate에서 쓴다는 것을 표현하기 위해 fixedDeltaTime으로 표현 

        // 리지드바디를 이용해 게임 오브젝트 위치 변경
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate()
    {

        // 상대적으로 회전할 수치 계산
        float turn = playerInput.rotate * rotateSpeed * Time.fixedDeltaTime;

        // 리지드바디를 이용해 게임 오브젝트 회전 변경
        playerRigidbody.rotation =
            playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
        // 현재 회전 상태에서 더 회전하고 싶을 때는 쿼터니언 곱을 주로 이용한다!
    }
}