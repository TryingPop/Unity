using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 명령 클래스
/// </summary>
public class EnemyOrderManager : MonoBehaviour
{

    [SerializeField] private CommandGroup playerUnits;
    [SerializeField] private CommandGroup enemyUnits;

    [SerializeField] private CommandGroup playerBuildings;
    [SerializeField] private CommandGroup enemyBuildings;


    [SerializeField, Range(5f, 600f)] private float waveStartTime;          // 적 활동 시작 시간
    [SerializeField, Range(5, 20)] private float thinkMinTime = 10f;        // 최소 반복 간격
    [SerializeField, Range(20, 40)] private float thinkMaxTime = 20f;       // 최대 반복 간격

    [SerializeField, Range(1, 10)] private int maxRandomTimes;            // 보관할 시간들 개수
    private WaitForSeconds[] times;                                         // 보관된 시간

    private Vector3[] initPos;                                              // 생성 위치

    public bool waveStart = false;                                         // 확인용

    private Transform target;                                               // 공격 대상

    [SerializeField] private int respawnEnemyNum = 2;                       // 리젠 포스에서 생성할 숫자
    [SerializeField] private int addTurn;
    private int curTurn;

    // 생성 번호
    [SerializeField] private int[] respawnEnemySelectIdxs;               // 생성할 적 idx
    private int[] respawnEnemyPoolIdxs;                                   

    private int forcedAtkNum = 30;                                       // 강제 공격할 숫자

    [SerializeField] private ScriptGroup attackScript;

    private void Awake()
    {

        // 랜덤 생각 시간 세팅(캐싱)
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
    /// 타겟이 죽었거나 없는 거 확인
    /// </summary>
    /// <returns></returns>
    private bool IsTargetNull()
    {

        if (target == null
            || target.gameObject.layer == VarianceManager.LAYER_DEAD) return true;

        return false;
    }

    /// <summary>
    /// 새로운 타겟 설정 건물 > 유닛 순서다
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
    /// command 를 이용해 플레이어로 공격!
    /// </summary>
    public void GoToPlayer()
    {

        int unitNum = enemyUnits.Count;
        if (enemyUnits.Count > ushort.MaxValue) unitNum = enemyUnits.Count;
        GiveCommand((ushort)unitNum, STATE_SELECTABLE.UNIT_ATTACK, target.position, true);
    }
    
    /// <summary>
    /// 적의 명령 주기!
    /// </summary>
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
    /// 악마성의 행동
    /// </summary>
    public void BuildingAction()
    {

        STATE_SELECTABLE type = (STATE_SELECTABLE)Random.Range((int)STATE_SELECTABLE.BUILDING_ACTION1, (int)STATE_SELECTABLE.BUILDING_ACTION3 + 1);

        ushort num = (ushort)enemyBuildings.Count;
        GiveCommand(num, type, Vector3.positiveInfinity, false);
    }
    
    /// <summary>
    /// 악마성이 터질 때 까지 반복
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


        // 처음 대기 시간
        yield return new WaitForSeconds(waveStartTime);

        // Script 시작 알리기!
        waveStart = true;
        UIManager.instance.SetScripts(attackScript.Scripts);

        while (!GameManager.instance.IsGameOver)
        {

            if (forcedAtkNum >= VarianceManager.MAX_ENEMY_UNITS) forcedAtkNum = (ushort)VarianceManager.MAX_ENEMY_UNITS;

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


    /// <summary>
    /// 적 생성
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
    /// 주변에 랜덤 좌표 찾기
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
