using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateUnits : BaseGameEvent
{

    [SerializeField] protected int selectedIdx;
    [SerializeField] protected int unitNum;
    [SerializeField] protected int layer;
    [SerializeField] protected Vector3 initPos;
    [Tooltip("배치 간격")]
    [SerializeField] protected int size;

    public override void InitalizeEvent()
    {

        int idx = PoolManager.instance.ChkIdx(selectedIdx);

        if (idx == VarianceManager.POOLMANAGER_NOTEXIST) return;

        Vector3 pos = initPos;

        for (int i = 1; i <= unitNum; i++)
        {

            Command.SetNextPos(size, i, ref pos);
            var go = PoolManager.instance.GetPrefabs(idx, layer, pos);
        }
    }
}
