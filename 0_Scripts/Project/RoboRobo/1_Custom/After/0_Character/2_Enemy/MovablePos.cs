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
    /// min, max ���谡 �̻� ������ Ȯ��
    /// min > max �ΰ�� min������ ����
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
    /// ��ǥ ����
    /// </summary>
    /// <param name="posY">y��ǥ</param>
    /// <param name="dir">���� ����</param>
    public Vector3 SetPos(float posY)
    {

        ChkPos();

        float posX = Random.Range(minPosX, maxPosX);
        float posZ = Random.Range(minPosZ, maxPosZ);

        return new Vector3(posX, posY, posZ);
    }
}
