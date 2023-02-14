using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMissile : MonoBehaviour
{

    protected static int atk;           // 공격력
    protected static float spd;         // 속도
    protected static float turn;        // 회전율
    
    protected Rigidbody rd;             // 강체를 통한 이동
    protected Transform targetTrans;    // 타겟 방향

    private void Awake()
    {

        rd = GetComponent<Rigidbody>();
        Destroy(gameObject, 3f);
    }

    // 매 프레임이 아닌 변하는 상황에서만 계산 시작
    private void FixedUpdate()
    {

        // 속도 재 조절
        rd.velocity = transform.forward * spd;

        // 타겟으로 서서히 회전
        var targetRotation = Quaternion.LookRotation(targetTrans.position - transform.position);
        rd.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));
    }

    /// <summary>
    /// 탄약 세팅
    /// </summary>
    /// <param name="atk">공격력</param>
    /// <param name="spd">속도</param>
    /// <param name="turn">회전율</param>
    public static void SetVar(int atk, float spd, float turn)
    {

        EnemyMissile.atk = atk;
        EnemyMissile.spd = spd;
        EnemyMissile.turn = turn;
    }

    /// <summary>
    /// 타겟 설정
    /// </summary>
    /// <param name="targetTrans"></param>
    public void Set(Transform targetTrans)
    {

        this.targetTrans = targetTrans;
    }

    private void OnCollisionEnter(Collision collision)
    {

        // 플레이어와 충돌 시 데미지
        if (collision.gameObject.tag == targetTrans.tag)
        {

            collision.gameObject.GetComponent<Stat>().OnDamaged(atk);
        }

        // 충돌했으므로 파괴
        Destroy(gameObject);
    }
}
