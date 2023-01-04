using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MinimapIcon : MonoBehaviour
{

    [SerializeField] [Tooltip("플레이어 Transform")]
    private Transform targetTrans;


    public float scaleMulti;
    public float height = 10f;
    public bool onlyPosY;

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
        if (!onlyPosY)
        {
            pos.x = targetTrans.position.x;
            pos.z = targetTrans.position.z;
        }
        else
        {
            pos.x = transform.position.x;
            pos.z = transform.position.z;
        }
        pos.y = height;

        transform.position = pos;
    }

    public void SetTarget(Transform targetTrans)
    {
        this.targetTrans = targetTrans;
    }
}
