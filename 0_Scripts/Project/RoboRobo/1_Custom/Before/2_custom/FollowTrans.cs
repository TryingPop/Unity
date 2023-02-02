using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class FollowTrans : MonoBehaviour
{

    [SerializeField] private Transform targetTrans;

    public float scaleMulti;        // 미니맵 크기 확대
    public float height = 10f;      // 높이

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

                Debug.Log("Player 태그를 가진 GameObject를 찾지 못했습니다");
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
