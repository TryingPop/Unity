using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class FollowTrans : MonoBehaviour
{

    [SerializeField] private Transform targetTrans;

    public float scaleMulti;        // �̴ϸ� ũ�� Ȯ��
    public float height = 10f;      // ����

    private Vector3 pos;

    private void Start()
    {
        if (scaleMulti > 0)
        {
            transform.localScale = scaleMulti * Vector3.one;
        }

        if (targetTrans == null)
        {

            targetTrans = GameObject.FindGameObjectWithTag("Player").transform;

            if (targetTrans == null)
            {

                Debug.Log("Player �±׸� ���� GameObject�� ã�� ���߽��ϴ�");
                Destroy(this);
            }
        }
    }

    private void FixedUpdate()
    {

        SetPosition();
    }

    private void SetPosition()
    {

        pos.x = targetTrans.position.x;
        pos.z = targetTrans.position.z;
        pos.y = targetTrans.position.y + height;
        transform.position = pos;
    }

    public void SetTarget(Transform targetTrans)
    {

        this.targetTrans = targetTrans;
    }
}
