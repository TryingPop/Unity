using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;

    [SerializeField] private float spawnMinTime;
    [SerializeField] private float spawnMaxTime;
    [SerializeField] private ObjCreator createScript;

    [SerializeField, Range(0, 9)] private int spawnMinNum;
    [SerializeField, Range(0, 9)] private int spawnMaxNum;

    public Transform[] spawnersTrans;

    public Transform poolTrans;

    public int maxNum;

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

        createScript = GetComponent<ObjCreator>();
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
    /// ���� �� ����
    /// </summary>
    void SetSpawnNum()
    {
        if (spawnMinNum > spawnMaxNum)
        {

            Swap(ref spawnMinNum, ref spawnMaxNum);
        }

        spawnCnt = Random.Range(Mathf.Min(spawnMinNum, spawnersTrans.Length), 
                                Mathf.Min(spawnMaxNum, spawnersTrans.Length) + 1
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

            int num = Random.Range(0, spawnersTrans.Length);

            // SetSpawnNum���� spawnerTrans ���� �ѱ� �� ������ �����ؼ�
            // 1������ �����ǰ� ����
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

        for (int i = 0; i < spawnerNum.Count; i++)
        {

            createScript.CreateObj(spawnersTrans[spawnerNum[i]].position);
        }
    }


    /// <summary>
    /// ���� �ð� ����
    /// �ּ� 0.1�� ����
    /// </summary>
    void SetSpawnTime()
    {
        if (spawnMinTime > spawnMaxTime)
        {

            Swap(ref spawnMinTime, ref spawnMaxTime);
        }

        if (spawnMinTime == spawnMaxTime)
        {

            spawnTime = spawnMinTime;
        }
        else
        {

            spawnTime = Random.Range(Mathf.Max(spawnMinTime, 0.1f), Mathf.Max(spawnMaxTime, 0.1f));
        }
    }

    /// <summary>
    /// T(���ø�)�� �̿��� a�� b�� ���� ���� ��ȯ
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
