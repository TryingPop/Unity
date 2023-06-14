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
        // 교재에서는 Time.deltaTime이라 되어져 있는데, fixedUpdate 연산방법에 의해
        // 컴퓨터 성능이 좋으면 실제보다 느린 이동속도를 내고 컴퓨터 성능이 안좋으면 빠른 속도를 내기에 fixedDeltaTime으로 수정

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