using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;

    [SerializeField] private float spawnMinTime;
    [SerializeField] private float spawnMaxTime;
    [SerializeField] private ObjCreator createScript;

    [SerializeField, Range(0, 5)] private int spawnMinNum;
    [SerializeField, Range(5, 9)] private int spawnMaxNum;

    public Transform[] spawnersTrans;

    public Transform poolTrans;

    public int maxNum;

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

        createScript = GetComponent<ObjCreator>();
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
        while (GameManager.instance.state == GameManager.GAMESTATE.Play)
        {
            SetSpawnNum();

            SetSpawners();

            Spawning();

            SetSpawnTime();

            yield return new WaitForSeconds(spawnTime);
        }
    }



    /// <summary>
    /// 스폰 수 설정
    /// </summary>
    void SetSpawnNum()
    {

        spawnCnt = Random.Range(Mathf.Min(spawnMinNum, spawnersTrans.Length), 
                                Mathf.Min(spawnMaxNum, spawnersTrans.Length) + 1
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

            int num = Random.Range(0, spawnersTrans.Length);

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

        for (int i = 0; i < spawnerNum.Count; i++)
        {

            createScript.CreateObj(spawnersTrans[spawnerNum[i]].position);
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
