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

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }

        int len = data.Length;
        parents = new Transform[len];

        for (int i = 0; i < len; i++)
        {

            GameObject go = new GameObject(data[i].prefab.name);
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

        GameObject go = null;

        // _idx의 생성된 프리팹 검사
        try
        {

            if (usedPrefabs[_idx].Count > 0)
            {

                curNums[_idx]++;
                go = usedPrefabs[_idx].Pop();
                go.SetActive(true);
                return go;
            }
        }
        catch
        {

            Debug.LogError("풀 매니저 인덱스 에러");
            return null;
        }

        if (curNums[_idx] + usedPrefabs[_idx].Count >= data[_idx].maxNum ) return null;

        curNums[_idx]++;
        go = Instantiate(data[_idx].prefab, transform);
        go.transform.parent = parents[_idx];
        return go;

    }

    public void UsedPrefab(GameObject _prefab)
    {

        int idx = ChkIdx(_prefab);

        if (idx == -1)
        {

            Debug.LogError("풀 매니저에 등록되지 않은 프리팹입니다.");
            return;
        }

        curNums[idx]--;
        if (usedPrefabs[idx].Count < data[idx].storageNum)
        {

            usedPrefabs[idx].Push(_prefab);
            _prefab.SetActive(false);
        }
        else Destroy(_prefab);
    }


    private int ChkIdx(GameObject _prefab)
    {

        int strLen = _prefab.name.Length;
        for (int i = 0; i < data.Length; i++)
        {

            if (data[i].prefab.name == _prefab.name.Split("(Clone)")[0])
            {

                return i;
            }
        }

        return -1;
    }

}
