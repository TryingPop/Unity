using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FixedUpdate, LateUpdate�� �ϰ� �����ϱ� ���� ���� Ŭ����
/// ����, �ǹ�, �̻����� ���⼭ �ϰ� �ൿ�Ѵ�!
/// ������ ���� ��� ��û ����ƴ�
/// </summary>
public class ActionManager : MonoBehaviour
{

    public static ActionManager instance;

    private ActionGroup<Unit> playerUnits;
    private ActionGroup<Unit> enemyUnits;
    private ActionGroup<Unit> neutralUnits;
    private ActionGroup<Unit> allyUnits;


    private ActionGroup<Building> playerBuildings;
    private ActionGroup<Building> enemyBuildings;
    private ActionGroup<Building> neutralBuilding;
    private ActionGroup<Building> allyBuilding;

    private List<Missile> missiles;

    public List<Follower> followMouse;

    // public delegate void DeadBuilding(Unit _unit, Building _building);
    public Mission.ChkMissionDelegate DeadUnit;
    public Mission.ChkMissionDelegate DeadBuilding;

    public ActionGroup<Unit> PlayerUnits => playerUnits;
    public ActionGroup<Unit> EnemyUnits => enemyUnits;
    public ActionGroup<Unit> NeutralUnits => neutralUnits;
    public ActionGroup<Unit> AllyUnits => allyUnits;
    

    public ActionGroup<Building> PlayerBuildings => playerBuildings;
    public ActionGroup<Building> EnemyBuildings => enemyBuildings;
    public ActionGroup<Building> NeutralBuilding => neutralBuilding;
    public ActionGroup<Building> AllyBuildings => allyBuilding;

   
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

        // delegate �ʱ�ȭ
        DeadUnit = null;
        DeadBuilding = null;

        // ����Ʈ�� �ʱ�ȭ
        playerUnits = new(VarianceManager.INIT_UNIT_LIST_NUM);
        playerBuildings = new(VarianceManager.INIT_BUILDING_LIST_NUM);
        
        enemyUnits = new(VarianceManager.INIT_UNIT_LIST_NUM);
        enemyBuildings = new(VarianceManager.INIT_BUILDING_LIST_NUM);

        neutralUnits = new(VarianceManager.INIT_NEUTRAL_LIST_NUM);
        neutralBuilding = new(VarianceManager.INIT_NEUTRAL_LIST_NUM);

        allyUnits = new(VarianceManager.INIT_ALLY_LIST_NUM);
        allyBuilding = new(VarianceManager.INIT_ALLY_LIST_NUM);

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
    /// �÷��̾� �ǹ� > �÷��̾� ���� > ���� �ǹ� > ���� ���� ������ �ൿ�Ѵ�!
    /// </summary>
    private void Action()
    {

        for (int i = 0; i < missiles.Count; i++)
        {

            missiles[i].Action();
        }
        
        playerBuildings.Action();
        playerUnits.Action();

        enemyBuildings.Action();
        enemyUnits.Action();

        allyBuilding.Action();
        allyUnits.Action();

        // neutral Building�� Ȱ�� X!
        neutralUnits.Action();
    }

    /// <summary>
    /// ����Ʈ�� �ش� ���� ���
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
    /// ����Ʈ�� �ش� ���� ����
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {

        int layer = _unit.MyTeam.TeamLayerNumber;
        if (layer == VarianceManager.LAYER_PLAYER) playerUnits.Pop(_unit);
        else if (layer == VarianceManager.LAYER_ENEMY) enemyUnits.Pop(_unit);
        else if (layer == VarianceManager.LAYER_NEUTRAL) neutralUnits.Pop(_unit);
        else if (layer == VarianceManager.LAYER_ALLY) allyUnits.Pop(_unit);

        // �̼� Ȯ�ο�
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

        // �̼� Ȯ�ο�
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