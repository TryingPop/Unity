using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyOrderManager : MonoBehaviour
{

    [SerializeField] private List<Unit> playerUnits;
    [SerializeField] private List<Unit> enemyUnits;

    [SerializeField] private List<Building> playerBuildings;
    [SerializeField] private List<Building> enemyBuildings;

    [SerializeField, Range(5, 20)] private float thinkMinTime = 10f;
    [SerializeField, Range(20, 40)] private float thinkMaxTime = 20f;

    [SerializeField, Range(1, 10)] private short maxRandomTimes;
    private WaitForSeconds[] times;

    private bool waveStart = false;

    [SerializeField] private Transform[] initTrans;
    [SerializeField] private ushort enemyCastleIdx;
    private short prefabIdx = -1;

    [SerializeField] private BuildingStateAction[] actions;

    private Transform target;

    public short PrefabIdx
    {

        get
        {

            if (prefabIdx == -1)
            {

                prefabIdx = PoolManager.instance.ChkIdx(enemyCastleIdx);
            }

            return prefabIdx;
        }
    }

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
        GiveCommand((ushort)unitNum, (int)STATE_UNIT.ATTACK, target.position, true);
    }
    
    private void GiveCommand(ushort _num, int _type, Vector3 _dir, bool _isUnit)
    {

        Command cmd;

        if (_dir == Vector3.positiveInfinity || _dir == Vector3.negativeInfinity)
        {

            cmd = Command.GetCommand(_num, _type);
        }
        else
        {

            cmd = Command.GetCommand(_num, _type, _dir);
        }

        if (_isUnit)
        {
            for (int i = 0; i < _num; i++)
            {

                enemyUnits[i].GetCommand(cmd, true);
            }
        }
        else
        {

            for (int i = 0; i < _num; i++)
            {

                enemyBuildings[i].GetCommand(cmd, true);
            }
        }
    }

    public void BuildingAction()
    {

        int type = Random.Range((int)STATE_UNIT.SKILL1, (int)STATE_UNIT.SKILL3 + 1);

        ushort num = (ushort)enemyBuildings.Count;
        GiveCommand(num, type, Vector3.positiveInfinity, false);
    }
    
    private IEnumerator OrderStart()
    {

        while (true)
        {

            if (!waveStart) yield return new WaitForSeconds(1f);
            else yield return times[Random.Range(0, maxRandomTimes)];


            if (!waveStart)
            {

                var castle = BuildEnemyCastle();
                SetTech(castle);
                waveStart = true;
                continue;
            }

            if (enemyUnits.Count > 20)
            {

                // 유닛이 n마리가 넘으면
                // 일정 시간마다 플레이어 강제 공격
                if (IsTargetNull()) SetTarget();
                if (target != null) GoToPlayer();
                continue;
            }

            
            continue;
        }
    }

    /// <summary>
    /// 적 성 건설
    /// </summary>
    private Building BuildEnemyCastle()
    {

        Vector3 randPos = initTrans[Random.Range(0, initTrans.Length)].position;
        var go = PoolManager.instance.GetPrefabs(PrefabIdx, VariableManager.LAYER_ENEMY, randPos, Vector3.positiveInfinity);
        
        if (go)
        {

            var enemyCastle = go.GetComponent<Building>();
            enemyCastle.AfterSettingLayer();
            enemyCastle.TargetPos = enemyCastle.transform.position;
            return enemyCastle;
        }
        else
        {

            Debug.Log("적 성이 생성 안되었습니다. prefabIdx를 확인해주세요.");
            return null;
        }

    }


    private void SetTech(Building _castle)
    {

        if (_castle == null
            || actions == null
            || actions.Length == 0) return;

        var tech = actions[Random.Range(0, actions.Length)];
        _castle.MyStateAction = tech;
    }

    private void EnemyRespawn()
    {


    }
}
