using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _10_Scanner : MonoBehaviour
{

    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void FixedUpdate()
    {

        // 현재 게임오브젝트를 중심으로 scanRange만큼 원을 생성해 쏘는데 방향은 0이고, 거리도 0이며 targetLayer에 대해서만 판별한다
        // 즉, 제자리에서 원의 크기로 판정한다 - Overlap.. 기능을 쓴다;
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    /// <summary>
    /// 가장 가까운 적 판정
    /// </summary>
    /// <returns></returns>
    Transform GetNearest()
    {

        Transform result = null;
        float diff = 100;

        foreach(RaycastHit2D target in targets)
        {

            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {

                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
