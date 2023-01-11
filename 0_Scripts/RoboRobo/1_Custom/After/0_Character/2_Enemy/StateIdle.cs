using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle: MonoBehaviour
{

    [SerializeField] public Action[] actions;               // ���� �ൿ

    [SerializeField] private float rotationRange;           // ȸ�� ����

    [SerializeField] private MovablePos posScript;          // ���� ���� ��ũ��Ʈ

    [SerializeField] private MessageScript messageScript;   // ��� ������
    [SerializeField] private Talk[] talk;                   // ���


    public enum State
    {

        None = -1,
        Idle,
        Wander,
        Chat
    }

    private State myState;

    private int actionNum;          // �ൿ ��ȣ

    private int totalWeight;        // �ൿ�� ����ġ
    private float cntTime;          // �ൿ ���� ����

    private Vector3 direction;      // �̵� ����

    public bool activeBool;         // Idle �������� Ȯ��

    // public bool moveBool;           // �̵� ������ Ȯ��
    // public bool chatBool;           // ��ȭ ������ Ȯ��

    private bool actionBool;        // �ൿ ��ȭ�� �ִ��� Ȯ��


    public bool ChkState(State state)
    {

        if (myState == state)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// actionBool �� ���� �� üũ
    /// </summary>
    /// <param name="state">���� �ൿ</param>
    /// <returns>actionBool�� ����</returns>
    public void SetActiveBool(EnemyState.State state)
    {

        if (state == EnemyState.State.idle)
        {

            myState = State.None;
            activeBool = true;
        }        
        else
        {

            activeBool = false;
        }
    }

    /// <summary>
    /// ��ü ����ġ
    /// </summary>
    public void SetTotalWeight()
    {

        totalWeight = 0;

        for (int i = 0; i < actions.Length; i++)
        {

            totalWeight += actions[i].weight;
        }
    }

    /// <summary>
    /// �ൿ ���� �� �ൿ
    /// </summary>
    /// <param name="moveDir">�̵� ����</param>
    public void Action(ref Vector3 moveDir)
    {

        actionNum = GetActionNum();

        GetAction(actionNum, ref moveDir);

        ChkTime();

    }

    /// <summary>
    /// �ൿ ��ȣ�� �°� ���� �ൿ
    /// </summary>
    /// <param name="actionNum">�ൿ ��ȣ</param>
    /// <param name="moveDir">�̵� ����</param>
    private void GetAction(int actionNum, ref Vector3 moveDir)
    {
        
        switch (actionNum)
        {
            
            // idle
            case 0:
                SetDir(ref moveDir, false);
                break;

            // wander
            case 1:
                
                Wander(ref moveDir);
                break;

            // chat
            case 2:
                SetDir(ref moveDir, false);
                Chat();
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// �׼� ��ȣ ����
    /// </summary>
    /// <returns>�׼� ��ȣ</returns>
    private int GetActionNum()
    {
        if (actionBool) return actionNum;

        actionBool = true;
        // moveBool = false;
        SetPos();

        int _random = UnityEngine.Random.Range(1, totalWeight + 1);

        for (int i = 0; i < actions.Length; i++)
        {

            if (_random <= actions[i].weight)
            {
                
                /*
                if (i == 1)
                {

                    moveBool = true;
                }
                */
                
                myState = (State)i;
                return i;
            }

            _random -= actions[i].weight;
        }
        
        // �� �� ���� ����?
        return -1;
    }


    /// <summary>
    /// �ظ��̴� ��
    /// </summary>
    public void Wander(ref Vector3 moveDir)
    {

        SetDir(ref moveDir);
        Rotation(moveDir);
    }
    
    /// <summary>
    /// ȥ�� ���
    /// </summary>
    public void Chat()
    {

        // chatBool = true;
        myState = State.Chat;

        messageScript.SetTalk(talk);

        messageScript.gameObject.SetActive(true);
    }

    /// <summary>
    /// �ð� Ȯ��
    /// </summary>
    private void ChkTime()
    {

        cntTime += Time.deltaTime;

        if (actions[actionNum].actionTime <= cntTime)
        {

            actionBool = false;
            cntTime = 0;
        }
    }

    /// <summary>
    /// �̵��� ��ǥ ����
    /// </summary>
    private void SetPos()
    {
        
        direction = posScript.SetPos(0f);
    }

    /// <summary>
    /// �̵� ���� ����
    /// </summary>
    /// <param name="moveDir">�̵� ����</param>
    /// <param name="wanderBool">�̵� ���¿����� ����</param>
    private void SetDir(ref Vector3 moveDir, bool wanderBool = true)
    {

        if (wanderBool)
        {

            Vector3 _dir = direction - transform.position;

            if (_dir.magnitude > 1)
            {

                _dir = _dir.normalized;
            }

            _dir.y = 0f;
            moveDir = _dir;
        }
        else
        {
            
            moveDir = Vector3.zero;
        }
    }

    /// <summary>
    /// �ٶ� ���� ����
    /// </summary>
    /// <param name="moveDir">�̵��� ����</param>
    private void Rotation(Vector3 moveDir)
    {

        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime);
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    public void StopChat()
    {

        messageScript.StopChat();
    }
}

/// <summary>
/// �ൿ ������ �޼ҵ�
/// </summary>
[Serializable]
public class Action 
{

    public string name;
    public int weight;
    public float actionTime;
}


