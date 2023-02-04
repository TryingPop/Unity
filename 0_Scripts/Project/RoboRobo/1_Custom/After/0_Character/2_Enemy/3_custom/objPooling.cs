using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objPooling : MonoBehaviour
{

    public static objPooling instance;                      // �̱���

    [SerializeField] private int objMaxNum;                 // �ִ� ��

    public GameObject[] prefabs;
    public List<GameObject>[] objs;  // ������Ʈ�� ���� ����Ʈ

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

        objs = new List<GameObject>[prefabs.Length];
    }


    /// <summary>
    /// Ǯ�� Ư�� ������ �߰�
    /// </summary>
    /// <param name="idx">������ ��ȣ</param>
    public GameObject CreateObj(int idx)
    {

        int chk = ChkInActiveObj(idx);
        // ���� ��Ȱ��ȭ ������Ʈ Ȯ��
        if (chk != -1)
        {
            objs[idx][chk].SetActive(true);
            return  objs[idx][chk];
        }
        // Ǯ�� �������� �ִ��� Ȯ��
        else if (GetObjNum(idx) < objMaxNum) 
        {

            GameObject obj = Instantiate(prefabs[idx], transform);
            objs[idx].Add(obj);
            return obj;
        }
        else
        {

            return null;
        }
    }

    /// <summary>
    /// Ǯ�� �ִ� ��� ��ü ��
    /// </summary>
    /// <returns>��� ��ü ��</returns>
    public int GetObjsNum()
    {

        int total = 0;

        for (int i = 0; i < objs.Length; i++)
        {

            total += objs[i].Count;
        }

        return total;
    }

    /// <summary>
    /// Ư�� �������� Ǯ�� ��ü �� Ȯ��
    /// </summary>
    /// <param name="idx">Ư�� �������� ��ȣ</param>
    /// <returns>��ü ��</returns>
    public int GetObjNum(int idx)
    {
        if (objs[idx] == null)
        {

            objs[idx] = new List<GameObject>();
        }

        return objs[idx].Count;
    }

    /// <summary>
    /// ������ ������Ʈ ��ȯ
    /// </summary>
    /// <param name="obj">������Ʈ</param>
    /// <param name="idx">�ٲ� ��ȣ</param>
    public void SetPrefab(GameObject obj, int idx)
    {

        prefabs[idx] = obj;
    }

    public void SetPrefabs(GameObject[] obj)
    {

        prefabs = obj;
        ClearObjs();
        objs = new List<GameObject>[prefabs.Length];
    }
    
    public bool ChkSummon()
    {

        if (GetObjsNum() < objMaxNum)
        {

            return true;
        }
        
        for (int i = 0; i < objs.Length; i++)
        {

            if (ChkInActiveObj(i) != -1)
            {

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Ư�� �������� ��Ȱ��ȭ ��ȣ �˾Ƴ���
    /// </summary>
    /// <param name="idx">Ư�� �������� ��ȣ</param>
    /// <returns>��Ȱ��ȭ�� ��ȣ, -1�� ���� ���</returns>
    private int ChkInActiveObj(int idx)
    {

        for (int i = 0; i < GetObjNum(idx); i++)
        {

            if (!objs[idx][i].activeSelf)
            {

                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Ǯ �ʱ�ȭ �� Ǯ �ȿ� �ִ� ������Ʈ �ı�
    /// </summary>
    private void ClearObjs()
    {
        GameObject obj;

        for (int i = 0; i < objs.Length; i++)
        {

            for (int j = 0; j < objs[i].Count; j++)
            {

                obj = objs[i][0];
                objs[i].Remove(objs[i][0]);
                Destroy(obj);
            }
        }
    }
}
