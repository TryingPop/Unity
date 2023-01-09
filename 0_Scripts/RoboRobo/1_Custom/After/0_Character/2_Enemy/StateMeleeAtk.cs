using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMeleeAtk : MonoBehaviour
{

    public bool actionBool;     // Ȱ��ȭ ����

    /// <summary>
    /// ����� ���� �ٶ󺸰� �̵� ���� ����
    /// </summary>
    /// <param name="moveDir">�̵� ����</param>
    /// <param name="targetTrans">��ǥ ���</param>
    public void Action(ref Vector3 moveDir, Transform targetTrans)
    {
        actionBool = true;
        SetDir(ref moveDir, targetTrans);
        transform.LookAt(transform.position + moveDir);
    }

    /// <summary>
    /// �̵� ���� ����
    /// </summary>
    /// <param name="moveDir">�̵� ����</param>
    /// <param name="targetTrans">��ǥ ���</param>
    private void SetDir(ref Vector3 moveDir, Transform targetTrans)
    {

        moveDir = (targetTrans.position - transform.position).normalized;
        moveDir.y = 0;
        
        transform.LookAt(transform.position + moveDir);
    }
}
