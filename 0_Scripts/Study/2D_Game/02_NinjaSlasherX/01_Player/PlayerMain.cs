using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{

    // 캐시
    PlayerController playerCtrl;

    // 코드 (MonoBehaviour 기본 기능 구현)
    private void Awake()
    {

        playerCtrl = GetComponent<PlayerController>();
    }


    void Update()
    {
        
        // 조작이 가능한가?
        if (!playerCtrl.activeSts)
        {

            return;
        }

        // 패드 처리
        float joyMv = Input.GetAxis("Horizontal");  // 좌우 키 입력 여부 확인
        playerCtrl.ActionMove(joyMv);               // 이동 변수 세팅 및 애니메이션 설정

        // 점프
        if (Input.GetButtonDown("Jump"))
        {

            playerCtrl.ActionJump();                // 점프 실행
        }
    }
}

