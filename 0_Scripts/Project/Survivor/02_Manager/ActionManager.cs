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

    private CommandGroup playerUnits;
    private CommandGroup enemyUnits;
    private CommandGroup neutralUnits;
    private CommandGroup allyUnits;


    private CommandGroup playerBuildings;
    private CommandGroup enemyBuildings;
    private CommandGroup neutralBuilding;
    private CommandGroup allyBuilding;

    private ActionGroup<Missile> missiles;

    public List<Follower> followMouse;

    // public delegate void DeadBuilding(Unit _unit, Building _building);
    public Mission.ChkMissionDelegate DeadUnit;
    public Mission.ChkMissionDelegate DeadBuilding;

    public CommandGroup PlayerUnits => playerUnits;
    public CommandGroup EnemyUnits => enemyUnits;
    public CommandGroup NeutralUnits => neutralUnits;
    public CommandGroup AllyUnits => allyUnits;

    public CommandGroup PlayerBuildings => playerBuildings;
    public CommandGroup EnemyBuildings => enemyBuildings;
    public CommandGroup NeutralBuilding => neutralBuilding;
    public CommandGroup AllyBuildings => allyBuilding;

   
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
        playerUnits = new(VarianceManager.INIT_UNIT_LIST_NUM);
        playerBuildings = new(VarianceManager.INIT_BUILDING_LIST_NUM);
        
        enemyUnits = new(VarianceManager.INIT_UNIT_LIST_NUM);
        enemyBuildings = new(VarianceManager.INIT_BUILDING_LIST_NUM);

        neutralUnits = new(VarianceManager.INIT_NEUTRAL_LIST_NUM);
        neutralBuilding = new(VarianceManager.INIT_NEUTRAL_LIST_NUM);

        allyUnits = new(VarianceManager.INIT_ALLY_LIST_NUM);
        allyBuilding = new(VarianceManager.INIT_ALLY_LIST_NUM);

        missiles = new ActionGroup<Missile>(VarianceManager.INIT_MISSILE_LIST_NUM);

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

        missiles.Action();

        playerBuildings.Action();
        playerUnits.Action();

        enemyBuildings.Action();
        enemyUnits.Action();

        allyBuilding.Action();
        allyUnits.Action();

        // neutral Building은 활동 X!
        neutralUnits.Action();
    }

    /// <summary>
    /// 리스트에 해당 유닛 등록
    /// </summary>
    /// <param name="_unit"></param>
    public void AddUnit(Unit _unit)
    {

        int layer = _unit.MyTeam.TeamLayerNumber;

        if (layer == VarianceManager.LAYER_PLAYER) playerUnits.AddLast(_unit);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyUnits.AddLast(_unit);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralUnits.AddLast(_unit);
        else if (layer == VarianceManager.LAYER_ALLY) allyUnits.AddLast(_unit);
    }

    /// <summary>
    /// 리스트에 해당 유닛 제거
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {

        int layer = _unit.MyTeam.TeamLayerNumber;
        if (layer == VarianceManager.LAYER_PLAYER) playerUnits.Pop(_unit);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyUnits.Pop(_unit);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralUnits.Pop(_unit);
        else if (layer == VarianceManager.LAYER_ALLY) allyUnits.Pop(_unit);

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
        if (layer == VarianceManager.LAYER_PLAYER) playerBuildings.AddLast(_building);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyBuildings.AddLast(_building);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralBuilding.AddLast(_building);
        else if (layer == VarianceManager.LAYER_ALLY) allyBuilding.AddLast(_building);
    }

    public void RemoveBuilding(Building _building)
    {

        int layer = _building.MyTeam.TeamLayerNumber;
        if (layer == VarianceManager.LAYER_PLAYER) playerBuildings.Pop(_building);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyBuildings.Pop(_building);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralBuilding.Pop(_building);
        else if (layer == VarianceManager.LAYER_ALLY) allyBuilding.Pop(_building);

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

        if (!missiles.Contains(_missile))missiles.AddLast(_missile);
    }

    public void RemoveMissile(Missile _missile)
    {

        missiles.Pop(_missile);
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