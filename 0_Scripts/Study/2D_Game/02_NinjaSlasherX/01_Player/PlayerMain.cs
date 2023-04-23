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
        if (playerCtrl.activeSts)
        {

            return;
        }

        // 패드 처리
        float joyMv = Input.GetAxis("Horizontal");
        playerCtrl.ActionMove(joyMv);

        // 점프
        if (Input.GetButtonDown("Jump"))
        {

            playerCtrl.ActionJump();
        }
    }
}
