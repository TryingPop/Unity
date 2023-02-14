using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objPooling : MonoBehaviour
{

    public static objPooling instance;                      // 싱글톤

    [SerializeField] private int objMaxNum;                 // 최대 수

    public GameObject[] prefabs;
    public List<GameObject>[] objs;  // 오브젝트를 담을 리스트

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
    /// 풀에 특정 프리팹 추가
    /// </summary>
    /// <param name="idx">프리팹 번호</param>
    public GameObject CreateObj(int idx)
    {

        int chk = ChkInActiveObj(idx);
        // 먼저 비활성화 오브젝트 확인
        if (chk != -1)
        {
            objs[idx][chk].SetActive(true);
            return  objs[idx][chk];
        }
        // 풀에 여유분이 있는지 확인
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
    /// 풀에 있는 모든 개체 수
    /// </summary>
    /// <returns>모든 개체 수</returns>
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
    /// 특정 프리팹의 풀에 개체 수 확인
    /// </summary>
    /// <param name="idx">특정 프리팹의 번호</param>
    /// <returns>개체 수</returns>
    public int GetObjNum(int idx)
    {
        if (objs[idx] == null)
        {

            objs[idx] = new List<GameObject>();
        }

        return objs[idx].Count;
    }

    /// <summary>
    /// 생성할 오브젝트 변환
    /// </summary>
    /// <param name="obj">오브젝트</param>
    /// <param name="idx">바꿀 번호</param>
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
    /// 특정 프리팹의 비활성화 번호 알아내기
    /// </summary>
    /// <param name="idx">특정 프리팹의 번호</param>
    /// <returns>비활성화된 번호, -1은 없는 경우</returns>
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
    /// 풀 초기화 및 풀 안에 있는 오브젝트 파괴
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
