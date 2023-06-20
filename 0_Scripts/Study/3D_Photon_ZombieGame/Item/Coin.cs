using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviourPun, IItem
{

    public int score = 200; // ������ ����

    public void Use(GameObject target)
    {

        // ���� �Ŵ����� ������ ���� �߰�
        GameManager.instance.AddScore(score);

        // ���Ǿ����� �ڽ��� �ı�
        PhotonNetwork.Destroy(gameObject);
    }
}
