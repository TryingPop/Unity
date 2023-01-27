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
        Summon = 10,
        Idle = 10,
        Wander = -1,
    }

    public STATE state;

    private BTBoss ai;
    // �ൿ ��í?

    private Vector3 destination;

    private float distance = 10f;

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

        if (num < 0)
        {

            state = STATE.Idle;
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

        /*
        switch (state)
        {

            case STATE.None:
                break;

            case STATE.Heal:
                Heal();
                break;

            case STATE.Wander:
                Wander();
                break;

            case STATE.Summon:
                SummonRobo();
                break;

            case STATE.Idle:
                Idle();
                break;

            default:
                break;
        }
        */

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
            // ��ȯ
        }
    }

    /// <summary>
    /// �ֺ� ����
    /// </summary>
    private void Wander()
    {

        if (actCnt == 0) 
        {

            destination = SetDestination(); 
            if (destination == Vector3.positiveInfinity || destination == Vector3.negativeInfinity)
            {

                destination = ai.transform.position;
            }
            Debug.Log(destination);
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
    private Vector3 SetDestination()
    {
        
        NavMeshHit hit;
        NavMesh.SamplePosition(ai.transform.position + Random.insideUnitSphere * distance, out hit, distance, NavMesh.AllAreas);
        
        return hit.position + Vector3.up * 0.5f;
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
