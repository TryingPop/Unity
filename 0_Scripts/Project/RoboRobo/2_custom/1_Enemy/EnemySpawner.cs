using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;                    // 싱글톤

    [SerializeField] private float spawnMinTime;            // 최소 스폰시간
    [SerializeField] private float spawnMaxTime;            // 최대 스폰시간
    [SerializeField] private ObjCreator createScript;       // ObjCreator에서 생성하므로 가져온다

    [SerializeField, Range(0, 9)] private int spawnMinNum;  // 최소 스폰 수
    [SerializeField, Range(0, 9)] private int spawnMaxNum;  // 최대 스폰 수

    public Transform[] spawnersTrans;                       // 스폰 위치

    public Transform poolTrans;                             // 하이라키에서 생성 될 장소 

    private List<int> spawnerNum;                           // 생성 시에 사용할 리스트
    private float spawnTime;                                // 스폰 시간
    private int spawnCnt;                                   // 스폰 수

    private void Awake()
    {

        // 싱글톤 할당
        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }

        // 컴포넌트 가져오기
        createScript = GetComponent<ObjCreator>();
        spawnerNum = new List<int>();
    }


    private void Start()
    {

        // 자동으로 생성 시작
        StartCoroutine(StartCreating());
    }

    /// <summary>
    /// 적 생성 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCreating()
    {

        // 실행 중에만 생성
        while (GameManager.instance.state == GameManager.GAMESTATE.Play)
        {

            // 생성될 유닛 수 설정
            SetSpawnNum();

            // 생성될 유닛 수에 맞춰 생성 장소 설정
            SetSpawners();

            // 적 생성
            Spawning();

            // 대기 시간 설정
            SetSpawnTime();

            // 설정된 대기 시간동안 대기
            yield return new WaitForSeconds(spawnTime);
        }
    }



    /// <summary>
    /// 생성될 유닛 수 설정
    /// </summary>
    void SetSpawnNum()
    {

        // 최소 수가 최대 수보다 많은 경우 서로 값 바꾼다
        if (spawnMinNum > spawnMaxNum)
        {

            Swap(ref spawnMinNum, ref spawnMaxNum);
        }

        // 최소 값과 최대 값 사이의 랜덤값
        // spawnCnt는 생성 장소보다 항상 적거나 같다
        spawnCnt = Random.Range(Mathf.Min(spawnMinNum, spawnersTrans.Length), 
                                Mathf.Min(spawnMaxNum, spawnersTrans.Length) + 1
                                );
    }

    /// <summary>
    /// 앞에서 설정한 숫자에 맞춰 스포너 설정
    /// </summary>
    void SetSpawners()
    {

        // 리스트 비우기
        spawnerNum.Clear();

        // spawnCnt가 생성 장소보다 항상 적거나 같기에 무한루프에 걸릴 가능성은 없다
        while (spawnerNum.Count < spawnCnt)
        {

            // 임의 번호 선택
            int num = Random.Range(0, spawnersTrans.Length);

            // 중복 값 확인을 list로 해서 중복값인지 확인하고
            // 리스트에 추가
            // set 자료구조 이용하면 바로 add하면 된다
            if (!spawnerNum.Contains(num))
            {

                spawnerNum.Add(num);
            }
        }
    }

    /// <summary>
    /// 적 생성
    /// </summary>
    private void Spawning()
    {

        // 앞에서 설정된 좌표에 생성
        for (int i = 0; i < spawnerNum.Count; i++)
        {

            // 유닛 생성
            createScript.CreateObj(spawnersTrans[spawnerNum[i]].position);
        }
    }


    /// <summary>
    /// 스폰 시간 설정
    /// 최소 0.1값 보정
    /// </summary>
    void SetSpawnTime()
    {

        // 최소 시간이 최대 시간 보다 큰 경우 값을 바꾼다
        if (spawnMinTime > spawnMaxTime)
        {

            Swap(ref spawnMinTime, ref spawnMaxTime);
        }

        // 최소 시간 0.1f
        if (spawnMinTime == spawnMaxTime)
        {

            spawnTime = Mathf.Max(spawnMinTime, 0.1f);
        }
        else
        {

            spawnTime = Random.Range(Mathf.Max(spawnMinTime, 0.1f), Mathf.Max(spawnMaxTime, 0.1f));
        }
    }

    /// <summary>
    /// T(템플릿)을 이용해 a과 b의 값을 서로 교환
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    static void Swap<T>(ref T a,  ref T b)
    {

        T temp = a;
        a = b;
        b = temp;
    }
}
