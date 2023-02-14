using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;                    // �̱���

    [SerializeField] private float spawnMinTime;            // �ּ� �����ð�
    [SerializeField] private float spawnMaxTime;            // �ִ� �����ð�
    [SerializeField] private ObjCreator createScript;       // ObjCreator���� �����ϹǷ� �����´�

    [SerializeField, Range(0, 9)] private int spawnMinNum;  // �ּ� ���� ��
    [SerializeField, Range(0, 9)] private int spawnMaxNum;  // �ִ� ���� ��

    public Transform[] spawnersTrans;                       // ���� ��ġ

    public Transform poolTrans;                             // ���̶�Ű���� ���� �� ��� 

    private List<int> spawnerNum;                           // ���� �ÿ� ����� ����Ʈ
    private float spawnTime;                                // ���� �ð�
    private int spawnCnt;                                   // ���� ��

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

        // ������Ʈ ��������
        createScript = GetComponent<ObjCreator>();
        spawnerNum = new List<int>();
    }


    private void Start()
    {

        // �ڵ����� ���� ����
        StartCoroutine(StartCreating());
    }

    /// <summary>
    /// �� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCreating()
    {

        // ���� �߿��� ����
        while (GameManager.instance.state == GameManager.GAMESTATE.Play)
        {

            // ������ ���� �� ����
            SetSpawnNum();

            // ������ ���� ���� ���� ���� ��� ����
            SetSpawners();

            // �� ����
            Spawning();

            // ��� �ð� ����
            SetSpawnTime();

            // ������ ��� �ð����� ���
            yield return new WaitForSeconds(spawnTime);
        }
    }



    /// <summary>
    /// ������ ���� �� ����
    /// </summary>
    void SetSpawnNum()
    {

        // �ּ� ���� �ִ� ������ ���� ��� ���� �� �ٲ۴�
        if (spawnMinNum > spawnMaxNum)
        {

            Swap(ref spawnMinNum, ref spawnMaxNum);
        }

        // �ּ� ���� �ִ� �� ������ ������
        // spawnCnt�� ���� ��Һ��� �׻� ���ų� ����
        spawnCnt = Random.Range(Mathf.Min(spawnMinNum, spawnersTrans.Length), 
                                Mathf.Min(spawnMaxNum, spawnersTrans.Length) + 1
                                );
    }

    /// <summary>
    /// �տ��� ������ ���ڿ� ���� ������ ����
    /// </summary>
    void SetSpawners()
    {

        // ����Ʈ ����
        spawnerNum.Clear();

        // spawnCnt�� ���� ��Һ��� �׻� ���ų� ���⿡ ���ѷ����� �ɸ� ���ɼ��� ����
        while (spawnerNum.Count < spawnCnt)
        {

            // ���� ��ȣ ����
            int num = Random.Range(0, spawnersTrans.Length);

            // �ߺ� �� Ȯ���� list�� �ؼ� �ߺ������� Ȯ���ϰ�
            // ����Ʈ�� �߰�
            // set �ڷᱸ�� �̿��ϸ� �ٷ� add�ϸ� �ȴ�
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

        // �տ��� ������ ��ǥ�� ����
        for (int i = 0; i < spawnerNum.Count; i++)
        {

            // ���� ����
            createScript.CreateObj(spawnersTrans[spawnerNum[i]].position);
        }
    }


    /// <summary>
    /// ���� �ð� ����
    /// �ּ� 0.1�� ����
    /// </summary>
    void SetSpawnTime()
    {

        // �ּ� �ð��� �ִ� �ð� ���� ū ��� ���� �ٲ۴�
        if (spawnMinTime > spawnMaxTime)
        {

            Swap(ref spawnMinTime, ref spawnMaxTime);
        }

        // �ּ� �ð� 0.1f
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
