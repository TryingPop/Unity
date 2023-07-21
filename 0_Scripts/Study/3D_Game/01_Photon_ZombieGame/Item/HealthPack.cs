using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthPack : MonoBehaviourPun, IItem
{

    public float health = 50;       // ü���� ȸ���� ��ġ

    public void Use(GameObject target)
    {

        // ���޹��� ���� ������Ʈ�κ��� LivingEntity ������Ʈ ��������
        LivingEntity life = target.GetComponent<LivingEntity>();

        // LivingEntity ������Ʈ�� �ִٸ�
        if (life != null)
        {

            // ü�� ȸ�� ����
            life.RestoreHealth(health);
        }

        // ���Ǿ����Ƿ� �ڽ��� �ı�
        PhotonNetwork.Destroy(gameObject);
    }
}
