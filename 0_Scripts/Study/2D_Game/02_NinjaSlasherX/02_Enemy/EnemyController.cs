using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseCharacterController
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public float initHpMax = 5.0f;
    public float initSpeed = 6.0f;
    public bool jumpActionEnabled = true;
    public Vector2 jumpPower = new Vector2(0.0f, 1500.0f);
    public int addScore = 500;

    // �ܺ� �Ķ����
    [HideInInspector] public bool attackEnabled = false;
    [HideInInspector] public int attackDamage = 1;
    [HideInInspector] public Vector2 attackNockBackVector = Vector3.zero;

    // �ִϸ��̼� �ؽ� �̸�
    public readonly static int ANISTS_Idle =
        Animator.StringToHash("Base Layer.Enemy_Idle");
    public readonly static int ANISTS_Run =
        Animator.StringToHash("Base Layer.Enemy_Run");
    public readonly static int ANISTS_Jump =
        Animator.StringToHash("Base Layer.Enemy_Jump");
    public readonly static int ANITAG_ATTACK =
        Animator.StringToHash("Attack");
    public readonly static int ANISTS_DMG_A =
        Animator.StringToHash("Base Layer.Enemy_DMG_A");
    public readonly static int ANISTS_DMG_B =
        Animator.StringToHash("Base Layer.Enemy_DMG_B");
    public readonly static int ANISTS_Dead =
        Animator.StringToHash("Base Layer.Enemy_Dead");

    // ĳ�� 
    PlayerController playerCtrl;
    Animator playerAnim;

    // �ڵ� (Monobehaviour �⺻ ��� ����)
    protected override void Awake()
    {

        base.Awake();

        if (playerCtrl == null) playerCtrl = PlayerController.GetController();
        if (playerAnim == null) playerAnim = playerCtrl.GetComponent<Animator>();

        hpMax = initHpMax;
        hp = hpMax;
        speed = initSpeed;
    }

    protected override void FixedUpdateCharacter()
    {

        // �����ߴ��� �˻�
        if (jumped)
        {

            // ���� �˻� (A: ���� ���� ����, B: ������ �ð��� ���� ����)
            if ((grounded && !groundedPrev) || 
                (grounded && Time.fixedTime > jumpStartTime + 1.0f))
            {

                jumped = false;
            }

            if (Time.fixedTime > jumpStartTime + 1.0f)
            {
                if (rigidbody2D.gravityScale < gravityScale)
                {

                    rigidbody2D.gravityScale = gravityScale;
                }
            }
        }
        else
        {

            rigidbody2D.gravityScale = gravityScale;
        }

        // ĳ���� ����
        transform.localScale = new Vector3(
            basScaleX * dir, transform.localScale.y, transform.localScale.z);

        // Memo: ���߿��� �ǰ��� ������ �� X ���� �̵� ����
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == EnemyController.ANISTS_DMG_A ||
            stateInfo.fullPathHash == EnemyController.ANISTS_DMG_B ||
            stateInfo.fullPathHash == EnemyController.ANISTS_Dead)
        {

            speedVx = 0.0f;
            rigidbody2D.velocity = new Vector2(0.0f, rigidbody2D.velocity.y);
        }
    }

    // �ڵ� (�⺻ �׼�)
    public bool ActionJump()
    {

        if (jumpActionEnabled && grounded && !jumped)
        {

            animator.SetTrigger("Jump");
            rigidbody2D.AddForce(jumpPower);
            jumped = true;
            jumpStartTime = Time.fixedTime;
        }

        return jumped;
    }

    public void ActionAttack(string atkname, int damage)
    {

        attackEnabled = true;
        attackDamage = damage;
        animator.SetTrigger(atkname);
    }

    public void ActionDamage()
    {

        int damage = 0;
        if (hp < 0)
        {

            return;
        }
        if (superArmor)
        {

            animator.SetTrigger("SuperArmor");
        }

        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == PlayerController.ANISTS_ATTACK_C)
        {

            damage = 3;
            if (!superArmor || superArmor_jumpAttackDmg)
            {

                animator.SetTrigger("DMG_B");
                jumped = true;
                jumpStartTime = Time.fixedTime;
                AddForceAnimatorVy(1500.0f);
                Debug.Log(string.Format(">>> DMG_B Jump {0}", stateInfo.fullPathHash));
            }
        }
        else
        {
            if (!grounded)
            {

                damage = 2;
                if (!superArmor || superArmor_jumpAttackDmg)
                {
                    animator.SetTrigger("DMG_B");
                    jumped = true;
                    jumpStartTime = Time.fixedTime;
                    // AddForceAnimatorVy(10.0f);
                    playerCtrl.rigidbody2D.AddForce(new Vector2(0.0f, 20.0f));
                    // Debug.Log(string.Format(">>> DMG_B {0}", stateInfo.fullPathHash));
                }
            }
            else
            {

                damage = 1;
                if (!superArmor)
                {

                    animator.SetTrigger("DMG_A");
                    // Debug.Log(string.Format(">>> DMG_A {0}", stateInfo.fullPathHash));
                }
            }
        }

        if (SetHp(hp - damage, hpMax))
        {

            Dead(false);
            int addScoreV =
                ((int)((float)addScore * (playerCtrl.hp / playerCtrl.hpMax)));
            addScoreV = (int)((float)addScore * (grounded? 1.0 : 1.5f));
            PlayerController.score += addScoreV;
        }
    }

    // �ڵ� (�� ��)
    public override void Dead(bool gameOver)
    {

        base.Dead(gameOver);
        Destroy(gameObject, 1.0f);
    }
}
