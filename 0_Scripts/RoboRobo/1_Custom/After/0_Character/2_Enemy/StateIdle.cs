using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : MonoBehaviour
{

    [SerializeField] public Action[] actions;   // 취할 행동

    [SerializeField] private GameObject script; // 대사를 담고 있는 UI 오브젝트

    private int actionNum;

    private int totalWeight;        // 행동들 가중치
    private float cntTime;          // 행동 진행 정도

    private Vector3 direction;      // 이동 방향

    public bool actionBool;         

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
    /// 액션 번호 선택
    /// </summary>
    /// <returns>액션 번호</returns>
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
        
        // 알 수 없는 오류?
        return -1;
    }


    /// <summary>
    /// 해메이는 것
    /// </summary>
    public void Wander(Rigidbody myRd, float moveSpd)
    {

        Move(myRd, moveSpd);
        Rotation(myRd);
    }

    /// <summary>
    /// 그냥 대기
    /// </summary>
    public void Idle()
    {

    }
    
    /// <summary>
    /// 혼자 잡담
    /// </summary>
    public void Chat()
    {


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
/// 행동 보유한 메소드
/// </summary>
[Serializable]
public class Action 
{

    public string name;
    public int weight;
    public float actionTime;
}


