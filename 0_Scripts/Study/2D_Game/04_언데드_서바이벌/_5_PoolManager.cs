using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class _5_PoolManager : MonoBehaviour
{

    // �����յ��� ������ ����
    public GameObject[] prefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools;

    private void Awake()
    {

        pools = new List<GameObject>[prefabs.Length];


        for (int index = 0; index < pools.Length; index++)
        {

            pools[index] = new List<GameObject>();
        }
        // Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {

        GameObject select = null;

        // ������ Ǯ�� ��� �ִ� ���� ������Ʈ ����
        foreach (GameObject item in pools[index])
        {

            if (!item.activeSelf)
            {

                // �߰��ϸ� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }
        }

        
        if (!select)
        {

            // ������ ���Ӱ� ������ select�� �ֱ�
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}