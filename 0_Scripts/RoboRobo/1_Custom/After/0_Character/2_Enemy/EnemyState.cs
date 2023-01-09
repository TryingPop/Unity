using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{

    public enum State 
    { 
        idle, 
        attack, 
        damaged 
    }

    [SerializeField] private State myState;
    
    [SerializeField] private LayerMask playerMask;      // 타겟 레이어
    [SerializeField] private LayerMask obstacleMask;    // 장애물 레이어
    
    [SerializeField] private string targetTag;          // 타겟 태그

    [SerializeField] private float findRadius;          // 공격 상태로 바뀔 거리
    [SerializeField] private float findAngle;           // 각도 찾기

    private State beforeState;                          // 상태 변화 확인용
    public bool chkBool;                                // 상태 변화 존재
    /// <summary>
    /// 상태 설정 메소드
    /// </summary>
    /// <param name="state">적용 시킬 상태</param>
    public void SetState(State state)
    {

        myState = state;
    }

    /// <summary>
    /// 현재 상태를 반환
    /// </summary>
    /// <returns>현재 상태</returns>
    public State GetState()
    {

        return myState;
    }

    /// <summary>
    /// 상태 확인 메소드
    /// </summary>
    /// <param name="targetTrans">공격 상태일 때 담을 목표</param>
    public void ChkState(ref Transform targetTrans) 
    {
        beforeState = myState;
        chkBool = false;

        // 타겟이 공격 범위에 있으면 공격 상태
        if (ChkTarget(findRadius, findAngle, ref targetTrans))
        {
            
            SetState(State.attack);
        }
        // 없는 경우면 대기
        else
        {

            SetState(State.idle);
        }

        if (beforeState != myState)
        {
            
            chkBool = true;
        }
    }
    

    /// <summary>
    /// 원하는 반경과 각도 안에 대상이 있는지 없는지 판별
    /// </summary>
    /// <param name="radius">찾을 반경</param>
    /// <param name="angle">찾을 각도</param>
    /// <param name="targetTrans">찾는 경우 담을 대상</param>
    /// <returns>있는지 없는지 유무</returns>
    private bool ChkTarget(float radius, float angle, ref Transform targetTrans)
    {

        Collider[] targetCols = ChkRadius(radius);

        if (targetCols.Length > 0 )
        {

            for (int i = 0; i < targetCols.Length; i++)
            {

                if (ChkAngle(targetCols[i].gameObject, angle) && ChkObstacle(targetCols[i].gameObject, radius))
                {

                    // SetTrans(targetCols[i].transform, targetTrans);

                    // Debug.Log(targetTrans);
                    targetTrans = targetCols[i].transform;
                    return true;
                }
            }
        }

        SetTrans(null, ref targetTrans);
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

        if (Physics.Raycast(transform.position, obj.transform.position - transform.position, out _hit, distance, playerMask | obstacleMask))
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
    private void SetTrans(Transform trans, ref Transform targetTrans)
    {

        targetTrans = trans;
    }


}
