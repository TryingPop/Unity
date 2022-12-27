using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingMission : MonoBehaviour
{

    [SerializeField] [Tooltip("¸ñÇ¥ ¼ö")] [Range(1, 100)]
    private int targetNum;

    private int destroyCnt;

    public void ChangeTargetNum(int num = 1)
    {
        targetNum += num;
    }

    public int GetRemainCnt()
    {
        
        return targetNum - destroyCnt;
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

}
