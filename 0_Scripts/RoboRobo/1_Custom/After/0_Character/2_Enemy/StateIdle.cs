using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : MonoBehaviour
{

    [SerializeField] public Action[] actions;   // ���� �ൿ

    [SerializeField] private GameObject script; // ��縦 ��� �ִ� UI ������Ʈ

    private int actionNum;

    private int totalWeight;        // �ൿ�� ����ġ
    private float cntTime;          // �ൿ ���� ����

    private Vector3 direction;      // �̵� ����

    public bool actionBool;         

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

    public void Action(Rigidbody myRd, float moveSpd)
    {

        actionNum = GetActionNum();

        GetAction(actionNum, myRd, moveSpd);

        ChkTime();
    }

    private void GetAction(int actionNum, Rigidbody myRd, float moveSpd)
    {

        switch (actionNum)
        {

            case 0:
                Idle();
                break;

            case 1:
                Wander(myRd, moveSpd);
                break;

            case 2:
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
        SetDir();

        int _random = UnityEngine.Random.Range(1, totalWeight + 1);

        for (int i = 0; i < actions.Length; i++)
        {

            if (_random <= actions[i].weight)
            {
                Debug.LogError(actions[i].name);
                Debug.LogError(_random);
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
    public void Wander(Rigidbody myRd, float moveSpd)
    {

        Move(myRd, moveSpd);
        Rotation(myRd);
    }

    /// <summary>
    /// �׳� ���
    /// </summary>
    public void Idle()
    {

    }
    
    /// <summary>
    /// ȥ�� ���
    /// </summary>
    public void Chat()
    {


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

    private void SetDir()
    {

        direction.Set(0f, UnityEngine.Random.Range(0f, 360f), 0f);
    }

    private void Move(Rigidbody myRd, float moveSpd)
    {

        myRd.MovePosition(transform.position + (transform.forward * moveSpd * Time.deltaTime));
    }

    private void Rotation(Rigidbody myRd)
    {

        Vector3 _rotation = Vector3.Lerp(transform.eulerAngles, direction, 0.01f);

        myRd.MoveRotation(Quaternion.Euler(_rotation));
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


