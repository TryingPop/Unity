using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FixedUpdate, LateUpdate등 일괄 실행하기 위해 만든 클래스
/// 유닛, 건물, 미사일이 여기서 일괄 행동한다!
/// 실제로 성능 향상에 엄청 도움됐다
/// </summary>
public class ActionManager : MonoBehaviour
{

    public static ActionManager instance;

    private List<Unit> playerUnits;
    private List<Unit> enemyUnits;
    private List<Unit> neutralUnits;

    private List<Building> playerBuildings;
    private List<Building> enemyBuildings;

    private List<Missile> missiles;

    public List<Follower> followMouse;

    // public delegate void DeadBuilding(Unit _unit, Building _building);
    public Mission.ChkMissionDelegate DeadUnit;
    public Mission.ChkMissionDelegate DeadBuilding;

    public List<Unit> PlayerUnits => playerUnits;
    public List<Unit> EnemyUnits => enemyUnits;
    public List<Unit> NeutralUnits => neutralUnits;

    public List<Building> PlayerBuildings => playerBuildings;
    public List<Building> EnemyBuildings => enemyBuildings;

   
    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
        }
        else
        {

            Destroy(gameObject);
        }

        // delegate 초기화
        DeadUnit = null;
        DeadBuilding = null;

        // 리스트들 초기화
        playerUnits = new List<Unit>(VarianceManager.INIT_UNIT_LIST_NUM);
        playerBuildings = new List<Building>(VarianceManager.INIT_BUILDING_LIST_NUM);
        
        enemyUnits = new List<Unit>(VarianceManager.INIT_UNIT_LIST_NUM);
        enemyBuildings = new List<Building>(VarianceManager.INIT_BUILDING_LIST_NUM);

        neutralUnits = new List<Unit>(VarianceManager.INIT_NEUTRAL_LIST_NUM);

        missiles = new List<Missile>(VarianceManager.INIT_MISSILE_LIST_NUM);

        followMouse = new List<Follower>();
    }

    private void FixedUpdate()
    {

        Action();
    }

    private void LateUpdate()
    {

        SetPos();
    }

    /// <summary>
    /// 플레이어 건물 > 플레이어 유닛 > 적군 건물 > 적군 유닛 순으로 행동한다!
    /// </summary>
    private void Action()
    {

        for (int i = 0; i < missiles.Count; i++)
        {

            missiles[i].Action();
        }
        
        for (int i = 0; i < playerBuildings.Count; i++)
        {

            playerBuildings[i].Action();
        }

        for (int i = 0; i < playerUnits.Count; i++)
        {

            playerUnits[i].Action();
        }

        for (int i = 0; i < enemyBuildings.Count; i++)
        {

            enemyBuildings[i].Action();
        }


        for (int i = 0; i < enemyUnits.Count; i++)
        {

            enemyUnits[i].Action();
        }

    }

    /// <summary>
    /// 리스트에 해당 유닛 등록
    /// </summary>
    /// <param name="_unit"></param>
    public void AddUnit(Unit _unit)
    {

        if (!_unit) return;

        if (_unit.MyTeam == TeamManager.instance.PlayerTeamInfo
            && playerUnits.Count < VarianceManager.MAX_CONTROL_UNITS) playerUnits.Add(_unit);

        else if (_unit.MyTeam == TeamManager.instance.EnemyTeamInfo) enemyUnits.Add(_unit);

        else if (_unit.MyTeam == TeamManager.instance.NeutralTeamInfo) neutralUnits.Add(_unit);
    }

    /// <summary>
    /// 리스트에 해당 유닛 제거
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {

        if (_unit.MyTeam == TeamManager.instance.PlayerTeamInfo) playerUnits.Remove(_unit);
        else if (_unit.MyTeam == TeamManager.instance.EnemyTeamInfo) enemyUnits.Remove(_unit);
        else if (_unit.MyTeam == TeamManager.instance.NeutralTeamInfo) neutralUnits.Remove(_unit);

        // GameManager.instance.Chk(_unit, null);
        if (DeadUnit != null) DeadUnit(_unit);
        else Debug.Log("null!!!");
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (DeadUnit == null) Debug.Log("null..");
            else Debug.Log($"{DeadUnit.GetInvocationList().Length}");
        }
    }

    public bool ContainsUnit(Unit _unit)
    {

        return playerUnits.Contains(_unit) 
            || enemyUnits.Contains(_unit)
            || neutralUnits.Contains(_unit);
    }

    public bool ContainsBuilding(Building _building)
    {

        return playerBuildings.Contains(_building) 
            || enemyBuildings.Contains(_building);
    }

    public void AddBuilding(Building _building)
    {

        if (_building.MyTeam == TeamManager.instance.PlayerTeamInfo
            && playerBuildings.Count < VarianceManager.MAX_BUILDINGS) playerBuildings.Add(_building);
        else if (_building.MyTeam == TeamManager.instance.EnemyTeamInfo) enemyBuildings.Add(_building);
    }

    public void RemoveBuilding(Building _building)
    {

        if (_building.MyTeam == TeamManager.instance.PlayerTeamInfo) playerBuildings.Remove(_building);
        else if (_building.MyTeam == TeamManager.instance.EnemyTeamInfo) enemyBuildings.Remove(_building);
        // GameManager.instance.Chk(null, _building);
        if (DeadBuilding != null) DeadBuilding(_building);
    }

    private void SetPos()
    {


        for (int i = 0; i < followMouse.Count; i++)
        {

            followMouse[i].SetPos();
        }
    }

    public void AddMissile(Missile _missile)
    {

        if (!missiles.Contains(_missile))missiles.Add(_missile);
    }

    public void RemoveMissile(Missile _missile)
    {

        missiles.Remove(_missile);
    }

    public void AddFollowMouse(Follower _followMouse)
    {

        if (!followMouse.Contains(_followMouse)) followMouse.Add(_followMouse);
    }

    public void RemoveFollowMouse(Follower _followMouse)
    {

        followMouse.Remove(_followMouse);
    }
}