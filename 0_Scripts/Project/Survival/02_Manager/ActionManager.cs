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
    private List<Unit> allyUnits;


    private List<Building> playerBuildings;
    private List<Building> enemyBuildings;
    private List<Building> neutralBuilding;
    private List<Building> allyBuilding;

    private List<Missile> missiles;

    public List<Follower> followMouse;

    // public delegate void DeadBuilding(Unit _unit, Building _building);
    public Mission.ChkMissionDelegate DeadUnit;
    public Mission.ChkMissionDelegate DeadBuilding;

    public List<Unit> PlayerUnits => playerUnits;
    public List<Unit> EnemyUnits => enemyUnits;
    public List<Unit> NeutralUnits => neutralUnits;
    public List<Unit> AllyUnits => allyUnits;
    

    public List<Building> PlayerBuildings => playerBuildings;
    public List<Building> EnemyBuildings => enemyBuildings;
    public List<Building> NeutralBuilding => neutralBuilding;
    public List<Building> AllyBuildings => allyBuilding;

   
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
        neutralBuilding = new List<Building>(VarianceManager.INIT_NEUTRAL_LIST_NUM);

        allyUnits = new List<Unit>(VarianceManager.INIT_ALLY_LIST_NUM);
        allyBuilding = new List<Building>(VarianceManager.INIT_ALLY_LIST_NUM);

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

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {

            Debug.Log(1 << 0);
            Debug.Log(0b_0000_0001);
        }
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

        for (int i = 0; i < allyBuilding.Count; i++)
        {

            allyBuilding[i].Action();
        }

        for (int i = 0; i < allyUnits.Count; i++)
        {

            allyUnits[i].Action();
        }

        // neutral Building은 활동 X!
        for (int i = 0; i < neutralUnits.Count; i++)
        {

            neutralUnits[i].Action();
        }
    }

    /// <summary>
    /// 리스트에 해당 유닛 등록
    /// </summary>
    /// <param name="_unit"></param>
    public void AddUnit(Unit _unit)
    {

        int layer = _unit.MyTeam.TeamLayerNumber;

        if (layer == VarianceManager.LAYER_PLAYER) playerUnits.Add(_unit);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyUnits.Add(_unit);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralUnits.Add(_unit);
        else if (layer == VarianceManager.LAYER_ALLY) allyUnits.Add(_unit);
    }

    /// <summary>
    /// 리스트에 해당 유닛 제거
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {

        int layer = _unit.MyTeam.TeamLayerNumber;
        if (layer == VarianceManager.LAYER_PLAYER) playerUnits.Remove(_unit);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyUnits.Remove(_unit);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralUnits.Remove(_unit);
        else if (layer == VarianceManager.LAYER_ALLY) allyUnits.Remove(_unit);

        // 미션 확인용
        if (DeadUnit != null) DeadUnit(_unit);
    }

    public bool ContainsUnit(Unit _unit)
    {

        int layer = _unit.MyTeam.TeamLayerNumber;

        if (layer == VarianceManager.LAYER_PLAYER) return playerUnits.Contains(_unit);
        else if (layer == VarianceManager.LAYER_ENEMY) return enemyUnits.Contains(_unit);
        else if (layer == VarianceManager.LAYER_NEUTRAL) return neutralUnits.Contains(_unit);
        else if (layer == VarianceManager.LAYER_ALLY) return allyUnits.Contains(_unit);
        else return false;
    }

    public bool ContainsBuilding(Building _building)
    {

        int layer = _building.MyTeam.TeamLayerNumber;

        if (layer == VarianceManager.LAYER_PLAYER) return playerBuildings.Contains(_building);
        else if (layer == VarianceManager.LAYER_ENEMY) return enemyBuildings.Contains(_building);
        else if (layer == VarianceManager.LAYER_NEUTRAL) return neutralBuilding.Contains(_building);
        else if (layer == VarianceManager.LAYER_ALLY) return allyBuilding.Contains(_building);
        else return false;
    }

    public void AddBuilding(Building _building)
    {

        int layer = _building.MyTeam.TeamLayerNumber;
        if (layer == VarianceManager.LAYER_PLAYER) playerBuildings.Add(_building);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyBuildings.Add(_building);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralBuilding.Add(_building);
        else if (layer == VarianceManager.LAYER_ALLY) allyBuilding.Add(_building);
    }

    public void RemoveBuilding(Building _building)
    {

        int layer = _building.MyTeam.TeamLayerNumber;
        if (layer == VarianceManager.LAYER_PLAYER) playerBuildings.Remove(_building);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyBuildings.Remove(_building);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralBuilding.Remove(_building);
        else if (layer == VarianceManager.LAYER_ALLY) allyBuilding.Remove(_building);

        // 미션 확인용
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