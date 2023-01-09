using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMeleeAtk : MonoBehaviour
{

    public bool actionBool;     // 활성화 여부

    /// <summary>
    /// 대상을 향해 바라보고 이동 방향 설정
    /// </summary>
    /// <param name="moveDir">이동 방향</param>
    /// <param name="targetTrans">목표 대상</param>
    public void Action(ref Vector3 moveDir, Transform targetTrans)
    {
        actionBool = true;
        SetDir(ref moveDir, targetTrans);
        transform.LookAt(transform.position + moveDir);
    }

    /// <summary>
    /// 이동 방향 설정
    /// </summary>
    /// <param name="moveDir">이동 방향</param>
    /// <param name="targetTrans">목표 대상</param>
    private void SetDir(ref Vector3 moveDir, Transform targetTrans)
    {

        moveDir = (targetTrans.position - transform.position).normalized;
        moveDir.y = 0;
        
        transform.LookAt(transform.position + moveDir);
    }
}
