using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    public static ActionManager instance;

    private List<Unit> playerUnits;
    private List<Unit> enemyUnits;

    private List<Building> playerBuildings;
    private List<Building> enemyBuildings;

    private Stack<HitBar> usedHitBars;
    private List<HitBar> hitBars;

    [SerializeField] private HitBar unitHitBar;
    [SerializeField] private Canvas hitBarCanvas;

    private List<Missile> missiles;

    public List<Unit> PlayerUnits => playerUnits;
    public List<Unit> EnemyUnits => enemyUnits;

    public List<Building> PlayerBuildings => playerBuildings;
    public List<Building> EnemyBuildings => enemyBuildings;

    public List<FollowMouse> followMouse;

    public bool HitBarCanvas
    {

        get { return hitBarCanvas.enabled; }
        set { hitBarCanvas.enabled = value; }
    }

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

        playerUnits = new List<Unit>(VariableManager.INIT_UNIT_LIST_NUM);
        playerBuildings = new List<Building>(VariableManager.INIT_BUILDING_LIST_NUM);
        
        enemyUnits = new List<Unit>(VariableManager.INIT_UNIT_LIST_NUM);
        enemyBuildings = new List<Building>(VariableManager.INIT_BUILDING_LIST_NUM);

        usedHitBars = new Stack<HitBar>(VariableManager.INIT_UNIT_LIST_NUM + VariableManager.INIT_BUILDING_LIST_NUM);
        hitBars = new List<HitBar>(VariableManager.INIT_UNIT_LIST_NUM + VariableManager.INIT_BUILDING_LIST_NUM);

        missiles = new List<Missile>(VariableManager.INIT_MISSILE_LIST_NUM);

        followMouse = new List<FollowMouse>();
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

        if (_unit.MyAlliance == TeamManager.instance.PlayerTeamInfo
            && playerUnits.Count < VariableManager.MAX_CONTROL_UNITS) playerUnits.Add(_unit);

        else if (_unit.MyAlliance == TeamManager.instance.EnemyTeamInfo) enemyUnits.Add(_unit);
    }

    /// <summary>
    /// 리스트에 해당 유닛 제거
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {
        if (_unit.MyAlliance == TeamManager.instance.PlayerTeamInfo) playerUnits.Remove(_unit);

        else if (_unit.MyAlliance == TeamManager.instance.EnemyTeamInfo) enemyUnits.Remove(_unit);
    }

    public bool ContainsUnit(Unit _unit)
    {

        return playerUnits.Contains(_unit) 
            || enemyUnits.Contains(_unit);
    }

    public bool ContainsBuilding(Building _building)
    {

        return playerBuildings.Contains(_building) 
            || enemyBuildings.Contains(_building);
    }

    public void AddBuilding(Building _building)
    {

        if (_building.MyAlliance == TeamManager.instance.PlayerTeamInfo
            && playerBuildings.Count < VariableManager.MAX_BUILDINGS) playerBuildings.Add(_building);
        else if (_building.MyAlliance == TeamManager.instance.EnemyTeamInfo) enemyBuildings.Add(_building);
    }

    public void RemoveBuilding(Building _building)
    {

        if (_building.MyAlliance == TeamManager.instance.PlayerTeamInfo) playerBuildings.Remove(_building);
        else if (_building.MyAlliance == TeamManager.instance.EnemyTeamInfo) enemyBuildings.Remove(_building);
    }


    public HitBar GetHitBar()
    {

        if (!usedHitBars.TryPop(out HitBar hitbar))
        {

            var go = Instantiate(unitHitBar, this.transform);
            hitbar = go.GetComponent<HitBar>();
        }

        hitBars.Add(hitbar);
        
        return hitbar;
    }

    public void ClearHitBar(HitBar _hitbar)
    {

        hitBars.Remove(_hitbar);
        usedHitBars.Push(_hitbar);
        _hitbar.Used();
    }

    private void SetPos()
    {

        SetHitBarPos();

        for (int i = 0; i < followMouse.Count; i++)
        {

            followMouse[i].SetPos();
        }
    }

    private void SetHitBarPos()
    {

        if (!HitBarCanvas) return;
        for (int i = 0; i < hitBars.Count; i++)
        {

            hitBars[i].SetPos();
        }
    }

    /// <summary>
    /// 리스트에 해당 체력바 등록
    /// </summary>
    public void AddHitBar(HitBar _hitBar)
    {

        if (hitBars.Count < VariableManager.MAX_CONTROL_UNITS) hitBars.Add(_hitBar);
    }

    /// <summary>
    /// 이미 포함되어져 있는지 확인
    /// </summary>
    public bool ContainsHitBar(HitBar _hitBar)
    {

        return hitBars.Contains(_hitBar);
    }

    public void UpgradeChk(AllianceInfo _compareInfo)
    {

        if (playerUnits.Count > 0 && playerUnits[0].MyAlliance == _compareInfo)
        {

            for (int i = 0; i < playerUnits.Count; i++)
            {

                playerUnits[i].SetStat();
            }
        }
        else if (enemyUnits.Count > 0 && enemyUnits[0].MyAlliance == _compareInfo)
        {

            for (int i = 0; i < enemyUnits.Count; i++)
            {

                enemyUnits[i].SetStat();
            }
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

    public void AddFollowMouse(FollowMouse _followMouse)
    {

        if (!followMouse.Contains(_followMouse)) followMouse.Add(_followMouse);
    }

    public void RemoveFollowMouse(FollowMouse _followMouse)
    {

        followMouse.Remove(_followMouse);
    }
}