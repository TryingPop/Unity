using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ���޿� Ŭ����
/// </summary>
public class Command
{

    // Ǯ�� ����
    private static Stack<Command> pool = null;

    // ȸ���� ��� ��ġ�� ���̴� ����
    public static float xBatchSize = 2f;
    public static float zBatchSize = 2f;

    
    public ushort unitNums;             // ������ ���� ��
    protected ushort receiveNum;        // ���� ����, Ǯ��Ȯ�ο�
    public Vector3 pos;                 // ����� ������
    public Selectable target;           // ����� ǥ��

    public STATE_SELECTABLE type;       // ��� Ÿ��

    // ������
    public Command(int _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        Init(_unitNums, _type, _pos, _target);
    }


    public Command(int _unitNums, STATE_SELECTABLE _type)
    {

        Init(_unitNums, _type);
    }


    /// <summary>
    /// ��� ������ ���� ���� ȸ���� ��ġ ���� ���´�
    /// </summary>
    /// <param name="_size"></param>
    public void Received(int _size) 
    {

        receiveNum++;
        SetNextPos(_size);

        ChkAllReceived();
    }

    /// <summary>
    /// ����� �� �������� �� ����
    /// </summary>
    public void ChkAllReceived()
    {

        if (unitNums == receiveNum) 
        {

            // curCommand--;
            // target != null �̶� �˾Ƽ� ���� �ȴ�
            if (pool.Count < VariableManager.MAX_SAVE_COMMANDS) pool.Push(this);
        }
    }

    /// <summary>
    /// ������ �װų� ����� ��ҵ� ��� ����
    /// </summary>
    public void Canceled()
    {

        unitNums--;

        ChkAllReceived();
    }

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    public void Init(int _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        unitNums = (ushort)_unitNums;
        receiveNum = 0;
        type = _type;

        target = _target;
        pos = _pos;
    }

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    public void Init(int _unitNums, STATE_SELECTABLE _type)
    {

        unitNums = (ushort)_unitNums;
        receiveNum = 0;
        type = _type;

        target = null;
        pos = Vector3.zero;
    }

    /// <summary>
    /// ȸ����? ����� ��ġ
    /// </summary>
    protected void SetNextPos(int _size)
    {

        if (receiveNum == 0) return;
        int i = 0;

        int n = receiveNum;
        while (n > 0)
        {

            i += 1;
            n -= 2 * i;
        }

        if (n + i <= 0)
        {

            if (i % 2 == 0)
            {

                pos.x -= xBatchSize * _size;
            }
            else
            {

                pos.x += xBatchSize * _size;
            }
        }
        else
        {

            if (i % 2 == 0)
            {

                pos.z += zBatchSize * _size;
            }
            else
            {

                pos.z -= zBatchSize * _size;
            }
        }
    }

    /// <summary>
    /// ��� ����, ���⼭ Ǯ��
    /// </summary>
    public static Command GetCommand(int _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        if (pool == null) pool = new Stack<Command>(VariableManager.MAX_SAVE_COMMANDS);

        // ��Ȱ�� or ���� ����
        // curCommand++;

        if (pool.Count > 0)
        {

            Command cmd = pool.Pop();
            cmd.Init(_unitNums, _type, _pos, _target);
            return cmd;
        }

        else return new Command(_unitNums, _type, _pos, _target);
    }

    /// <summary>
    /// ��� ����, ���⼭ Ǯ��
    /// </summary>
    public static Command GetCommand(int _unitNums, STATE_SELECTABLE _type)
    {

        if (pool == null) pool = new Stack<Command>(VariableManager.MAX_SAVE_COMMANDS);

        // curCommand++;

        if (pool.Count > 0)
        {

            Command cmd = pool.Pop();
            cmd.Init(_unitNums, _type);
            return cmd;
        }

        else return new Command(_unitNums, _type);
    }
}