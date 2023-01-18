using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNode : Node
{

    private float radius;
    private float angle;

    private LayerMask targetLayer;
    private LayerMask obstacleLayer;

    private string targetTag;

    private Transform originTrans;
    private Transform targetTrans;

    public FindNode(float radius, float angle,
                    Transform originTrans, ref Transform targetTrans,
                    LayerMask targetLayer, LayerMask obstacleLayer,
                    string targetTag)
    {

        this.radius = radius;
        this.angle = angle;
        this.originTrans = originTrans;
        this.targetTrans = targetTrans;
        this.targetLayer = targetLayer;
        this.obstacleLayer = obstacleLayer;
        this.targetTag = targetTag;
    }

    public override NodeState Evaluate()
    {

        return ChkTarget() ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private bool ChkTarget()
    {

        Collider[] targetCols = ChkRadius();

        if (targetCols.Length > 0)
        {

            for (int i = 0; i < targetCols.Length; i++)
            {

                if (ChkAngle(targetCols[i].transform) && ChkObstacle(targetCols[i].transform))
                {

                    targetTrans = targetCols[i].transform;
                    return true;
                }
            }
        }

        targetTrans = null;
        return false;
    }

    private Collider[] ChkRadius()
    {

        return Physics.OverlapSphere(originTrans.position, radius, targetLayer);
    }

    private bool ChkAngle(Transform transform)
    {

        if (Vector3.Angle(transform.position - originTrans.position, originTrans.forward)
            < 0.5 * angle)
        {

            return true;
        }

        return false;
    }
    
    private bool ChkObstacle(Transform transform)
    {

        RaycastHit hit;

        if (Physics.Raycast(originTrans.position, transform.position - originTrans.position, 
            out hit, radius, targetLayer | obstacleLayer))
        {

            if (hit.transform.tag == targetTag)
            {

                return true;
            }
        }

        return false;
    }
}
