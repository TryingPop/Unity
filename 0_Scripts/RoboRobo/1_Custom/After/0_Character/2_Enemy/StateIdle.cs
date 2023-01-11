using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle: MonoBehaviour
{

    [SerializeField] public Action[] actions;               // 취할 행동

    [SerializeField] private float rotationRange;           // 회전 정도

    [SerializeField] private MovablePos posScript;          // 방향 설정 스크립트

    [SerializeField] private MessageScript messageScript;   // 대사 보조용
    [SerializeField] private Talk[] talk;                   // 대사


    public enum State
    {

        None = -1,
        Idle,
        Wander,
        Chat
    }

    private State myState;

    private int actionNum;          // 행동 번호

    private int totalWeight;        // 행동들 가중치
    private float cntTime;          // 행동 진행 정도

    private Vector3 direction;      // 이동 방향

    public bool activeBool;         // Idle 상태인지 확인

    // public bool moveBool;           // 이동 중인지 확인
    // public bool chatBool;           // 대화 중인지 확인

    private bool actionBool;        // 행동 변화가 있는지 확인


    public bool ChkState(State state)
    {

        if (myState == state)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// actionBool 값 설정 및 체크
    /// </summary>
    /// <param name="state">현재 행동</param>
    /// <returns>actionBool의 상태</returns>
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
    /// 전체 가중치
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
    /// 행동 설정 및 행동
    /// </summary>
    /// <param name="moveDir">이동 방향</param>
    public void Action(ref Vector3 moveDir)
    {

        actionNum = GetActionNum();

        GetAction(actionNum, ref moveDir);

        ChkTime();

    }

    /// <summary>
    /// 행동 번호에 맞게 취할 행동
    /// </summary>
    /// <param name="actionNum">행동 번호</param>
    /// <param name="moveDir">이동 방향</param>
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
    /// 액션 번호 선택
    /// </summary>
    /// <returns>액션 번호</returns>
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
        
        // 알 수 없는 오류?
        return -1;
    }


    /// <summary>
    /// 해메이는 것
    /// </summary>
    public void Wander(ref Vector3 moveDir)
    {

        SetDir(ref moveDir);
        Rotation(moveDir);
    }
    
    /// <summary>
    /// 혼자 잡담
    /// </summary>
    public void Chat()
    {

        // chatBool = true;
        myState = State.Chat;

        messageScript.SetTalk(talk);

        messageScript.gameObject.SetActive(true);
    }

    /// <summary>
    /// 시간 확인
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
    /// 이동할 좌표 설정
    /// </summary>
    private void SetPos()
    {
        
        direction = posScript.SetPos(0f);
    }

    /// <summary>
    /// 이동 방향 설정
    /// </summary>
    /// <param name="moveDir">이동 방향</param>
    /// <param name="wanderBool">이동 상태에서만 적용</param>
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
    /// 바라볼 방향 설정
    /// </summary>
    /// <param name="moveDir">이동할 방향</param>
    private void Rotation(Vector3 moveDir)
    {

        transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime);
    }

    /// <summary>
    /// 대사 중지
    /// </summary>
    public void StopChat()
    {

        messageScript.StopChat();
    }
}

/// <summary>
/// 행동 보유한 메소드
/// </summary>
[Serializable]
public class Action 
{

    public string name;
    public int weight;
    public float actionTime;
}


