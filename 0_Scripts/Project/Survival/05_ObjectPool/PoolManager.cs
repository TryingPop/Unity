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

        GameObject go = null;

        // _idx의 생성된 프리팹 검사
        try
        {

            if (usedPrefabs[_idx].Count > 0)
            {

                curNums[_idx]++;
                go = usedPrefabs[_idx].Pop();
                go.gameObject.SetActive(true);
                return go;
            }
        }
        catch
        {

            return null;
        }

        if (curNums[_idx] + usedPrefabs[_idx].Count >= data[_idx].maxNum ) return null;

        curNums[_idx]++;
        go = Instantiate(data[_idx].prefab, transform);
        go.transform.parent = parents[_idx];
        return go;
    }

    public void UsedPrefab(GameObject _prefab, int _idx)
    {

        curNums[_idx]--;
        if (usedPrefabs[_idx].Count < data[_idx].storageNum)
        {

            usedPrefabs[_idx].Push(_prefab);
            _prefab.gameObject.SetActive(false);
        }
        else Destroy(_prefab);
    }


    /// <summary>
    /// 등록된 프리팹인지 검사한다!
    /// </summary>
    /// <param name="_prefab"></param>
    /// <returns></returns>
    private int ChkIdx(GameObject _prefab)
    {

        int strLen = _prefab.name.Length;

        for (int i = 0; i < data.Length; i++)
        {

            // 여기 방법을 바꿔야한다
            // 갈비지가 하나씩 생성된다고 한다
            if (_prefab.name.Split("(Clone)")[0] == data[i].prefab.name)
            {

                return i;
            }
        }

        return -1;
    }

}
