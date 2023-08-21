using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Command
{


    private static int MAX_COMMANDS = 80;

    private static Stack<Command> pool = null;
    private static int curCommand = 0;

    public static float xBatchSize = 2f;
    public static float zBatchSize = 2f;

    public byte unitNums;
    protected byte receiveNum;
    public Vector3 pos;
    public Transform target;

    public int type;

    public Command(byte _unitNums, int _type, Vector3 _pos, Transform _target = null)
    {

        Set(_unitNums, _type, _pos, _target);
    }

    public void Received(int _size) 
    {

        receiveNum++;
        SetNextPos(_size);

        if (ReceivedAll()) 
        {

            // 다썼으면 풀에 넣는다
            curCommand--;
            if (pool == null) pool = new Stack<Command>(MAX_COMMANDS);
            pool.Push(this); 
        }
    }

    public bool ReceivedAll()
    {

        if (unitNums == receiveNum) return true;
        return false;
    }

    public void Set(byte _unitNums, int _type, Vector3 _pos, Transform _target = null)
    {

        unitNums = _unitNums;
        receiveNum = 0;
        type = _type;

        if (_target == null)
        {

            target = null;
            pos = _pos;
        }
        else
        {

            target = _target;
            pos = target.position;
        }
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

    public static Command GetCommand(byte _unitNums, int _type, Vector3 _pos, Transform _target = null)
    {

        if (pool == null) pool = new Stack<Command>(MAX_COMMANDS);

        // 최대 명령 갯수 초과인 경우 생성 안한다
       

        // 재활용 or 새로 생성
        curCommand++;
        if (pool.Count > 0)
        {

            Command cmd = pool.Pop();
            cmd.Set(_unitNums, _type, _pos, _target);
            return cmd;
        }
        else if (curCommand + pool.Count > MAX_COMMANDS) return null;
        else return new Command(_unitNums, _type, _pos, _target);
    }
}