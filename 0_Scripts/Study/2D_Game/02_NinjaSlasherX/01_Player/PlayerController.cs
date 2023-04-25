using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacterController
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public float initHpMax = 20.0f;
    [Range(0.1f, 100.0f)] public float initSpeed = 12.0f;

    // ���� �Ķ����
    int jumpCount = 0;
    bool breakEnabled = true;
    float groundFriction = 0.0f;

    // �ڵ� (MonoBehaviour �⺻ ��� ����)
    protected override void Awake()
    {
        base.Awake();

        // �Ķ���� �ʱ�ȭ
        speed = initSpeed;
        SetHp(initHpMax, initHpMax);
    }

    protected override void FixedUpdateCharacter()
    {

        // ���� �˻�
        if (jumped)     // ���� ���� ���
        {

            // �� ���� ���鿡 ���� �ʰ� ���� ���鿡 ���� ��� ���� ���� ����
            // ���� ���鿡 �ְ� �������� 1�ʰ� �����ٸ� ������ ������ ����
            if ((grounded && !groundedPrev) || (grounded && Time.fixedTime > jumpStartTime +1.0f))
            {

                animator.SetTrigger("Idle");    // Ż�� �ִϸ��̼�
                jumped = false;
                jumpCount = 0;
            }
        }
        // if (!jumped)
        else            // �ܴ̿� ���鿡 �����Ƿ� �׻� 0
        {

            jumpCount = 0;
        }
        

        // ĳ���� ����
        transform.localScale = new Vector3(     // ũ�� �������� ĳ���� �ٶ󺸴� ���� ����
            basScaleX * dir, transform.localScale.y, transform.localScale.z);

        // ���� ���߿� ���� �̵� ����
        if (jumped && grounded)         // ���� ���� ��
        {

            if (breakEnabled)
            {

                breakEnabled = false;   
                speedVx *= 0.9f;        // ���� �ӵ��� 10% ����
            }
        }

        // �̵� ����(����) ó��
        if (breakEnabled)               // ����� ���� ��ȯ�̳� Ű�Է��� ���� ��
        {

            speedVx *= groundFriction;  // �ٷ� ����
                                        // velocity�� �̵� �����Ƿ� ���� ���� ������ �����ؾ��Ѵ�
                                        // ���Ǳ��� ǥ���Ϸ��� 0.5 ���� 1 ���� ��� ������ �ϸ�ȴ�
        }

        // ī�޶�
        Camera.main.transform.position = transform.position - Vector3.forward;  // ���� ī�޶� �±׷� ��ϵ� ī�޶��� ��ġ�� ĳ���Ϳ� -1 ��ǥ ��ġ
    }

    // �ڵ� (�⺻ �׼�)
    public override void ActionMove(float n)
    {

        if (!activeSts)
        {

            return;
        }

        // �ʱ�ȭ
        float dirOld = dir;
        breakEnabled = false;

        // �ִϸ��̼� ����
        float moveSpeed = Mathf.Clamp(Mathf.Abs(n), -1.0f, 1.0f);   // -1, 1 ������ �� ����
        animator.SetFloat("MoveSpeed", moveSpeed);  // movespeed ��ġ �� ������ �޸���� �ȱ� ǥ��
        // animator.speed = 1.0f + moveSpeed;       // ���� �ȴ� ����� ���� �ٴ°� ǥ��?

        // �̵� �˻�
        if (n != 0.0f)
        {

            // �̵�
            dir = Mathf.Sign(n);
            moveSpeed = (moveSpeed < 0.5f) ? (moveSpeed * (1.0f / 0.5f)) : 1.0f;    // ���� �� �ӵ��� �ִ� 25%��, �޸� ���� 100% ����
            speedVx = initSpeed * moveSpeed * dir;
        }
        else
        {

            // �̵� ����
            breakEnabled = true;
        }

        // �� �������� ���ƺ��� �˻�
        if(dirOld != dir)
        {

            // ���� ��ȯ Ȯ��
            breakEnabled = true;
        }
    }

    public void ActionJump()
    {

        switch (jumpCount)
        {

            case 0:     // ù ������ ���
                if (grounded)
                {

                    animator.SetTrigger("Jump");                // �ִϸ��̼� ����
                    rigidbody2D.velocity = Vector2.up * 30.0f;  // ���� 30 �ӵ� �̵�
                    jumpStartTime = Time.fixedTime;             // ���� ���� �ð� ����
                    jumped = true;                              // ���� ���� 
                    jumpCount++;                                // ���� ��� �������� Ȯ��
                }
                break;

            case 1:     // �̴� ���� �ϴ��� üũ
                if (!grounded)
                {

                    animator.Play("Player_Jump", 0, 0.0f);       // ���� �ִϸ��̼� ����
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20.0f);  // ���� 20��ŭ��
                    jumped = true;                              // ���� ���� true
                    jumpCount++;                                // ���� ī��Ʈ ��
                }
                break;
        }

    }

    public void ActionAttack()
    {

        animator.SetTrigger("Attack_A");
    }
}
