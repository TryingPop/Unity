using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{

    // ĳ��
    PlayerController playerCtrl;
    zFoxVirtualPad vpad;
    
    bool actionEtcRun = true;

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    private void Awake()
    {

        playerCtrl = GetComponent<PlayerController>();
        vpad = GameObject.FindObjectOfType<zFoxVirtualPad>();
    }


    void Update()
    {

        // ������ �����Ѱ�?
        if (!playerCtrl.activeSts)
        {

            return;
        }

        // �����е�
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

        // �̵�
        float joyMv = Input.GetAxis("Horizontal");  // �¿� Ű �Է� ���� Ȯ��
        joyMv = Mathf.Pow(Mathf.Abs(joyMv), 3.0f) * Mathf.Sign(joyMv);  // joyMv�� ���밪�� 3������ �� �� ��ȣ�� �����ش�

        float vpadMv = vpad_horizontal;
        vpadMv = Mathf.Pow(Mathf.Abs(vpadMv), 1.5f) * Mathf.Sign(vpadMv);   // vpadMv�� ���밪�� 1.5������ �� �� ��ȣ�� vpad�� �����ϰ�
        
        playerCtrl.ActionMove(joyMv + vpadMv);               // �̵� ���� ���� �� �ִϸ��̼� ����

        // ����
        if (Input.GetButtonDown("Jump") || vpad_btnA = zFOXVPAD_BUTTON.DOWN)
        {

            playerCtrl.ActionJump();                // ���� ����
            return;                                 // ���� �� �ٷ� ������ �� ���� ó���� ������Ų��
        }

        // ����
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

        // ���� ���ų� ��ο� ����
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

