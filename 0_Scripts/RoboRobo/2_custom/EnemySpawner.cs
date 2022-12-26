using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;

    [Tooltip("������ ���")]
    public Transform[] enemySpawners;

    [SerializeField] [Tooltip("������ ��")]
    private GameObject[] enemyPrefabs;

    [SerializeField] [Tooltip("�ּ� ���� �ð�")]
    private float spawnMinTime;

    [SerializeField] [Tooltip("�ִ� ���� �ð�")]
    private float spawnMaxTime;

    [SerializeField] [Tooltip("�ּ� ���� ����")]
    [Range(0, 5)]
    private int spawnMinNum;

    [SerializeField] [Tooltip("�ִ� ���� ����")]
    [Range(5, 9)]
    private int spawnMaxNum;

    [SerializeField] [Tooltip("Enemy Pooling")]
    private Transform enemyPool;

    private List<int> spawnerNum;
    private float spawnTime;
    private int spawnCnt;

    private void Awake()
    {

        // �̱��� �Ҵ�
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
    /// �� ���� �ڷ�ƾ
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
    /// ���� �� ����
    /// </summary>
    void SetSpawnNum()
    {

        spawnCnt = Random.Range(Mathf.Min(spawnMinNum, enemySpawners.Length), 
                                Mathf.Max(spawnMaxNum, enemySpawners.Length) + 1
                                );
    }

    /// <summary>
    /// �տ��� ������ ���ڿ� ���� ������ ����
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
    /// �� ����
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
    /// ���� �ð� ����
    /// �ּ� 0.1�� ����
    /// </summary>
    void SetSpawnTime()
    {

        spawnTime = Random.Range(Mathf.Max(spawnMinTime, 0.1f), Mathf.Max(spawnMaxTime,0.1f));
    }
}
