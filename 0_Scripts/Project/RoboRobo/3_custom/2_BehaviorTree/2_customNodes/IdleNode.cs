using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleNode : Node
{

    /// <summary>
    /// FSM���� ����
    /// �����̸��� �ߵ��� �ʿ��� �� �� �̴�
    /// ���� ���� üũ�� 0.3�ʸ��� �ϱ⿡ 0.3 x �ϼ� = �ð��� �ȴ�
    /// </summary>
    public enum STATE
    {

        None = 1,   
        Heal = 8,       // ü�� ȸ��
        Summon = 20,    // ��ȯ
        Idle = 10,      // ���
        Wander = -1,    // ����
    }

    public STATE state;             // ���� ����

    private BTBoss ai;      

    private Vector3 destination;    // ������

    private byte actCnt;            // �ൿ ���� ī��Ʈ 0 �̸�
                                    // ������ �ൿ ����
    private bool smBool = true;     // ��ȯ ���� ó�� ������ Ȯ��

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

        // idle ���������Ƿ� ���� idle �� true
        ai.nowIdleBool = true;

        // �ٸ� ���¿��� idle�� �����ߴ��� Ȯ��
        if (ai.ChkIdle()) actCnt = 0;

        // �ൿ ����
        SetAction();

        // �ൿ ����
        ChkAction();
    }

    /// <summary>
    /// �ൿ ����
    /// ü�� ȸ�� > ��� / ��ȯ / ���� ���̴�
    /// ��ȯ ������ ������ ���,
    /// ��� Ȯ���� 40%, ��ȯ Ȯ�� 20%, ���� Ȯ�� 40%�̴�
    /// ��ȯ �Ұ��� �����̸� ��ȯȮ���� ���� Ȯ���� �ٲ��
    /// </summary>
    private void SetAction()
    {

        // �ൿ ���¿� ������ ����̹Ƿ� �ٷ� Ż��
        if (actCnt != 0) return;

        // �ȴ� ��� ����
        ai.WalkAnim(false);

        // ü�� ȸ�������̸� 
        if (ai.ChkHeal()) { state = STATE.Heal; return; };

        // ���, ��ȯ, ���� �ൿ ����
        int num = Random.Range(0, 10);

        if (num < 4)
        {

            state = STATE.Idle;
        }
        // ��ȯ ���� ���� Ȯ��
        else if (num < 6 && objPooling.instance.ChkSummon())
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

        // ������ ���¿� ���� �ൿ ����
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

        // �ǰ��̸� accCnt 0���� �ൿ �缳��
        if (ai.damagedBool) 
        {

            actCnt = 0; 
            return; 
        }

        // �ൿ �� �� ������ �ൿ ����
        if (actCnt >= (byte)state)
        {

            // ���� �� �ൿ �ʱ�ȭ
            actCnt = 0;

            // ��ȯ�� �ִ��� Ȯ��
            if (ai.summoners.Length > 0)
            {
                // ��ȯ
                int num = Random.Range(0, ai.summoners.Length);

                do
                {
                    // ��ȯ ��� ����
                    destination = SetDestination(5f);
                }
                // ��ȯ ������ ����Ƽ ũ��� 2 ~ 5 ���� �ȿ� �ִ��� üũ
                while (Vector3.Distance(ai.transform.position, destination) <= 2f);
                {
                    // ���� ����̹Ƿ� �ٽ� ����
                    destination = SetDestination(5f);
                }

                // ó�� ��ȯ�ϴ� ���
                if (smBool)
                {

                    // ������ ����
                    smBool = false;

                    // �����յ� ���� ����
                    objPooling.instance.SetPrefabs(ai.summoners);
                }

                // ������ summoners ���� ����
                int idx = Random.Range(0, ai.summoners.Length);

                // ������Ʈ ������
                GameObject obj = objPooling.instance.CreateObj(idx);

                // ������ ������Ʈ �������� �̵�
                obj.transform.position = destination;
            }

        }
    }

    /// <summary>
    /// �ֺ� ����
    /// </summary>
    private void Wander()
    {

        // ó�� ������ ���� ������ ����
        if (actCnt == 0) 
        {

            // ������ ����
            destination = SetDestination(8f); 
            
            // ���Ѵ� �ִ��� Ȯ�� ������ �������� ������������ ��� ���¶� ����
            if (destination == Vector3.positiveInfinity)
            {
                
                destination = ai.transform.position;
            }
            else
            {

                // ĳ���� ������ �߹ٴڿ� �־ ���� 0.5 �÷��� �̵� ����
                destination += Vector3.up * 0.5f;
            }

            // �ȴ� ���� ����
            ai.WalkAnim(true);
            ai.agent.destination = destination;
            actCnt++;
        };

        // �������� �Ÿ� 1 ���ϸ� �����
        if (Vector3.Distance(ai.transform.position, destination) <= 1f) 
        {

            ai.WalkAnim(false);
            actCnt = 0;
        }
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    /// <returns>������ �Ÿ�</returns>
    private Vector3 SetDestination(float distance)
    {
        
        NavMeshHit hit;
        // �Ÿ��ȿ� ��ǥ ����
        // �Ÿ� 8 �̳��� �̵� ������ ��ǥ�� �������� �����Ѵ�
        // ������ pdf ����
        NavMesh.SamplePosition(ai.transform.position + Random.insideUnitSphere * distance, out hit, distance, 1) ;
        
        return hit.position;
    }

    /// <summary>
    /// ü�� ȸ��
    /// </summary>
    private void Heal()
    {

        // �ǰݵǸ� �ߴ�
        if (ai.damagedBool)
        {

            actCnt = 0;
            return;
        }

        actCnt++;
        
        // ��
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
