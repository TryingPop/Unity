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
        Summon = 20,
        Idle = 10,
        Wander = -1,
    }

    public STATE state;

    private BTBoss ai;
    // �ൿ ��í?

    private Vector3 destination;

    private byte actCnt;
    private bool smBool = true;
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

        if (ai.ChkHeal()) { state = STATE.Heal; return; };
        int num = Random.Range(0, 10);
        // �ൿ ����

        if (false)
        {

            state = STATE.Idle;
        }
        else if (true && objPooling.instance.ChkSummon())
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

        if (ai.damagedBool) 
        {

            actCnt = 0; 
            return; 
        }

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

                if (smBool)
                {

                    smBool = false;
                    objPooling.instance.SetPrefabs(ai.summoners);
                }

                int idx = Random.Range(0, ai.summoners.Length);
                GameObject obj = objPooling.instance.CreateObj(idx);
                obj.transform.position = destination;
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
            if (destination == Vector3.positiveInfinity)
            {

                destination = ai.transform.position;
            }
            else
            {
                destination += Vector3.up * 0.5f;
            }


            
            ai.WalkAnim(true);
            ai.agent.destination = destination;
            actCnt++;
        };

        if (Vector3.Distance(ai.transform.position, destination) <= 1f) 
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
        NavMesh.SamplePosition(ai.transform.position + Random.insideUnitSphere * distance, out hit, distance, 1) ;
        

        Debug.Log(hit.position);
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

            ai.NowHp += 10;
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
