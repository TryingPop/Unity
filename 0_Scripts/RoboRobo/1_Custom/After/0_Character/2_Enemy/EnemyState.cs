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
    /// 상태 설정 메소드
    /// </summary>
    /// <param name="state">적용 시킬 상태</param>
    public void SetState(State state)
    {

        myState = state;
    }


    

    /// <summary>
    /// 원하는 반경과 각도 안에 대상이 있는지 없는지 판별
    /// </summary>
    /// <param name="radius">찾을 반경</param>
    /// <param name="angle">찾을 각도</param>
    /// <param name="targetTrans">찾는 경우 담을 대상</param>
    /// <returns>있는지 없는지 유무</returns>
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
    /// 반경 안에 찾는 적이 있는지 확인
    /// </summary>
    /// <param name="radius">반경</param>
    /// <returns></returns>
    private Collider[] ChkRadius(float radius)
    {

        return Physics.OverlapSphere(transform.position, radius, playerMask);
    }

    /// <summary>
    /// 대상이 시야 각도 안에 있는지 확인
    /// </summary>
    /// <param name="obj">대상</param>
    /// <param name="angle">시야 각도</param>
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
    /// 대상과 자신 사이에 다른 오브젝트가 있는지 확인
    /// </summary>
    /// <param name="obj">대상</param>
    /// <param name="distance">거리</param>
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
    /// 위치 받아오기
    /// </summary>
    /// <param name="trans">넘길 값</param>
    /// <param name="targetTrans">받아올 대상</param>
    private void SetTrans(Transform trans, out Transform targetTrans)
    {

        targetTrans = trans;
    }
}
