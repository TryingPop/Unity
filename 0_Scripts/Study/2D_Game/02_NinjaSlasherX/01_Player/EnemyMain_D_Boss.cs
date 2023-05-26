using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMain_D_Boss : EnemyMain
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public int aiIfRUNTOPLAYER = 30;
    public int aiIfJUMPTOPLAYER = 10;
    public int aiIfESCAPE = 20;
    public int aiIfRETURNTODOGPILE = 10;

    public float minMapX = 0f;
    public float maxMapX = 0f;

    // ĳ��
    GameObject bossHud;
    LineRenderer hudHpBar;
    Transform playerTrfm;

    // ���� �Ķ����
    float dogPileCheckTime = 0.0f;
    float jumpCheckTime = 0.0f;

    // �ڵ� (AI ��� �� ó��)
    public override void Start()
    {
        base.Start();
        bossHud = GameObject.Find("BossHud");
        hudHpBar = GameObject.Find("HUD_HPBar_Boss").GetComponent<LineRenderer>();
        playerTrfm = PlayerController.GetTransform();
    }

    public override void Update()
    {
        base.Update();

        // ���� ǥ��
        if (enemyCtrl.hp > 0)
        {

            hudHpBar.SetPosition(1, new Vector3(15.0f * ((float)enemyCtrl.hp / 
                (float)enemyCtrl.hpMax), 0.0f, 0.0f));
        }
        else
        {

            if (bossHud != null)
            {

                bossHud.SetActive(false);
                bossHud = null;
            }
        }
    }

    public override void FixedUpdateAI()
    {
        // �÷��̾ �������� ���� ������ ������ ���¶�� ���� ���ϱ��� ���ƿ´�
        if (Time.fixedTime - dogPileCheckTime > 3.0f && 
            (playerTrfm.position.x < minMapX || playerTrfm.position.x > maxMapX))
        {

            if (transform.position.x < minMapX + 2f || transform.position.x > maxMapX - 2f)
            {

                if (dogPile != null)
                {

                    SetAIState(ENEMYAISTS.RETURNTODOGPILE, Random.Range(2.0f, 3.0f));
                }
            }
            else
            {

                Attack_A();
                SetAIState(ENEMYAISTS.WAIT, 3.0f);
            }

            dogPileCheckTime = Time.fixedTime;

            jumpCheckTime = Time.fixedTime + 3.0f;
        }
        // �÷��̾ �ö�Ÿ�� ���� ���� ������� ���� ���� ��� ó��
        else if (Time.fixedTime - jumpCheckTime > 1.0f &&
            enemyCtrl.hp > enemyCtrl.hpMax / 2.0f && 
            GetDistancePlayer() < 4.0f)
        {

            Attack_Jump();
            SetAIState(ENEMYAISTS.WAIT, 3.0f);
            jumpCheckTime = Time.fixedTime;
        }

        // AI ����
        switch (aiState)
        {

            case ENEMYAISTS.ACTIONSELECT:   // ������ ���� ����
                // �׼� ����
                int n = SelectRandomAIState();
                if (n < aiIfRUNTOPLAYER)
                {

                    SetAIState(ENEMYAISTS.RUNTOPLAYER, 3.0f);
                }
                else if (n < aiIfRUNTOPLAYER + aiIfJUMPTOPLAYER)
                {

                    SetAIState(ENEMYAISTS.JUMPTOPLAYER, 1.0f);
                }
                else if (n < aiIfRUNTOPLAYER + aiIfJUMPTOPLAYER + aiIfESCAPE)
                {

                    SetAIState(ENEMYAISTS.ESCAPE, Random.Range(2.0f, 5.0f));
                }
                else if (n < aiIfRUNTOPLAYER + aiIfJUMPTOPLAYER + aiIfESCAPE + aiIfRETURNTODOGPILE)
                {

                    if (dogPile != null)
                    {

                        SetAIState(ENEMYAISTS.RETURNTODOGPILE, Random.Range(2.0f, 3.0f));
                    }
                }
                else
                {

                    SetAIState(ENEMYAISTS.WAIT, 1.0f + Random.Range(0.0f, 1.0f));
                }

                enemyCtrl.ActionMove(0.0f);

                // ȣ�� ������ ü���� �������� �� ����
                if (enemyCtrl.hp > enemyCtrl.hpMax / 2.0f)
                {

                    if (aiState == ENEMYAISTS.ESCAPE)
                    {

                        Attack_Jump();
                        SetAIState(ENEMYAISTS.WAIT, 3.0f);
                    }
                }
                break;

            case ENEMYAISTS.WAIT:   // �޽�
                enemyCtrl.ActionLookup(player, 0.1f);
                enemyCtrl.ActionMove(0.0f);
                break;

            case ENEMYAISTS.RUNTOPLAYER:    // �ٰ�����
                if (!enemyCtrl.ActionMoveToNear(player, 7.0f))
                {

                    Attack_A();
                }
                break;

            case ENEMYAISTS.JUMPTOPLAYER:   // ������ �ٰ�����
                if (GetDistancePlayer() > 5.0f)
                {

                    Attack_Jump();
                }
                else
                {

                    enemyCtrl.ActionLookup(player, 0.1f);
                    SetAIState(ENEMYAISTS.WAIT, 3.0f);
                }
                break;

            case ENEMYAISTS.ESCAPE: // �־�����
                if (!enemyCtrl.ActionMoveToFar(player, 7.0f))
                {

                    Attack_B();
                }
                break;

            case ENEMYAISTS.RETURNTODOGPILE:    // �������Ϸ� ���ƿ´�
                if (enemyCtrl.ActionMoveToNear(dogPile, 3.0f))
                {
                }
                else
                {

                    enemyCtrl.ActionMove(0.0f);
                    SetAIState(ENEMYAISTS.ACTIONSELECT, 1.0f);
                }
                break;
        }
    }

    // �ڵ� (�׼� ó��)
    void Attack_A()
    {

        enemyCtrl.ActionLookup(player, 0.1f);
        enemyCtrl.ActionAttack("Attack_A", 10);
        enemyCtrl.attackNockBackVector = new Vector2(1000.0f, 100.0f);
        SetAIState(ENEMYAISTS.WAIT, 3.0f);
    }

    void Attack_B()
    {

        enemyCtrl.ActionMove(0.0f);
        enemyCtrl.ActionAttack("Attack_B", 0);
        SetAIState(ENEMYAISTS.WAIT, 5.0f);
    }

    void Attack_Jump()
    {

        enemyCtrl.ActionLookup(player, 0.1f);
        enemyCtrl.ActionMove(0.0f);
        enemyCtrl.attackEnabled = true;
        enemyCtrl.attackDamage = 1;
        enemyCtrl.attackNockBackVector = new Vector2(1000.0f, 100.0f);
        enemyCtrl.ActionJump();
    }
}
