using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ź���� �����ϴ� ������
public class AmmoPack : MonoBehaviourPun, IItem
{

    public int ammo = 30;           // ������ ź�� ��

    public void Use(GameObject target)
    {

        // target�� ź���� �߰��ϴ� ó��
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        // Playershooter ������Ʈ�� ������ �� ������Ʈ�� �����ϸ�
        if (playerShooter != null && playerShooter.gun != null)
        {

            // ���� ���� źȯ ���� ammo ��ŭ ���ϱ�, ��� Ŭ���̾�Ʈ���� ����
            playerShooter.gun.photonView.RPC("AddAmmo", RpcTarget.All, ammo);
        }

        // ���Ǿ����Ƿ� �ڽ��� �ı�
        PhotonNetwork.Destroy(gameObject);
    }
}
