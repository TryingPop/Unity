using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain_B : EnemyMain
{

    // �ܺ� �Ķ����
    public int aiIfRUNTOPLAYER = 30;
    public int aiIfESCAPE = 20;

    public int damageAttack_A = 1;
    public int damageAttack_B = 2;

    // �ڵ� (AI ��� ��ƾ ó��)
    public override void FixedUpdateAI()
    {

        // AI ������Ʈ
        // Debug.Log(string.Format(">>> aists {0}", aiState));
        // �׻� ǥ���ϸ� ó���� �������Ƿ� ����

        switch (aiState)
        {

            case ENEMYAISTS.ACTIONSELECT:   // ��� ��ƾ ����
                // �׼� ����
                int n = SelectRandomAIState();
                if (n < aiIfRUNTOPLAYER)
                {

                    SetAIState(ENEMYAISTS.RUNTOPLAYER, 3.0f);
                }
                else if (n < aiIfRUNTOPLAYER + aiIfESCAPE)
                {

                    SetAIState(ENEMYAISTS.ESCAPE, Random.Range(2.0f, 5.0f));
                }
                else
                {

                    SetAIState(ENEMYAISTS.WAIT, 1.0f + Random.Range(0.0f, 1.0f));
                }
                enemyCtrl.ActionMove(0.0f);
                break;

            case ENEMYAISTS.WAIT:
                enemyCtrl.ActionLookup(player, 0.1f);
                enemyCtrl.ActionMove(0.0f);
                break;

            case ENEMYAISTS.RUNTOPLAYER:    // ������ �ٰ�����
                if (GetDistancePlayerY() < 3.0f)
                {

                    if (!enemyCtrl.ActionMoveToNear(player, 2.0f))
                    {

                        Attack_A();
                    }
                }
                else
                {

                    if(GetDistancePlayerX() > 3.0f && !enemyCtrl.ActionMoveToNear(player, 5.0f))
                    {

                        Attack_A();
                    }
                }
                break;

            case ENEMYAISTS.ESCAPE: // �־�����
                if (!enemyCtrl.ActionMoveToFar(player, 4.0f))
                {

                    Attack_B();
                }
                break;
        }
    }

    // �ڵ� (�׼� ó��)
    void Attack_A()
    {

        enemyCtrl.ActionLookup(player, 0.1f);
        enemyCtrl.ActionMove(0.0f);
        enemyCtrl.ActionAttack("Attack_A", damageAttack_A);
        enemyCtrl.attackNockBackVector = new Vector2(500.0f, 2000.0f);
        SetAIState(ENEMYAISTS.WAIT, 3.0f);
    }

    void Attack_B()
    {

        enemyCtrl.ActionLookup(player, 0.1f);
        enemyCtrl.ActionMove(0.0f);
        enemyCtrl.ActionAttack("Attack_B", damageAttack_B);
        enemyCtrl.attackNockBackVector = new Vector2(500.0f, 1000.0f);
        SetAIState(ENEMYAISTS.FREEZ, 5.0f);
    }
}