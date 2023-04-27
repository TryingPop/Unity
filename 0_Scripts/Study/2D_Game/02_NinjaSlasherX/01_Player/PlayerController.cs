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

    volatile bool atkInputEnabled = false;              // ��Ƽ������ ���α׷����� �����Ϸ��� ���� ���� ó���� �����ϴ� ����ȭ�� ���� �ʴ´ٴ� ��
    volatile bool atkInputNow = false;

    bool breakEnabled = true;
    float groundFriction = 0.0f;

    // �ܺ� �Ķ����
    public readonly static int ANISTS_Idle =
        Animator.StringToHash("Base Layer.Player_Idle");
    public readonly static int ANISTS_Walk =
        Animator.StringToHash("Base Layer.Player_Walk");
    public readonly static int ANISTS_Run =
        Animator.StringToHash("Base Layer.Player_Run");
    public readonly static int ANISTS_Jump =
        Animator.StringToHash("Base Layer.Player_Jump");
    public readonly static int ANISTS_ATTACK_A =
        Animator.StringToHash("Base Layer.Player_ATK_A");
    public readonly static int ANISTS_ATTACK_B =
        Animator.StringToHash("Base Layer.Player_ATK_B");
    public readonly static int ANISTS_ATTACK_C =
        Animator.StringToHash("Base Layer.Player_ATK_C");
    public readonly static int ANISTS_ATTACKJUMP_A =
        Animator.StringToHash("Base Layer.Player_ATKJUMP_A");
    public readonly static int ANISTS_ATTACKJUMP_B =
        Animator.StringToHash("Base Layer.Player_ATKJUMP_B");

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

        // ���� ������Ʈ ��������
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // �ִϸ����Ϳ��� �� ���� �ִ� Default Layer�� ���� �����´�

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
                rigidbody2D.gravityScale = gravityScale;
            }
            if (Time.fixedTime > jumpStartTime + 1.0f)
            {

                if (stateInfo.fullPathHash == ANISTS_Idle ||
                    stateInfo.fullPathHash == ANISTS_Walk ||
                    stateInfo.fullPathHash == ANISTS_Run ||
                    stateInfo.fullPathHash == ANISTS_Jump)
                {

                    rigidbody2D.gravityScale = gravityScale;
                }
            }
        }
        // if (!jumped)
        else            // �ܴ̿� ���鿡 �����Ƿ� �׻� 0
        {

            jumpCount = 0;
            rigidbody2D.gravityScale = gravityScale;
        }
        
        // ���� ������ Ȯ��   
        // nameHash�� �� �̻� ���ȵǰ� fullPathHash �̿��϶�� �Ѵ�
        if(stateInfo.fullPathHash == ANISTS_ATTACK_A ||     
            stateInfo.fullPathHash == ANISTS_ATTACK_B ||
            stateInfo.fullPathHash == ANISTS_ATTACK_C ||
            stateInfo.fullPathHash == ANISTS_ATTACKJUMP_A ||
            stateInfo.fullPathHash == ANISTS_ATTACKJUMP_B)
        {

            // �̵� ����
            speedVx = 0;
        }

        // ĳ���� ����
        transform.localScale = new Vector3(     // ũ�� �������� ĳ���� �ٶ󺸴� ���� ����
            basScaleX * dir, transform.localScale.y, transform.localScale.z);

        // ���� ���߿� ���� �̵� ����
        if (jumped && !grounded && groundCheck_OnMoveObject == null)         // ���� ���� ��
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

    // �ڵ� (�ִϸ��̼� �̺�Ʈ�� �ڵ�)
    public void EnableAttackInput()
    {

        atkInputEnabled = true;
    }

    public void SetNextAttack(string name)
    {

        if (atkInputNow == true)
        {

            atkInputNow = false;
            animator.Play(name);
        }

        
        // �߰��� �ڵ�
        atkInputEnabled = false;    // �չ� ���ݿ��� �ȷο� ���� �� ���� ��ư�� ������
                                    // ���� ���� �� 2�� ������ ������ �̸� �����ϰ��� �߰�
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
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == ANISTS_Idle ||
            stateInfo.fullPathHash == ANISTS_Walk ||
            stateInfo.fullPathHash == ANISTS_Run ||
            (stateInfo.fullPathHash == ANISTS_Jump &&
            rigidbody2D.gravityScale >= gravityScale))
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
    }

    public void ActionAttack()
    {

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.fullPathHash == ANISTS_Idle ||
            stateInfo.fullPathHash == ANISTS_Walk ||
            stateInfo.fullPathHash == ANISTS_Run ||
            stateInfo.fullPathHash == ANISTS_Jump ||
            stateInfo.fullPathHash == ANISTS_ATTACK_C)
        {

            animator.SetTrigger("Attack_A");
            if (stateInfo.fullPathHash == ANISTS_Jump ||
                stateInfo.fullPathHash == ANISTS_ATTACK_C)
            {

                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.gravityScale = 0.1f;
            }
        }
        else
        {

            if (atkInputEnabled)
            {

                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
    }

    public void ActionAttackJump()
    {

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (grounded &&
            (stateInfo.fullPathHash == ANISTS_Idle ||
            stateInfo.fullPathHash == ANISTS_Walk ||
            stateInfo.fullPathHash == ANISTS_Run ||
            stateInfo.fullPathHash == ANISTS_ATTACK_A ||
            stateInfo.fullPathHash == ANISTS_ATTACK_B))
        {

            animator.SetTrigger("Attack_C");
            jumpCount = 2;
        }
        else
        {

            if (atkInputEnabled)
            {

                atkInputEnabled = false;
                atkInputNow = true;
            }
        }
    }
}
