using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;

    [Tooltip("생성할 장소")]
    public Transform[] enemySpawners;

    [SerializeField] [Tooltip("생성할 적")]
    private GameObject[] enemyPrefabs;

    [SerializeField] [Tooltip("최소 생성 시간")]
    private float spawnMinTime;

    [SerializeField] [Tooltip("최대 생성 시간")]
    private float spawnMaxTime;

    [SerializeField] [Tooltip("최소 생성 개수")]
    [Range(0, 5)]
    private int spawnMinNum;

    [SerializeField] [Tooltip("최대 생성 개수")]
    [Range(5, 9)]
    private int spawnMaxNum;

    [SerializeField] [Tooltip("Enemy Pooling")]
    private Transform enemyPool;

    private List<int> spawnerNum;
    private float spawnTime;
    private int spawnCnt;

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
    }


    private void Start()
    {

        StartCoroutine(StartCreating());
    }

    /// <summary>
    /// 적 생성 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCreating()
    {

        SetSpawnNum();

        SetSpawners();

        Spawning();

        SetSpawnTime();

        yield return new WaitForSeconds(spawnTime);
    }



    /// <summary>
    /// 스폰 수 설정
    /// </summary>
    void SetSpawnNum()
    {

        spawnCnt = Random.Range(Mathf.Min(spawnMinNum, enemySpawners.Length), 
                                Mathf.Max(spawnMaxNum, enemySpawners.Length) + 1
                                );
    }

    /// <summary>
    /// 앞에서 설정한 숫자에 맞춰 스포너 설정
    /// </summary>
    void SetSpawners()
    {

        spawnerNum = new List<int>();


        while (spawnerNum.Count < spawnCnt)
        {

            int num = Random.Range(0, enemySpawners.Length);

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
        foreach (var item in enemySpawners)
        {

            int num = Random.Range(0, enemyPrefabs.Length);

            Instantiate(enemyPrefabs[num], item.position, Quaternion.identity);
        }
    }


    /// <summary>
    /// 스폰 시간 설정
    /// 최소 0.1값 보정
    /// </summary>
    void SetSpawnTime()
    {

        spawnTime = Random.Range(Mathf.Max(spawnMinTime, 0.1f), Mathf.Max(spawnMaxTime,0.1f));
    }
}
