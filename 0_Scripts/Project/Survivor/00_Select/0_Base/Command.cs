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

    
    public int unitNums;             // ������ ���� ��
    protected int receiveNum;        // ���� ����, Ǯ��Ȯ�ο�
    public Vector3 pos;                 // ����� ������
    public Selectable target;           // ����� ǥ��

    public STATE_SELECTABLE type;       // ��� Ÿ��

    #region ������
    // ������
    public Command(int _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        Init(_unitNums, _type, _pos, _target);
    }


    public Command(int _unitNums, STATE_SELECTABLE _type)
    {

        Init(_unitNums, _type);
    }
    #endregion

    #region �޼���
    public bool ChkUsedCommand(int _size)
    {

        // �����ο� �߰�
        receiveNum++;

        if (unitNums < receiveNum)
        {

            // ���д´�
            return true;
        }
        else if (unitNums == receiveNum)
        {

            // ��δ� ���� ���
            if (pool.Count < VarianceManager.MAX_SAVE_COMMANDS) pool.Push(this);
        }

        SetNextPos(_size, receiveNum, ref pos);
        return false;
    }

    public void Canceled()
    {

        unitNums--;
        if (unitNums == receiveNum)
        {

            if (pool.Count < VarianceManager.MAX_SAVE_COMMANDS) pool.Push(this);
        }
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

        unitNums = _unitNums;
        receiveNum = 0;
        type = _type;

        target = null;
        pos = Vector3.zero;
    }
    #endregion

    #region static �޼���
    /// <summary>
    /// ȸ����? ����� ��ġ
    /// </summary>
    public static void SetNextPos(int _size, int num, ref Vector3 pos)
    {

        // ó���� ����
        if (num == 1) return;
        int i = 0;

        int n = num - 1;
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

        if (pool == null) pool = new Stack<Command>(VarianceManager.MAX_SAVE_COMMANDS);

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

        if (pool == null) pool = new Stack<Command>(VarianceManager.MAX_SAVE_COMMANDS);

        // curCommand++;

        if (pool.Count > 0)
        {

            Command cmd = pool.Pop();
            cmd.Init(_unitNums, _type);
            return cmd;
        }

        else return new Command(_unitNums, _type);
    }
#endregion
}