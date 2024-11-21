using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 풀링 오브젝트! -> 유니티 pooling 기법을 보고 비슷하게 작성
/// </summary>
public class PoolManager : MonoBehaviour
{

    public static PoolManager instance;
    [SerializeField] private PoolingData[] data;
    private Transform[] parents;                // 해당 prefabIdx의 부모 객체
    private Stack<GameObject>[] usedPrefabs;    // 사용된 프리팹 모음집
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
            go.layer = _layer;
            return go;
        }

        // if (ChkMaxPrefab(_idx)) return null;
        curNums[_idx]++;
        go = Instantiate(data[_idx].prefab, transform);
        go.transform.parent = parents[_idx];
        go.layer = _layer;
        return go;
    }

    /// <summary>
    /// 인덱스로 데이터 정보 가져오기
    /// </summary>
    public GameObject GetData(int _idx)
    {

        if (_idx == -1) return null;

        return data[_idx].prefab;
    }

    /// <summary>
    /// 해당 좌표로 유닛 생성
    /// </summary>
    public GameObject GetPrefabs(int _idx, int _layer, Vector3 _pos)
    {

        var go = GetPrefabs(_idx, _layer);
        if (go)
        {

            go.transform.position = _pos;
        }

        return go;
    }

    /// <summary>
    /// 해당 좌표와 바라보는 방향 설정해서 좌표 obj 생성
    /// </summary>
    public GameObject GetPrefabs(int _idx, int _layer, Vector3 _pos, Vector3 _forward)
    {

        var go = GetPrefabs(_idx, _layer);
        if (go)
        {

            go.transform.position = _pos;
            go.transform.forward = _forward;
        }

        return go;
    }

    /// <summary>
    ///  다 사용 했을 경우
    /// </summary>
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
    public int ChkIdx(int _idx)
    {

        for (int i = 0; i < data.Length; i++)
        {

            // 여기 방법을 바꿔야한다
            // 갈비지가 하나씩 생성된다고 한다
            if (data[i].idx == _idx)
            {

                return i;
            }
        }

        return VarianceManager.POOLMANAGER_NOTEXIST;
    }

    

    /// <summary>
    /// 같은 오브젝트 생성
    /// </summary>
    public GameObject GetSamePrefabs(GameEntity _chkObj, int _layer, Vector3 _pos, Vector3 _forward)
    {

        int prefabIdx = ChkIdx(_chkObj.MyStat.SelectIdx);
        return GetPrefabs(prefabIdx, _layer, _pos, _forward);
    }

    /// <summary>
    /// 같은 오브젝트 생성
    /// </summary>
    public GameObject GetSamePrefabs(GameEntity _chkObj, int _layer, Vector3 _pos)
    {

        int prefabIdx = ChkIdx(_chkObj.MyStat.SelectIdx);
        return GetPrefabs(prefabIdx, _layer, _pos);
    }
}