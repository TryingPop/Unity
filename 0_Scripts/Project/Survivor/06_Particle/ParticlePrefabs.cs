using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 비활성화 되면 자동으로 소모되게 해준다
/// </summary>
public class ParticlePrefabs : MonoBehaviour
{

    [SerializeField] private int prefabIdx;
    protected int poolIdx = -1;

    public int PoolIdx
    {

        get
        {

            if (poolIdx == -1)
            {

                poolIdx = PoolManager.instance.ChkIdx(prefabIdx);
            }

            return poolIdx;
        }
    }



    private void OnDisable()
    {

        PoolManager.instance.UsedPrefab(gameObject, PoolIdx);
    }
}
