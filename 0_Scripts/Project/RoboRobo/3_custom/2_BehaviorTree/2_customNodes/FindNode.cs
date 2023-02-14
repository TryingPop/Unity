using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNode : Node
{

    private BTBoss ai;

    private float radius;   // �Ÿ�
    private float angle;    // ����

    // ������
    public FindNode(BTBoss ai, float radius, float angle)
    {

        this.ai = ai;
        this.radius = radius;
        this.angle = angle;

    }

    public override NodeState Evaluate()
    {

        return ChkTarget() ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    /// <summary>
    /// Ÿ�� ã��
    /// </summary>
    /// <returns></returns>
    private bool ChkTarget()
    {

        // �Ÿ� �ȿ� �ִ� Ÿ�ٵ��� �ݶ��̴� Ȯ��
        Collider[] targetCols = ChkRadius();

        // ã�� ����� ������ �˾Ƽ� Ż��
        for (int i = 0; i < targetCols.Length; i++)
        {

            // �þ� ���� �ȿ� �ִ��� Ȯ���ϰ� ��ֹ��� �߰��� �ִ��� Ȯ��
            if (ChkAngle(targetCols[i].transform) && ChkObstacle(targetCols[i].transform))
            {

                // ��ֹ��� ���� �þ߰��� ���̹Ƿ� targetTrans�� �����ϰ� true�� ��ȯ
                ai.targetTrans = targetCols[i].transform;
                return true;
            }
        }

        // �þ� �ȿ� ���� �� ã�����Ƿ� ���ٰ� �ϰ� �������� ��ȯ
        ai.targetTrans = null;
        return false;
    }

    /// <summary>
    /// ã�ƾ� �ϴ� ����� ���� �Ÿ��ȿ� �ִ��� Ȯ��
    /// </summary>
    /// <returns></returns>
    private Collider[] ChkRadius()
    {

        return Physics.OverlapSphere(ai.transform.position, radius, ai.targetLayer);
    }

    /// <summary>
    /// ���� �ȿ� �ִ��� Ȯ��
    /// </summary>
    /// <param name="targetTrans">Ȯ���� ��ġ</param>
    /// <returns>����� ����</returns>
    private bool ChkAngle(Transform targetTrans)
    {

        // ���� ������ �̿��� ������ ����� �� ������ 
        if (Vector3.Angle(targetTrans.position - ai.transform.position, ai.transform.forward)
            < 0.5 * angle)
        {

            return true;
        }

        return false;
    }
    
    /// <summary>
    /// ��ֹ� üũ
    /// </summary>
    /// <param name="targetTrans">Ȯ���� ��ġ</param>
    /// <returns>����� ����</returns>
    private bool ChkObstacle(Transform targetTrans)
    {

        // ���� �Ÿ���ŭ Ray�� ���� Layer�� �´��� Ȯ��
        RaycastHit hit;
        if (Physics.Raycast(ai.transform.position, targetTrans.position - ai.transform.position, 
            out hit, radius, ai.targetLayer | ai.obstacleLayer))
        {

            if (hit.transform.tag == ai.targetTag)
            {

                return true;
            }
        }

        return false;
    }
}
