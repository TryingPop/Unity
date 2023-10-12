using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Command
{

    private static Stack<Command> pool = null;
    // private static int curCommand = 0;

    public static float xBatchSize = 2f;
    public static float zBatchSize = 2f;

    public ushort unitNums;
    protected ushort receiveNum;
    public Vector3 pos;
    public Selectable target;

    public STATE_SELECTABLE type;

    public Command(ushort _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        Set(_unitNums, _type, _pos, _target);
    }

    public Command(ushort _unitNums, STATE_SELECTABLE _type)
    {

        Set(_unitNums, _type);
    }

    public void Received(int _size) 
    {

        receiveNum++;
        SetNextPos(_size);

        ChkAllReceived();
    }

    /// <summary>
    /// 명령을 다 수행했을 때 실행
    /// </summary>
    public void ChkAllReceived()
    {

        if (unitNums == receiveNum) 
        {

            // curCommand--;
            // target != null 이라도 알아서 해제 된다
            if (pool.Count < VariableManager.MAX_SAVE_COMMANDS) pool.Push(this);
        }
    }

    /// <summary>
    /// 유닛이 죽거나 명령이 취소될 경우 실행
    /// </summary>
    public void Canceled()
    {

        unitNums--;

        ChkAllReceived();
    }

    public void Set(ushort _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        unitNums = _unitNums;
        receiveNum = 0;
        type = _type;

        target = _target;
        pos = _pos;
    }

    public void Set(ushort _unitNums, STATE_SELECTABLE _type)
    {

        unitNums = _unitNums;
        receiveNum = 0;
        type = _type;

        target = null;
        pos = Vector3.zero;
    }

    /// <summary>
    /// 회오리? 모양의 배치
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

    public static Command GetCommand(ushort _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        if (pool == null) pool = new Stack<Command>(VariableManager.MAX_SAVE_COMMANDS);

        // 재활용 or 새로 생성
        // curCommand++;

        if (pool.Count > 0)
        {

            Command cmd = pool.Pop();
            cmd.Set(_unitNums, _type, _pos, _target);
            return cmd;
        }

        // else if (curCommand + pool.Count > VariableManager.MAX_COMMANDS) return null;
        else return new Command(_unitNums, _type, _pos, _target);
    }

    public static Command GetCommand(ushort _unitNums, STATE_SELECTABLE _type)
    {

        if (pool == null) pool = new Stack<Command>(VariableManager.MAX_SAVE_COMMANDS);

        // curCommand++;

        if (pool.Count > 0)
        {

            Command cmd = pool.Pop();
            cmd.Set(_unitNums, _type);
            return cmd;
        }

        // else if (curCommand + pool.Count > VariableManager.MAX_COMMANDS) return null;
        else return new Command(_unitNums, _type);
    }
}