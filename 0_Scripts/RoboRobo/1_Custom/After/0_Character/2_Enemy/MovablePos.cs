using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePos : MonoBehaviour
{

    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;

    [SerializeField] private float minPosZ;
    [SerializeField] private float maxPosZ;


    /// <summary>
    /// min, max 관계가 이상 없는지 확인
    /// min > max 인경우 min값으로 설정
    /// </summary>
    private void ChkPos()
    {

        if (minPosX > maxPosX)
        {

            minPosX = maxPosX;
        }

        if (minPosZ > maxPosZ)
        {

            minPosZ = maxPosZ;
        }
    }

    /// <summary>
    /// 좌표 설정
    /// </summary>
    /// <param name="posY">y좌표</param>
    /// <param name="dir">담을 벡터</param>
    public Vector3 SetPos(float posY)
    {

        ChkPos();

        float posX = Random.Range(minPosX, maxPosX);
        float posZ = Random.Range(minPosZ, maxPosZ);

        return new Vector3(posX, posY, posZ);
    }
}
