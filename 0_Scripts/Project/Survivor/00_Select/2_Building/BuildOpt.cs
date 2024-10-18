using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingOpt", menuName = "Stat/BuildingOpt")]
public class BuildOpt : ScriptableObject
{

    [SerializeField] protected int buildTurn;            // 생산에 걸리는 턴
    [SerializeField] protected int destroyIdx;           // 파괴 이펙트 인덱스
    [SerializeField] protected float increaseY;             // 건설 시 진행 정도를 보여주기 위해 메시 오브젝트를 조금씩 상승시킨다
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