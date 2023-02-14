using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNode : Node
{

    private BTBoss ai;

    private float radius;   // 거리
    private float angle;    // 각도

    // 생성자
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
    /// 타겟 찾기
    /// </summary>
    /// <returns></returns>
    private bool ChkTarget()
    {

        // 거리 안에 있는 타겟들의 콜라이더 확인
        Collider[] targetCols = ChkRadius();

        // 찾은 대상이 없으면 알아서 탈출
        for (int i = 0; i < targetCols.Length; i++)
        {

            // 시야 각도 안에 있는지 확인하고 장애물이 중간에 있는지 확인
            if (ChkAngle(targetCols[i].transform) && ChkObstacle(targetCols[i].transform))
            {

                // 장애물도 없고 시야각도 안이므로 targetTrans로 선정하고 true로 반환
                ai.targetTrans = targetCols[i].transform;
                return true;
            }
        }

        // 시야 안에 적을 못 찾았으므로 없다고 하고 거짓으로 반환
        ai.targetTrans = null;
        return false;
    }

    /// <summary>
    /// 찾아야 하는 대상이 구형 거리안에 있는지 확인
    /// </summary>
    /// <returns></returns>
    private Collider[] ChkRadius()
    {

        return Physics.OverlapSphere(ai.transform.position, radius, ai.targetLayer);
    }

    /// <summary>
    /// 각도 안에 있는지 확인
    /// </summary>
    /// <param name="targetTrans">확인할 위치</param>
    /// <returns>대상의 유무</returns>
    private bool ChkAngle(Transform targetTrans)
    {

        // 벡터 내적을 이용해 각도를 계산할 수 있으나 
        if (Vector3.Angle(targetTrans.position - ai.transform.position, ai.transform.forward)
            < 0.5 * angle)
        {

            return true;
        }

        return false;
    }
    
    /// <summary>
    /// 장애물 체크
    /// </summary>
    /// <param name="targetTrans">확인할 위치</param>
    /// <returns>대상의 유무</returns>
    private bool ChkObstacle(Transform targetTrans)
    {

        // 일정 거리만큼 Ray를 쏴서 Layer가 맞는지 확인
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
