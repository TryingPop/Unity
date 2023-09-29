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

        // �θ� ������Ʈ ����
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
    /// ���� �޼���
    /// </summary>
    /// <param name="_idx"></param>
    /// <param name="_layer"></param>
    /// <returns></returns>
    public GameObject GetPrefabs(int _idx, int _layer)
    {

        if (_idx == -1) return null;
        GameObject go = null;

        // _idx�� ������ ������ �˻�
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
    /// �ε����� �������� �ִ��� �˻��Ѵ�!
    /// -1�ΰ�� ���� X
    /// </summary>
    /// <returns>���� �ε���</returns>
    public short ChkIdx(ushort _idx)
    {

        for (short i = 0; i < data.Length; i++)
        {

            // ���� ����� �ٲ���Ѵ�
            // �������� �ϳ��� �����ȴٰ� �Ѵ�
            if (data[i].idx == _idx)
            {

                return i;
            }
        }

        return VariableManager.POOLMANAGER_NOTEXIST;
    }

    /// <summary>
    /// ���� ������Ʈ ����
    /// </summary>
    public GameObject GetSamePrefabs(Selectable _chkObj, int _layer)
    {

        int prefabIdx = ChkIdx(_chkObj.MyStat.SelectIdx);
        return GetPrefabs(prefabIdx, _layer);
    }
}