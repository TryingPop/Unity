using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 풀링 할 데이터
/// </summary>
[System.Serializable]
public class PoolingData
{

    public ushort idx;              // selectedIdx랑 같다
    public ushort storageNum;       // 최대 저장 수 넘어가면 파괴!
    public GameObject prefab;       // 풀링 오브젝트
}   