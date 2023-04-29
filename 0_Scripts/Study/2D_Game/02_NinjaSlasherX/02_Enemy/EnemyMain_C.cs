using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMain_C : EnemyMain
{

    // �ܺ� �Ķ���� (Inspector ǥ��)
    public int aiIfATTACKONSIGHT = 50;
    public int aiIfRUNTOPLAYER = 10;
    public int aiIfESCAPE = 10;
    public float aiPlayerEscapeDistance = 0.0f;

    public int damageAttack_A = 1;

    public int fireAttack_A = 3;
    public float waitAttack_A = 10.0f;

    // ���� �Ķ����
    int fireCountAttack_A = 0;

    // �ڵ� (AI ��� ��ƾ ó��)
    public override void FixedUpdateAI()
    {

        // �÷��̾ ���� ��������
        enemyCtrl.ActionMoveToFar(player, aiPlayerEscapeDistance);

        // AI ������Ʈ
        // Debug.Loag(string.Format(">>> aists {0}", aiState));
        // �׻� ǥ���ϸ� ó���� �������Ƿ� ����

        switch (aiState)
        {

            case ENEMYAISTS.ACTIONSELECT:   // ��� ��ƾ ����
                // �׼� ����
                int n = SelectRandomAIState();
                if (n < aiIfATTACKONSIGHT)
                {

                    SetAIState(ENEMYAISTS.ATTACKONSIGHT, 100.0f);
                }
                else if (n < aiIfATTACKONSIGHT + aiIfRUNTOPLAYER)
                {

                    SetAIState(ENEMYAISTS.RUNTOPLAYER, 3.0f);
                }
                else if (n < aiIfATTACKONSIGHT + aiIfRUNTOPLAYER + aiIfESCAPE)
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

            case ENEMYAISTS.ATTACKONSIGHT:  // �� �ڸ����� ����
                Attack_A();
                break;

            case ENEMYAISTS.RUNTOPLAYER:    // ������ �ٰ�����
                if (!enemyCtrl.ActionMoveToNear(player, 10.0f))
                {

                    Attack_A();
                }

                break;

            case ENEMYAISTS.ESCAPE: // �־�����
                if (!enemyCtrl.ActionMoveToFar(player, 4.0f))
                {

                    Attack_A();
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
        fireCountAttack_A++;

        if (fireCountAttack_A >= fireAttack_A)
        {

            fireCountAttack_A = 0;
            SetAIState(ENEMYAISTS.FREEZ, waitAttack_A);
        }
    }
}