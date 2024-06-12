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

            // ù ��° �浹 ������ ���� ����
            ContactPoint cp = collision.GetContact(0);

            // �浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // ����ũ ���� ����
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);

            // 0.5�� �� ����ũ ����
            Destroy(spark, 0.5f);

            Destroy(collision.gameObject);
        }
    }
}
