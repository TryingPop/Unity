using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 명령 전달용 클래스
/// </summary>
public class Command
{

    // 풀링 스텍
    private static Stack<Command> pool = null;

    // 회오리 모양 배치에 쓰이는 변수
    public static float xBatchSize = 2f;
    public static float zBatchSize = 2f;

    
    public int unitNums;             // 전달할 유닛 수
    protected int receiveNum;        // 받은 변수, 풀링확인용
    public Vector3 pos;                 // 명령의 목적지
    public Selectable target;           // 명령의 표적

    public STATE_SELECTABLE type;       // 명령 타입

    #region 생성자
    // 생성자
    public Command(int _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        Init(_unitNums, _type, _pos, _target);
    }


    public Command(int _unitNums, STATE_SELECTABLE _type)
    {

        Init(_unitNums, _type);
    }
    #endregion

    #region 메서드
    public bool ChkUsedCommand(int _size)
    {

        // 수령인원 추가
        receiveNum++;

        if (unitNums < receiveNum)
        {

            // 못읽는다
            return true;
        }
        else if (unitNums == receiveNum)
        {

            // 모두다 읽은 경우
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
    /// 초기화
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
    /// 초기화
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

    #region static 메서드
    /// <summary>
    /// 회오리? 모양의 배치
    /// </summary>
    public static void SetNextPos(int _size, int num, ref Vector3 pos)
    {

        // 처음은 생략
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
    /// 명령 생성, 여기서 풀링
    /// </summary>
    public static Command GetCommand(int _unitNums, STATE_SELECTABLE _type, Vector3 _pos, Selectable _target = null)
    {

        if (pool == null) pool = new Stack<Command>(VarianceManager.MAX_SAVE_COMMANDS);

        // 재활용 or 새로 생성
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
    /// 명령 생성, 여기서 풀링
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