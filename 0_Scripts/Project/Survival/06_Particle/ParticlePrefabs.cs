using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ȱ��ȭ �Ǹ� �ڵ����� �Ҹ�ǰ� ���ش�
/// </summary>
public class ParticlePrefabs : MonoBehaviour
{

    [SerializeField] private ushort prefabIdx;
    protected short poolIdx = -1;

    public short PoolIdx
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
