using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain_A : EnemyMain
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public int aiIfRUNTOPLAYER = 20;
    public int aiIfJUMPTOPLAYER = 30;
    public int aiIfESCAPE = 10;

    public int damageAttack_A = 1;

    // �ڵ� (AI ��� ��ƾ ó��)
    public override void FixedUpdateAI()
    {

        // AI ������Ʈ
        // Deubug.Log(string.Format(">>> aists {0}", aiState));
        switch (aiState)
        {

            case ENEMYAISTS.ACTIONSELECT:   // ��� ��ƾ ����
                // �׼� ����
                int n = SelectRandomAIState();
                if ( n < aiIfRUNTOPLAYER)   
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
                else
                {

                    SetAIState(ENEMYAISTS.WAIT, 1.0f + Random.Range(0.0f, 1.0f));
                }
                enemyCtrl.ActionMove(0.0f);
                break;

            case ENEMYAISTS.WAIT:   // �޽�
                enemyCtrl.ActionLookup(player, 0.1f);
                enemyCtrl.ActionMove(0.0f);
                break;

            case ENEMYAISTS.RUNTOPLAYER:    // �ٰ�����
                if (GetDistancePlayerY() > 3.0f)
                {

                    SetAIState(ENEMYAISTS.JUMPTOPLAYER, 1.0f);
                }
                if (!enemyCtrl.ActionMoveToNear(player, 2.0f))
                {

                    Attack_A();
                }
                break;

            case ENEMYAISTS.JUMPTOPLAYER:   // �����ؼ� �ٰ�����
                if (GetDistancePlayer() < 2.0f && IsChangeDistancePlayer(0.5f))
                {

                    Attack_A();
                    break;
                }
                enemyCtrl.ActionJump();
                enemyCtrl.ActionMoveToNear(player, 0.1f);
                SetAIState(ENEMYAISTS.FREEZ, 0.5f);
                break;

            case ENEMYAISTS.ESCAPE:     // �־�����
                if (!enemyCtrl.ActionMoveToFar(player, 7.0f))
                {

                    SetAIState(ENEMYAISTS.ACTIONSELECT, 1.0f);
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
        SetAIState(ENEMYAISTS.WAIT, 2.0f);
    }
}
