using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �� ��� Ŭ����
/// </summary>
public class EnemyOrderManager : MonoBehaviour
{

    [SerializeField] private CommandGroup playerUnits;
    [SerializeField] private CommandGroup enemyUnits;

    [SerializeField] private CommandGroup playerBuildings;
    [SerializeField] private CommandGroup enemyBuildings;


    [SerializeField, Range(5f, 600f)] private float waveStartTime;          // �� Ȱ�� ���� �ð�
    [SerializeField, Range(5, 20)] private float thinkMinTime = 10f;        // �ּ� �ݺ� ����
    [SerializeField, Range(20, 40)] private float thinkMaxTime = 20f;       // �ִ� �ݺ� ����

    [SerializeField, Range(1, 10)] private int maxRandomTimes;            // ������ �ð��� ����
    private WaitForSeconds[] times;                                         // ������ �ð�

    private Vector3[] initPos;                                              // ���� ��ġ

    public bool waveStart = false;                                         // Ȯ�ο�

    private Transform target;                                               // ���� ���

    [SerializeField] private int respawnEnemyNum = 2;                       // ���� �������� ������ ����
    [SerializeField] private int addTurn;
    private int curTurn;

    // ���� ��ȣ
    [SerializeField] private int[] respawnEnemySelectIdxs;               // ������ �� idx
    private int[] respawnEnemyPoolIdxs;                                   

    private int forcedAtkNum = 30;                                       // ���� ������ ����

    [SerializeField] private ScriptGroup attackScript;

    private void Awake()
    {

        // ���� ���� �ð� ����(ĳ��)
        times = new WaitForSeconds[maxRandomTimes];
        for (int i = 0; i < maxRandomTimes; i++)
        {

            float time = Random.Range(thinkMinTime, thinkMaxTime);
            times[i] = new WaitForSeconds(time);
        }
    }

    private void Start()
    {

        enemyUnits = ActionManager.instance.EnemyUnits;
        playerUnits = ActionManager.instance.PlayerUnits;
        playerBuildings = ActionManager.instance.PlayerBuildings;
        enemyBuildings = ActionManager.instance.EnemyBuildings;

        StartCoroutine(OrderStart());

        respawnEnemyPoolIdxs = new int[respawnEnemySelectIdxs.Length];
        for (int i = 0; i < respawnEnemyPoolIdxs.Length; i++)
        {

            respawnEnemyPoolIdxs[i] = PoolManager.instance.ChkIdx(respawnEnemySelectIdxs[i]);
        }
    }

    /// <summary>
    /// Ÿ���� �׾��ų� ���� �� Ȯ��
    /// </summary>
    /// <returns></returns>
    private bool IsTargetNull()
    {

        if (target == null
            || target.gameObject.layer == VarianceManager.LAYER_DEAD) return true;

        return false;
    }

    /// <summary>
    /// ���ο� Ÿ�� ���� �ǹ� > ���� ������
    /// </summary>
    private void SetTarget()
    {

        if (playerBuildings.Count != 0)
        {

            target = playerBuildings.First?.transform;
        }
        else if (playerUnits.Count != 0)
        {

            target = playerUnits.First?.transform;
        }
    }

    /// <summary>
    /// command �� �̿��� �÷��̾�� ����!
    /// </summary>
    public void GoToPlayer()
    {

        int unitNum = enemyUnits.Count;
        if (enemyUnits.Count > ushort.MaxValue) unitNum = enemyUnits.Count;
        GiveCommand((ushort)unitNum, STATE_SELECTABLE.UNIT_ATTACK, target.position, true);
    }
    
    /// <summary>
    /// ���� ��� �ֱ�!
    /// </summary>
    private void GiveCommand(ushort _num, STATE_SELECTABLE _type, Vector3 _dir, bool _isUnit)
    {

        Command cmd;
        // ��� ����
        if (_dir == Vector3.positiveInfinity || _dir == Vector3.negativeInfinity)
        {

            // �ǹ� ���
            cmd = Command.GetCommand(_num, _type);
        }
        else
        {

            // ���� ���
            cmd = Command.GetCommand(_num, _type, _dir);
        }

        // ��� �����ε� �������� �ƴ� �ٷ� ������ ����̴�!
        if (_isUnit)
            enemyUnits.GetCommand(cmd, false);
        else
            enemyBuildings.First.GetCommand(cmd, false);
    }

    private void GiveCommand(Unit _unit, STATE_SELECTABLE _type, Vector3 _dir)
    {

        Command cmd;

        if (_dir == Vector3.positiveInfinity || _dir == Vector3.negativeInfinity)
        {

            cmd = Command.GetCommand(1, _type);
        }
        else
        {

            cmd = Command.GetCommand(1, _type, _dir);
        }

        _unit.GetCommand(cmd, true);
    }

    /// <summary>
    /// �Ǹ����� �ൿ
    /// </summary>
    public void BuildingAction()
    {

        STATE_SELECTABLE type = (STATE_SELECTABLE)Random.Range((int)STATE_SELECTABLE.BUILDING_ACTION1, (int)STATE_SELECTABLE.BUILDING_ACTION3 + 1);

        ushort num = (ushort)enemyBuildings.Count;
        GiveCommand(num, type, Vector3.positiveInfinity, false);
    }
    
    /// <summary>
    /// �Ǹ����� ���� �� ���� �ݺ�
    /// </summary>
    private IEnumerator OrderStart()
    {

        waveStart = false;
        initPos = new Vector3[3];

        Vector3 pos = enemyBuildings.First.transform.position;
        for (int i = 0; i < 3; i++)
        {

            initPos[i] = SetRandPos(pos, 15f);
        }


        // ó�� ��� �ð�
        yield return new WaitForSeconds(waveStartTime);

        // Script ���� �˸���!
        waveStart = true;
        UIManager.instance.SetScripts(attackScript.Scripts);

        while (!GameManager.instance.IsGameOver)
        {

            if (forcedAtkNum >= VarianceManager.MAX_ENEMY_UNITS) forcedAtkNum = (ushort)VarianceManager.MAX_ENEMY_UNITS;

            // ���� �����ο��� ������ ���� ����
            if (enemyUnits.Count > forcedAtkNum)
            {

                // ������ n������ ������
                // ���� �ð����� �÷��̾� ���� ����
                if (IsTargetNull()) SetTarget();
                if (target != null) GoToPlayer();
            }

            // �� ����
            RespawnEnemy();
         
            yield return times[Random.Range(0, maxRandomTimes)];
        }
    }


    /// <summary>
    /// �� ����
    /// </summary>
    private void RespawnEnemy()
    {

        if (enemyUnits.Count >= VarianceManager.MAX_ENEMY_UNITS) return;

        if (target == null) SetTarget();



        for (int i = 0; i < initPos.Length; i++)
        {

            for (int j = 0; j < respawnEnemyNum; j++)
            {

                Vector3 randPos = SetRandPos(initPos[i], 8f);

                var go = PoolManager.instance.GetPrefabs(respawnEnemyPoolIdxs[Random.Range(0, respawnEnemyPoolIdxs.Length)], VarianceManager.LAYER_ENEMY, randPos);
                
                Unit unit = go.GetComponent<Unit>();
                
                if (target != null) GiveCommand(unit, STATE_SELECTABLE.UNIT_ATTACK, target.position);
            }
        }

        if (respawnEnemyNum < VarianceManager.MAX_ENEMY_UNITS)
        {

            curTurn++;
            if (curTurn > addTurn)
            {

                curTurn = 0;
                respawnEnemyNum++;
            }
        }
    }

    /// <summary>
    /// �ֺ��� ���� ��ǥ ã��
    /// </summary>
    private Vector3 SetRandPos(Vector3 _randPos, float _range)
    {

        Vector3 randPos;
        while (true)
        {

            randPos = _randPos;
            randPos += Random.insideUnitSphere * _range;

            if (NavMesh.SamplePosition(randPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {

                return hit.position;
            }
        }
    }
}
