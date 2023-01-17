using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoverAvailableNode : Node
{

    private Cover[] availableCovers;
    private Transform target;
    private BTEnemy ai;


    public IsCoverAvailableNode(Cover[] availableCovers, Transform target, BTEnemy ai)
    {

        this.availableCovers= availableCovers;
        this.target= target;
        this.ai= ai;
    }

    public override NodeState Evaluate()
    {

        Transform bestSpot = FindBestCoverSpot();
        ai.SetBestCover(bestSpot);
        return bestSpot != null ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    private Transform FindBestCoverSpot()
    {

        // 숨을 장소를 먼저 정했고 적당한지 확인
        if (ai.GetBestCoverSpot() != null)
        {

            if (CheckIfSpotIsValid(ai.GetBestCoverSpot()))
            {

                return ai.GetBestCoverSpot();
            }
        }
        
        float minAngle = 90;
        Transform bestSpot = null;

        for (int i = 0; i < availableCovers.Length; i++)
        {

            Transform bestSpotInCover = FindBestSpotInCover(availableCovers[i], ref minAngle);
            if (bestSpotInCover != null)
            {

                bestSpot = bestSpotInCover;
                break;
            }
        }
        return bestSpot;
    }

    /// <summary>
    /// Cover 중 숨을 장소 찾기
    /// 타겟과 벽하나 끼고 
    /// minAngle보다 적은 각도에 한해서만 찾는다
    /// </summary>
    /// <param name="cover">숨을 장소</param>
    /// <param name="minAngle">이 각도 보다 적은 경우에만 허용</param>
    /// <returns></returns>
    private Transform FindBestSpotInCover(Cover cover, ref float minAngle)
    {

        Transform[] availableSpots = cover.GetCoverSpots();

        Transform bestSpot = null;

        for (int i = 0; i < availableCovers.Length; i++)
        {

            Vector3 direction = target.position - availableSpots[i].position;

            if (CheckIfSpotIsValid(availableSpots[i]))
            {

                float angle = Vector3.Angle(availableSpots[i].forward, direction);

                if (angle < minAngle)
                {

                    minAngle = angle;
                    bestSpot = availableSpots[i];
                }
            }
        }

        return bestSpot;
    }

    /// <summary>
    /// 숨을 장소랑 타겟 사이에 장애물이 있는지 확인
    /// </summary>
    /// <param name="spot">숨을 장소</param>
    /// <returns>장애물이 있다</returns>
    private bool CheckIfSpotIsValid(Transform spot)
    {

        RaycastHit hit;
        Vector3 direction = target.position - spot.position;

        if (Physics.Raycast(spot.position, direction, out hit))
        {

            if (hit.collider.transform != target)
            {

                return true;
            }
        }

        return false;

    }
}
