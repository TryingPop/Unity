using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{

    // ĳ��
    PlayerController playerCtrl;
    bool actionEtcRun = true;

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
            return;                                 // ���� �� �ٷ� ������ �� ���� ó���� ������Ų��
        }

        // ����
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") 
            || Input.GetButtonDown("Fire3"))
        {

            if (Input.GetAxisRaw("Vertical") < 0.5f)
            {

                playerCtrl.ActionAttack();
            }
            else 
            {

                playerCtrl.ActionAttackJump();
            }
            
        }

        // ���� ���ų� ��ο� ����
        if (Input.GetAxisRaw("Vertical") > 0.7f)
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

