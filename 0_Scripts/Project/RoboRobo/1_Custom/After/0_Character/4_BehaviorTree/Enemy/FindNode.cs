using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNode : Node
{

    private BTBoss ai;

    private float radius;
    private float angle;

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

    private bool ChkTarget()
    {

        Collider[] targetCols = ChkRadius();

        if (targetCols.Length > 0)
        {

            for (int i = 0; i < targetCols.Length; i++)
            {

                if (ChkAngle(targetCols[i].transform) && ChkObstacle(targetCols[i].transform))
                {

                    ai.targetTrans = targetCols[i].transform;

                    return true;
                }
            }
        }

        ai.targetTrans = null;
        return false;
    }

    private Collider[] ChkRadius()
    {

        return Physics.OverlapSphere(ai.transform.position, radius, ai.targetLayer);
    }

    private bool ChkAngle(Transform targetTrans)
    {

        if (Vector3.Angle(targetTrans.position - ai.transform.position, ai.transform.forward)
            < 0.5 * angle)
        {

            return true;
        }

        return false;
    }
    
    private bool ChkObstacle(Transform targetTrans)
    {

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
