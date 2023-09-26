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

        playerUnits = new List<Unit>(VariableManager.MAX_CONTROL_UNITS);
        playerBuildings = new List<Building>(VariableManager.MAX_BUILDINGS);
        
        enemyUnits = new List<Unit>(50);
        enemyBuildings = new List<Building>(50);

        usedHitBars = new Stack<HitBar>();
        hitBars = new List<HitBar>(VariableManager.MAX_CONTROL_UNITS + VariableManager.MAX_BUILDINGS);

    }

    private void FixedUpdate()
    {

        Action();
    }

    private void LateUpdate()
    {

        SetHitBarPos();
    }

    /// <summary>
    /// �÷��̾� �ǹ� > �÷��̾� ���� > ���� �ǹ� > ���� ���� ������ �ൿ�Ѵ�!
    /// </summary>
    private void Action()
    {

        
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
    /// ����Ʈ�� �ش� ���� ���
    /// </summary>
    /// <param name="_unit"></param>
    public void AddUnit(Unit _unit)
    {

        if (!_unit) return;

        if (_unit.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_PLAYER)
            && playerUnits.Count < VariableManager.MAX_CONTROL_UNITS) playerUnits.Add(_unit);

        else if (_unit.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_ENEMY)) enemyUnits.Add(_unit);

        EquipHitBar(_unit);
    }

    /// <summary>
    /// ����Ʈ�� �ش� ���� ����
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {
        if (_unit.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_PLAYER)) playerUnits.Remove(_unit);

        else if (_unit.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_ENEMY)) enemyUnits.Remove(_unit);


        ClearHitBar(_unit);
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

        if (_building.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_PLAYER)
            && playerBuildings.Count < VariableManager.MAX_BUILDINGS) playerBuildings.Add(_building);
        else if (_building.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_ENEMY)) enemyBuildings.Add(_building);


        EquipHitBar(_building);
    }

    public void RemoveBuilding(Building _building)
    {

        if (_building.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_PLAYER)) playerBuildings.Remove(_building);
        else if (_building.MyTeam == GameManager.instance.GetTeamInfo(VariableManager.LAYER_ENEMY)) enemyBuildings.Remove(_building);

        ClearHitBar(_building);
    }


    private void EquipHitBar(Selectable _select)
    {

        if (!usedHitBars.TryPop(out HitBar hitbar))
        {

            var go = Instantiate(unitHitBar, this.transform);
            hitbar = go.GetComponent<HitBar>();
        }

        _select.MyHitBar = hitbar;
        hitBars.Add(hitbar);
    }

    private void ClearHitBar(Selectable _select)
    {

        HitBar hitbar = _select.MyHitBar;
        hitBars.Remove(hitbar);
        usedHitBars.Push(hitbar);
        hitbar.Used();
        _select.MyHitBar = null;
    }

    private void SetHitBarPos()
    {

        for (int i = 0; i < hitBars.Count; i++)
        {

            hitBars[i].SetPosition();
        }
    }

    /// <summary>
    /// ����Ʈ�� �ش� ü�¹� ���
    /// </summary>
    public void AddHitBar(HitBar _hitBar)
    {

        if (hitBars.Count < VariableManager.MAX_CONTROL_UNITS) hitBars.Add(_hitBar);
    }

    /// <summary>
    /// �̹� ���ԵǾ��� �ִ��� Ȯ��
    /// </summary>
    public bool ContainsHitBar(HitBar _hitBar)
    {

        return hitBars.Contains(_hitBar);
    }
}