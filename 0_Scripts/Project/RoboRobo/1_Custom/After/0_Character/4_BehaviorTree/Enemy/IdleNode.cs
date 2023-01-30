using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleNode : Node
{

    public enum STATE
    {

        None = 1,
        Heal = 3,
        Summon = 50,
        Idle = 10,
        Wander = -1,
    }

    public STATE state;

    private BTBoss ai;
    // �ൿ ��í?

    private Vector3 destination;

    private byte actCnt;

    // ������
    public IdleNode(BTBoss ai)
    {

        this.ai = ai;
    }

    public override NodeState Evaluate()
    {

        Action();
        return NodeState.SUCCESS;
    }

    /// <summary>
    /// �ൿ ���� �� �ൿ �ǽ�
    /// </summary>
    private void Action()
    {

        ai.nowIdleBool = true;
        if (ai.ChkIdle()) actCnt = 0;

        SetAction();
        ChkAction();
    }

    /// <summary>
    /// �ൿ ����
    /// </summary>
    private void SetAction()
    {

        if (actCnt != 0) return;
        if (ai.nowHp < ai.maxHp) { state = STATE.Heal; return; };
        int num = Random.Range(0, 10);
        // �ൿ ����

        if (num < 3)
        {

            state = STATE.Idle;
        }
        else if (num < 4)
        {

            state = STATE.Summon;
        }
        else
        {

            state = STATE.Wander;
        }
        return;
    }

    /// <summary>
    /// �ൿ ����
    /// </summary>
    private void ChkAction()
    {

        if (state == STATE.Heal)
        {

            Heal();
        }
        else if (state == STATE.Wander)
        {

            Wander();
        }
        else if (state == STATE.Summon)
        {

            SummonRobo();
        }
        else if (state == STATE.Idle)
        {

            Idle();
        }
        
        return;
    }

    /// <summary>
    /// �κ� ����
    /// </summary>
    private void SummonRobo()
    {

        actCnt++;

        if (actCnt >= (byte)state)
        {

            actCnt = 0;


            if (ai.summoners.Length > 0)
            {
                // ��ȯ
                int num = Random.Range(0, ai.summoners.Length);
                do
                {
                    destination = SetDestination(5f);
                }
                while (Vector3.Distance(ai.transform.position, destination) <= 2f);
                {

                    destination = SetDestination(5f);
                }

                GameObject.Instantiate(ai.summoners[num], destination, Quaternion.Euler(ai.transform.forward));
            }

            else
            {

                Debug.Log(destination);
                Debug.Log("��ȯ");
            }
        }
    }

    /// <summary>
    /// �ֺ� ����
    /// </summary>
    private void Wander()
    {

        if (actCnt == 0) 
        {

            destination = SetDestination(10f); 
            ai.WalkAnim(true);
            ai.agent.destination = destination;
            actCnt++;
        };

        if (Vector3.Distance(ai.transform.position, destination) <= 0.2f) 
        {

            ai.WalkAnim(false);
            actCnt = 0;
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <returns>������ ��ǥ</returns>
    private Vector3 SetDestination(float distance)
    {
        
        NavMeshHit hit;
        NavMesh.SamplePosition(ai.transform.position + Random.insideUnitSphere * distance, out hit, distance, NavMesh.AllAreas);
        if (hit.position == Vector3.positiveInfinity || hit.position == Vector3.negativeInfinity)
        {

            hit.position = ai.transform.position;
        }
        else
        {

            hit.position += Vector3.up * 0.5f;
        }

        return hit.position;
    }

    /// <summary>
    /// ü�� ȸ��
    /// </summary>
    private void Heal()
    {

        actCnt++;

        if (actCnt >= (byte)state)
        {

            ai.nowHp += 10;
            actCnt = 0;
        }
    }

    /// <summary>
    /// ��� 
    /// </summary>
    private void Idle()
    {

        actCnt++;

        if (actCnt >= (byte)state)
        {

            actCnt = 0;
        }
    }
}
