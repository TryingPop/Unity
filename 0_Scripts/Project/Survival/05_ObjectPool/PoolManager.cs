using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolManager : MonoBehaviour
{

    public static PoolManager instance;
    [SerializeField] private PoolingData[] data;
    private Transform[] parents;
    private Stack<GameObject>[] usedPrefabs;
    [SerializeField] private int[] curNums;

    
    private void Awake()
    {

        if (instance != null)
        {

            Destroy(gameObject);
            return;
        }

        instance = this;

        int len = data.Length;

        parents = new Transform[len];

        // 부모 오브젝트 생성
        for (int i = 0; i < len; i++)
        {

            GameObject go = new GameObject(data[i].idx.ToString());
            go.transform.parent = transform;
            parents[i] = go.transform;
        }

        usedPrefabs = new Stack<GameObject>[len];

        for (int i = 0; i < len; i++)
        {

            usedPrefabs[i] = new Stack<GameObject>(data[i].storageNum);
        }

        curNums = new int[len];
    }


    /// <summary>
    /// 생성 메서드
    /// </summary>
    /// <param name="_idx"></param>
    /// <param name="_layer"></param>
    /// <returns></returns>
    public GameObject GetPrefabs(int _idx, int _layer)
    {

        if (_idx == -1) return null;
        GameObject go = null;

        // _idx의 생성된 프리팹 검사
        if (usedPrefabs[_idx].Count > 0)
        {

            curNums[_idx]++;
            go = usedPrefabs[_idx].Pop();
            go.gameObject.SetActive(true);
            return go;
        }

        // if (ChkMaxPrefab(_idx)) return null;
        curNums[_idx]++;
        go = Instantiate(data[_idx].prefab, transform);
        go.transform.parent = parents[_idx];
        return go;
    }

    public void UsedPrefab(GameObject _prefab, int _idx)
    {

        if (_idx == -1)
        {

            Destroy(_prefab);
            return;
        }

        curNums[_idx]--;
        if (usedPrefabs[_idx].Count < data[_idx].storageNum)
        {

            usedPrefabs[_idx].Push(_prefab);
            _prefab.gameObject.SetActive(false);
        }
        else Destroy(_prefab);
    }


    /// <summary>
    /// 인덱스로 프리팹이 있는지 검사한다!
    /// -1인경우 존재 X
    /// </summary>
    /// <returns>현재 인덱스</returns>
    public short ChkIdx(ushort _idx)
    {

        for (short i = 0; i < data.Length; i++)
        {

            // 여기 방법을 바꿔야한다
            // 갈비지가 하나씩 생성된다고 한다
            if (data[i].idx == _idx)
            {

                return i;
            }
        }

        return VariableManager.POOLMANAGER_NOTEXIST;
    }

    /// <summary>
    /// 같은 오브젝트 생성
    /// </summary>
    public GameObject GetSamePrefabs(Selectable _chkObj, int _layer)
    {

        int prefabIdx = ChkIdx(_chkObj.MyStat.SelectIdx);
        return GetPrefabs(prefabIdx, _layer);
    }
}