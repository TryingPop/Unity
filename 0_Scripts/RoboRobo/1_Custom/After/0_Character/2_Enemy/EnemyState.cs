using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{

    public enum State { idle, tracking, attack, damaged }

    [SerializeField; private State myState;
    
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    
    [SerializeField] private string targetTag;

    [SerializeField] private float findRadius;
    [SerializeField] private float findAngle;

    /// <summary>
    /// ���� ���� �޼ҵ�
    /// </summary>
    /// <param name="state">���� ��ų ����</param>
    public void SetState(State state)
    {

        myState = state;
    }


    

    /// <summary>
    /// ���ϴ� �ݰ�� ���� �ȿ� ����� �ִ��� ������ �Ǻ�
    /// </summary>
    /// <param name="radius">ã�� �ݰ�</param>
    /// <param name="angle">ã�� ����</param>
    /// <param name="targetTrans">ã�� ��� ���� ���</param>
    /// <returns>�ִ��� ������ ����</returns>
    private bool ChkTarget(float radius, float angle, out Transform targetTrans)
    {

        Collider[] targetCols = ChkRadius(radius);

        if (targetCols.Length > 0 )
        {

            for (int i = 0; i < targetCols.Length; i++)
            {

                if (ChkAngle(targetCols[i].gameObject, angle) && ChkObstacle(targetCols[i].gameObject, radius))
                {
                 
                    targetTrans = targetCols[i].transform;
                    return true;
                }
            }
        }

        targetTrans = null;
        return false;
    }

    /// <summary>
    /// �ݰ� �ȿ� ã�� ���� �ִ��� Ȯ��
    /// </summary>
    /// <param name="radius">�ݰ�</param>
    /// <returns></returns>
    private Collider[] ChkRadius(float radius)
    {

        return Physics.OverlapSphere(transform.position, radius, playerMask);
    }

    /// <summary>
    /// ����� �þ� ���� �ȿ� �ִ��� Ȯ��
    /// </summary>
    /// <param name="obj">���</param>
    /// <param name="angle">�þ� ����</param>
    /// <returns></returns>
    private bool ChkAngle(GameObject obj, float angle)
    {

        if (Vector3.Angle(obj.transform.position - transform.position, transform.forward) < angle * 0.5f)
        {

            return true;
        }

        return false;
    }


    /// <summary>
    /// ���� �ڽ� ���̿� �ٸ� ������Ʈ�� �ִ��� Ȯ��
    /// </summary>
    /// <param name="obj">���</param>
    /// <param name="distance">�Ÿ�</param>
    /// <returns></returns>
    private bool ChkObstacle(GameObject obj, float distance)
    {

        RaycastHit _hit;

        if (Physics.Raycast(transform.position, obj.transform.position - transform.position, out _hit, playerMask | obstacleMask))
        {

            if (_hit.transform.tag == targetTag)
            {

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ��ġ �޾ƿ���
    /// </summary>
    /// <param name="trans">�ѱ� ��</param>
    /// <param name="targetTrans">�޾ƿ� ���</param>
    private void SetTrans(Transform trans, out Transform targetTrans)
    {

        targetTrans = trans;
    }
}
