using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


// ����ü�� ������ ���� ������Ʈ���� ���� ���븦 ����
// ü��, ������ �޾Ƶ��̱�, ��� ���, ��� �̺�Ʈ�� ����
public class LivingEntity : MonoBehaviourPun, IDamageable
{

    public float startingHealth = 100f;             // ���� ü��
    public float health { get; protected set; }     // ���� ü��
    public bool dead { get; protected set; }        // ��� ����
    public event Action onDeath;                     // ��� �� �ߵ��� �̺�Ʈ
    // event Ű����� Ŭ���� �ܺο��� ���� ���ϰ� �����ϴ� Ű����

    // ȣ��Ʈ -> ��� Ŭ���̾�Ʈ �������� ü�°� ��� ���¸� ����ȭ �ϴ� �޼ҵ�
    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        
        health = newHealth;
        dead = newDead;
    }

    // ����ü�� Ȱ��ȭ�� �� ���¸� ����
    protected virtual void OnEnable()
    {

        // ������� ���� ���·� ����
        dead = false;
        // ü���� ���� ü������ �ʱ�ȭ
        health = startingHealth;
    }

    // ������ ó��
    // ȣ��Ʈ���� ���� �ܵ� ����ǰ�, ȣ��Ʈ�� ���� �ٸ� Ŭ���̾�Ʈ���� �ϰ� �����
    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {

        if (PhotonNetwork.IsMasterClient)
        {
         
            // ��������ŭ ü�� ����
            health -= damage;

            // ȣ��Ʈ���� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead);       // �ٸ� Ŭ���̾�Ʈ���� �������� PunRPC �Ӽ��� �ο��� �޼ҵ带 �����Ű�� �Լ�
                                                                                        // ���ڷ� �޼ҵ��, ���, �޼ҵ��� �Ű������� �����̴�

            // �ٸ� Ŭ���̾�Ʈ�� OnDamage�� �����ϵ��� ��
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        // ü���� 0 ���� && ���� ���� �ʾҴٸ� ��� ó�� ����
        if (health <= 0 && !dead)
        {

            Die();
        }
    }

    // ü���� ȸ���ϴ� ���
    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {

        if (dead)
        {

            // �̹� ����� ��� ü���� ȸ���� �� ����
            return;
        }

        // ȣ��Ʈ�� ü���� ���� ���� ����
        if (PhotonNetwork.IsMasterClient)
        {

            // ü�� �߰�
            health += newHealth;

            // �������� Ŭ���̾�Ʈ�� ����ȭ
            photonView.RPC("ApplyUpdateHealth", RpcTarget.Others, health, dead);
        }

    }

    // ��� ó��
    public virtual void Die()
    {

        // OnDeath�̺�Ʈ�� ��ϵ� �޼��尡 �ִٸ� ����
        if (onDeath != null)
        {

            onDeath();
        }

        // ��� ���¸� ������ ����
        dead = true;
    }
}
