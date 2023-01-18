using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StateMeleeAtk : MonoBehaviour
{

    public bool activeBool;     // Ȱ��ȭ ����

    /// <summary>
    /// actionBool ����
    /// </summary>
    /// <param name="state">���� ����</param>
    public void SetActiveBool(EnemyState.State state)
    {

        if(state == EnemyState.State.attack)
        {

            activeBool = true;
        }
        else
        {

            activeBool = false;
        }
    }

    /// <summary>
    /// ����� ���� �ٶ󺸰� �̵� ���� ����
    /// </summary>
    /// <param name="moveDir">�̵� ����</param>
    /// <param name="targetTrans">��ǥ ���</param>
    public void Action(ref Vector3 moveDir, Transform targetTrans)
    {
        activeBool = true;
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

    /*
    public void SizeUp()
    {

        transform.localScale = Vector3.one;
    }
    */
}
