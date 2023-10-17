using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyOrderManager : MonoBehaviour
{

    [SerializeField] private List<Unit> playerUnits;
    [SerializeField] private List<Unit> enemyUnits;

    [SerializeField] private List<Building> playerBuildings;
    [SerializeField] private List<Building> enemyBuildings;


    [SerializeField, Range(5f, 600f)] private float waveStartTime;
    [SerializeField, Range(5, 20)] private float thinkMinTime = 10f;
    [SerializeField, Range(20, 40)] private float thinkMaxTime = 20f;

    [SerializeField, Range(1, 10)] private short maxRandomTimes;
    private WaitForSeconds[] times;

    private Vector3[] initPos;

    private bool waveStart = false;

    private Transform target;

    [SerializeField] private int respawnEnemyNum = 2;

    // 생성 번호
    [SerializeField] private ushort[] respawnEnemySelectIdxs;
    private short[] respawnEnemyPoolIdxs;

    private ushort forcedAtkNum = 30;

    private void Awake()
    {

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

        respawnEnemyPoolIdxs = new short[respawnEnemySelectIdxs.Length];
        for (int i = 0; i < respawnEnemyPoolIdxs.Length; i++)
        {

            respawnEnemyPoolIdxs[i] = PoolManager.instance.ChkIdx(respawnEnemySelectIdxs[i]);
        }
    }

    private bool IsTargetNull()
    {

        if (target == null
            || target.gameObject.layer == VariableManager.LAYER_DEAD) return true;

        return false;
    }

    private void SetTarget()
    {

        if (playerBuildings.Count != 0)
        {

            target = playerBuildings[Random.Range(0, playerBuildings.Count)].transform;
        }
        else if (playerUnits.Count != 0)
        {

            target = playerUnits[Random.Range(0, playerUnits.Count)].transform;
        }
    }

    public void GoToPlayer()
    {

        int unitNum = enemyUnits.Count;
        if (enemyUnits.Count > ushort.MaxValue) unitNum = enemyUnits.Count;
        GiveCommand((ushort)unitNum, STATE_SELECTABLE.UNIT_ATTACK, target.position, true);
    }
    
    private void GiveCommand(ushort _num, STATE_SELECTABLE _type, Vector3 _dir, bool _isUnit)
    {

        Command cmd;
        // 명령 생성
        if (_dir == Vector3.positiveInfinity || _dir == Vector3.negativeInfinity)
        {

            // 건물 명령
            cmd = Command.GetCommand(_num, _type);
        }
        else
        {

            // 유닛 명령
            cmd = Command.GetCommand(_num, _type, _dir);
        }

        // 명령 전달인데 예약명령이 아닌 바로 실행할 명령이다!
        if (_isUnit)
        {

            for (int i = 0; i < _num; i++)
            {

                enemyUnits[i].GetCommand(cmd, false);
            }
        }
        else
        {

            for (int i = 0; i < _num; i++)
            {

                enemyBuildings[i].GetCommand(cmd, false);
            }
        }
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

    public void BuildingAction()
    {

        STATE_SELECTABLE type = (STATE_SELECTABLE)Random.Range((int)STATE_SELECTABLE.BUILDING_ACTION1, (int)STATE_SELECTABLE.BUILDING_ACTION3 + 1);

        ushort num = (ushort)enemyBuildings.Count;
        GiveCommand(num, type, Vector3.positiveInfinity, false);
    }
    
    private IEnumerator OrderStart()
    {

        waveStart = false;
        initPos = new Vector3[3];

        Vector3 pos = enemyBuildings[0].transform.position;
        for (int i = 0; i < 3; i++)
        {

            initPos[i] = SetRandPos(pos, 15f);
        }

        yield return new WaitForSeconds(waveStartTime);

        waveStart = true;

        while (GameManager.instance.IsGameOver)
        {

            if (forcedAtkNum >= VariableManager.MAX_ENEMY_UNITS) forcedAtkNum = (ushort)VariableManager.MAX_ENEMY_UNITS;

            // 강제 공격인원을 넘으면 강제 공격
            if (enemyUnits.Count > forcedAtkNum)
            {

                // 유닛이 n마리가 넘으면
                // 일정 시간마다 플레이어 강제 공격
                if (IsTargetNull()) SetTarget();
                if (target != null) GoToPlayer();
            }

            // 적 생성
            RespawnEnemy();
         
            yield return times[Random.Range(0, maxRandomTimes)];
        }
    }


    private void RespawnEnemy()
    {

        if (enemyUnits.Count >= VariableManager.MAX_ENEMY_UNITS) return;

        if (target == null) SetTarget();

        for (int i = 0; i < initPos.Length; i++)
        {

            for (int j = 0; j < respawnEnemyNum; j++)
            {

                Vector3 randPos = SetRandPos(initPos[i], 8f);

                var go = PoolManager.instance.GetPrefabs(respawnEnemyPoolIdxs[Random.Range(0, respawnEnemyPoolIdxs.Length)], VariableManager.LAYER_ENEMY, randPos);
                
                Unit unit = go.GetComponent<Unit>();
                
                if (unit) unit.AfterSettingLayer();

                if (target != null) GiveCommand(unit, STATE_SELECTABLE.UNIT_ATTACK, target.position);
            }
        }
    }

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
