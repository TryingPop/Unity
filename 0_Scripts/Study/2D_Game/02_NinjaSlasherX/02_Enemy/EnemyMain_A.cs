using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain_A : EnemyMain
{

    // 외부 파라미터 (Inspector 표시)
    public int aiIfRUNTOPLAYER = 20;
    public int aiIfJUMPTOPLAYER = 30;
    public int aiIfESCAPE = 10;
    public int aiIfRETURNTODOGPILE = 10;

    public int damageAttack_A = 1;

    // 코드 (AI 사고 루틴 처리)
    public override void FixedUpdateAI()
    {

        // AI 스테이트
        // Deubug.Log(string.Format(">>> aists {0}", aiState));
        switch (aiState)
        {

            case ENEMYAISTS.ACTIONSELECT:   // 사고 루틴 기점
                // 액션 선택
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
                else if (n < aiIfRUNTOPLAYER + aiIfJUMPTOPLAYER + aiIfESCAPE)
                {

                    if (dogPile != null)
                    {

                        SetAIState(ENEMYAISTS.RETURNTODOGPILE, 3.0f);
                    }
                }

                else
                {

                    SetAIState(ENEMYAISTS.WAIT, 1.0f + Random.Range(0.0f, 1.0f));
                }
                enemyCtrl.ActionMove(0.0f);
                break;

            case ENEMYAISTS.WAIT:   // 휴식
                enemyCtrl.ActionLookup(player, 0.1f);
                enemyCtrl.ActionMove(0.0f);
                break;

            case ENEMYAISTS.RUNTOPLAYER:    // 다가간다
                if (GetDistancePlayerY() > 3.0f)
                {

                    SetAIState(ENEMYAISTS.JUMPTOPLAYER, 1.0f);
                }
                if (!enemyCtrl.ActionMoveToNear(player, 2.0f))
                {

                    Attack_A();
                }
                break;

            case ENEMYAISTS.JUMPTOPLAYER:   // 점프해서 다가간다
                if (GetDistancePlayer() < 2.0f && IsChangeDistancePlayer(0.5f))
                {

                    Attack_A();
                    break;
                }
                enemyCtrl.ActionJump();
                enemyCtrl.ActionMoveToNear(player, 0.1f);
                SetAIState(ENEMYAISTS.FREEZ, 0.5f);
                break;

            case ENEMYAISTS.ESCAPE:     // 멀어진다
                if (!enemyCtrl.ActionMoveToFar(player, 7.0f))
                {

                    SetAIState(ENEMYAISTS.ACTIONSELECT, 1.0f);
                }
                break;

            case ENEMYAISTS.RETURNTODOGPILE:    // 도그파일로 돌아온다
                if (enemyCtrl.ActionMoveToNear(dogPile, 2.0f))
                {

                    if (GetDistancePlayer() < 2.0f)
                    {

                        Attack_A();
                    }
                    else
                    {

                        SetAIState(ENEMYAISTS.ACTIONSELECT, 1.0f);
                    }
                }
                break;
        }
    }

    // 코드 (액션 처리)
    void Attack_A()
    {

        enemyCtrl.ActionLookup(player, 0.1f);
        enemyCtrl.ActionMove(0.0f);
        enemyCtrl.ActionAttack("Attack_A", damageAttack_A);
        SetAIState(ENEMYAISTS.WAIT, 2.0f);
    }

    // 코드 (COMBAT AI 관련 처리)
    public override void SetCombatAIState(ENEMYAISTS sts)
    {
        base.SetCombatAIState(sts);
        switch (aiState)
        {

            case ENEMYAISTS.ACTIONSELECT: break;
            case ENEMYAISTS.WAIT:
                aiActionTimeLength = 1.0f + Random.Range(0.0f, 1.0f); break;
            case ENEMYAISTS.RUNTOPLAYER: aiActionTimeLength = 3.0f; break;
            case ENEMYAISTS.JUMPTOPLAYER: aiActionTimeLength = 1.0f; break;
            case ENEMYAISTS.ESCAPE:
                aiActionTimeLength = Random.Range(2.0f, 5.0f); break;
            case ENEMYAISTS.RETURNTODOGPILE: aiActionTimeLength = 3.0f; break;
        }
    }
}
