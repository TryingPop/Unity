using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAI : MonoBehaviour
{

    // �ܺ� �Ķ����(Inspector ǥ��)
    public int freeAIMax = 3;
    public int blockAttackAIMax = 10;

    // �ڵ� (Monobehaviour �⺻ ��� ����)
    void FixedUpdate()
    {

        var activeEnemyMainList = new List<EnemyMain>();

        // ī�޶� ��ġ�� �ִ� ���� �˻�
        GameObject[] enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemyList == null)
        {

            return;
        }

        foreach(GameObject enemy in enemyList)
        {

            // Debug.Log(string.Format(">>> obj Name {0} position {1}", 
            //     enemy.name, enemy.transform.position));
            EnemyMain enemyMain = enemy.GetComponent<EnemyMain>();
            if (enemyMain != null)
            {

                if (enemyMain.combatAIOrder && enemyMain.cameraEnabled)
                {

                    activeEnemyMainList.Add(enemyMain);
                }
                else
                {

                    // Debug.LogWarning(string.Format("CombatAI: EnemyMain null : {0} {1}",
                    //    enemy.name, enemy.transform.position));
                }
            }
        }

        // �����ϴ� ���� �����Ѵ�
        int i = 0;
        foreach(EnemyMain enemyMain in activeEnemyMainList)
        {

            if (i < freeAIMax)
            {

                // �׳� �����Ӱ� �ൿ�ϵ��� �д�
            }
            else if (i < freeAIMax + blockAttackAIMax)
            {

                // ������ �����Ѵ�
                if (enemyMain.aiState == ENEMYAISTS.RUNTOPLAYER)
                {

                    enemyMain.SetCombatAIState(ENEMYAISTS.WAIT);
                }
            }
            else
            {

                // �ൿ�� ������Ų��
                if (enemyMain.aiState != ENEMYAISTS.WAIT)
                {

                    enemyMain.SetCombatAIState(ENEMYAISTS.WAIT);
                }
            }

            i++;
        }

        // Debug.Log(string.Format(">>> Combat AI {0}", i));
    }
}
