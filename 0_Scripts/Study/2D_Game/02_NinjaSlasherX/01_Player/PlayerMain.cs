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
        if (!playerCtrl.activeSts)
        {

            return;
        }

        // �е� ó��
        float joyMv = Input.GetAxis("Horizontal");  // �¿� Ű �Է� ���� Ȯ��
        playerCtrl.ActionMove(joyMv);               // �̵� ���� ���� �� �ִϸ��̼� ����

        // ����
        if (Input.GetButtonDown("Jump"))
        {

            playerCtrl.ActionJump();                // ���� ����
        }
    }
}

