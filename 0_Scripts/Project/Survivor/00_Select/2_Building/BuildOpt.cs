using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingOpt", menuName = "Stat/BuildingOpt")]
public class BuildOpt : ScriptableObject
{

    [SerializeField] protected int buildTurn;            // ���꿡 �ɸ��� ��
    [SerializeField] protected int destroyIdx;           // �ı� ����Ʈ �ε���
    [SerializeField] protected float increaseY;             // �Ǽ� �� ���� ������ �����ֱ� ���� �޽� ������Ʈ�� ���ݾ� ��½�Ų��
    [SerializeField] protected float initPosY;

    protected int destroyPoolIdx = -1;

    public int BuildTurn => buildTurn;

    public float IncreaseY => increaseY;
    public float InitPosY => initPosY;

    public int DestroyPoolIdx
    {

        get
        {

            if (destroyPoolIdx == -1)
            {

                destroyPoolIdx = PoolManager.instance.ChkIdx(destroyIdx);
            }

            return destroyPoolIdx;
        }
    }
}