using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HuntingMission : MonoBehaviour
{
    public static HuntingMission instance;

    [SerializeField]
    [Tooltip("남은 적 표시")]
    private Text remainEnemyCnt;

    [SerializeField] [Tooltip("목표 수")] [Range(1, 999)]
    private int setTargetNum;


    private int targetNum;

    private int destroyCnt;

    private int targetLength;

    private void Start()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }

        Reset(this, EventArgs.Empty);
        GameManager.instance.otherReset += Reset;
    }

    private void GetTargetLength()
    {
        targetLength = targetNum.ToString().Length;
    }

    public void SetTargetNum(int num = 1)
    {
        
        targetNum = num;
    }

    public int GetRemainCnt()
    {
        
        return targetNum - destroyCnt < 0 ? 0: targetNum - destroyCnt ;
    }

    public void ChangeDestroyCnt(int num = 1)
    {
        
        destroyCnt += num;
    }

    public bool ChkWin()
    {

        if (destroyCnt >= targetNum)
        {

            return true;
        }

        return false;
    }

    public void Reset(object sender, EventArgs e)
    {
        targetNum = setTargetNum;
        destroyCnt = 0;

        GetTargetLength();
        ChkRemainEnemyCnt();
    }



    public void ChkRemainEnemyCnt()
    {
        
        if (targetNum == 1)
        {

            remainEnemyCnt.text = $"BOSS";
        }
        else if (targetLength == 1) 
        {

            remainEnemyCnt.text = $"{GetRemainCnt(), 1} / {targetNum, 1}";
        } 
        else if (targetLength == 2)
        {

            remainEnemyCnt.text = $"{GetRemainCnt(), 2} / {targetNum, 2}";
        }
        else
        {

            remainEnemyCnt.text = $"{GetRemainCnt(), 3} / {targetNum, 3}";
        }
    }
}
