using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{

    // ĳ��
    PlayerController playerCtrl;

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    private void Awake()
    {

        playerCtrl = GetComponent<PlayerController>();
    }


    void Update()
    {
        
        // ������ �����Ѱ�?
        if (playerCtrl.activeSts)
        {

            return;
        }

        // �е� ó��
        float joyMv = Input.GetAxis("Horizontal");
        playerCtrl.ActionMove(joyMv);

        // ����
        if (Input.GetButtonDown("Jump"))
        {

            playerCtrl.ActionJump();
        }
    }
}
