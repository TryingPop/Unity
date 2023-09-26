using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrderManager : MonoBehaviour
{

    [SerializeField] private List<Unit> playerUnits;
    [SerializeField] private List<Unit> enemyUnits;

    [SerializeField] private List<Building> playerBuildings;
    [SerializeField] private List<Building> enemyBuildings;

    private void Start()
    {

        enemyUnits = ActionManager.instance.EnemyUnits;
        playerUnits = ActionManager.instance.PlayerUnits;
        playerBuildings = ActionManager.instance.PlayerBuildings;
        enemyBuildings = ActionManager.instance.EnemyBuildings;

        StartCoroutine(OrderStart());
    }

    public void GoToPlayer()
    {

        Vector3 dir = Vector3.positiveInfinity;

        if (playerBuildings.Count != 0)
        {

            dir = playerBuildings[Random.Range(0, playerBuildings.Count)].transform.position;
        }
        else if (playerUnits.Count != 0)
        {

            dir = playerUnits[Random.Range(0, playerUnits.Count)].transform.position;
        }

        if (dir != Vector3.positiveInfinity)
        {

            int unitNum = enemyUnits.Count;
            if (enemyUnits.Count > ushort.MaxValue) unitNum = enemyUnits.Count;
            GiveCommand((ushort)unitNum, (int)STATE_UNIT.ATTACK, dir, true);
        }
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

    public void CreateUnit()
    {

        int type = Random.Range((int)STATE_UNIT.SKILL1, (int)STATE_UNIT.SKILL3 + 1);

        ushort num = (ushort)enemyBuildings.Count;
        GiveCommand(num, type, Vector3.positiveInfinity, false);
    }
    
    private IEnumerator OrderStart()
    {

        yield return new WaitForSeconds(60f);
        GoToPlayer();
    }
}
