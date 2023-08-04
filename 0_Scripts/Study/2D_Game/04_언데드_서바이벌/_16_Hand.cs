using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class _16_Hand : MonoBehaviour
{

    // 왼손 오른손 구분
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0f);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0f);

    Quaternion leftRot = Quaternion.Euler(0f, 0f, -35f);
    Quaternion leftRotReverse = Quaternion.Euler(0f, 0f, -125f);

    private void Awake()
    {

        player = GetComponentsInParent<SpriteRenderer>()[1];
    }


    private void LateUpdate()
    {

        bool isReverse = player.flipX;

        if (isLeft)
        {

            // 근접 무기
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {

            // 원거리 무기
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}