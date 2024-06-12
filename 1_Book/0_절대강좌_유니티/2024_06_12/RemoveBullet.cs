using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    public GameObject sparkEffect;


    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.tag == "BULLET")
        {

            // 첫 번째 충돌 지점의 정보 추출
            ContactPoint cp = collision.GetContact(0);

            // 충돌한 총알의 법선 벡터를 쿼터니언 타입으로 변환
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // 스파크 동적 생성
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);

            // 0.5초 후 스파크 삭제
            Destroy(spark, 0.5f);

            Destroy(collision.gameObject);
        }
    }
}
