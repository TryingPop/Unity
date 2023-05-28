using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{

    // 캐시
    PlayerController playerCtrl;
    zFoxVirtualPad vpad;
    
    bool actionEtcRun = true;

    // 코드 (MonoBehaviour 기본 기능 구현)
    private void Awake()
    {

        playerCtrl = GetComponent<PlayerController>();
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
    }


    void Update()
    {

        // 조작이 가능한가?
        if (!playerCtrl.activeSts)
        {

            return;
        }

        // 가상패드
        float vpad_vertical = 0.0f;
        float vpad_horizontal = 0.0f;
        zFOXVPAD_BUTTON vpad_btnA = zFOXVPAD_BUTTON.NON;
        zFOXVPAD_BUTTON vpad_btnB = zFOXVPAD_BUTTON.NON;
        if (vpad != null)
        {

            vpad_vertical = vpad.vertical;
            vpad_horizontal = vpad.horizontal;
            vpad_btnA = vpad.buttonA;
            vpad_btnB = vpad.buttonB;
        }

        // 이동
        float joyMv = Input.GetAxis("Horizontal");  // 좌우 키 입력 여부 확인
        joyMv = Mathf.Pow(Mathf.Abs(joyMv), 3.0f) * Mathf.Sign(joyMv);  // joyMv의 절대값에 3제곱을 한 뒤 부호를 맞춰준다

        float vpadMv = vpad_horizontal;
        vpadMv = Mathf.Pow(Mathf.Abs(vpadMv), 1.5f) * Mathf.Sign(vpadMv);   // vpadMv의 절대값에 1.5제곱을 한 값 부호는 vpad와 동일하게
        
        playerCtrl.ActionMove(joyMv + vpadMv);               // 이동 변수 세팅 및 애니메이션 설정

        // 점프
        if (Input.GetButtonDown("Jump") || vpad_btnA = zFOXVPAD_BUTTON.DOWN)
        {

            playerCtrl.ActionJump();                // 점프 실행
            return;                                 // 점프 후 바로 공격할 수 없게 처리를 정지시킨다
        }

        // 공격
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") 
            || Input.GetButtonDown("Fire3") || vpad_btnB == zFOXVPAD_BUTTON.DOWN)
        {

            if (Input.GetAxisRaw("Vertical") + vpad_vertical < 0.5f)
            {

                playerCtrl.ActionAttack();
            }
            else 
            {

                playerCtrl.ActionAttackJump();
            }
            
        }

        // 문을 열거나 통로에 들어간다
        if (Input.GetAxisRaw("Vertical") + vpad_vertical > 0.7f)
        {
            if (actionEtcRun)
            {

                playerCtrl.ActionEtc();
                actionEtcRun = false;
            }
        }
        else
        {

            actionEtcRun = true;
        }
    }
}

