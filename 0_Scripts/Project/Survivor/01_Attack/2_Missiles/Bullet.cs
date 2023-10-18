using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private TrailRenderer myTrail;     // �̵��� ǥ��

    private void OnEnable()
    {

        myTrail.Clear();                                // ��Ȱ�� �� �� ����� ȿ�� ���ֱ� ���� ���� ��ũ��Ʈ
        myTrail.enabled = true;
    }

    private void OnDisable()
    {

        myTrail.enabled = false;
    }
}
