using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class ActionManager : MonoBehaviour
{

    public static ActionManager instance;

    private List<Unit> actionUnits;
    private List<Building> actionBuildings;

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

        actionUnits = new List<Unit>(VariableManager.MAX_CONTROL_UNITS);
        actionBuildings = new List<Building>(VariableManager.MAX_BUILDINGS);

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
    /// 건물, 유닛 순으로 행동 실행
    /// </summary>
    private void Action()
    {

        for (int i = 0; i < actionBuildings.Count; i++)
        {

            // 건물 행동
            actionBuildings[i].Action();
        }

        for (int i = 0; i < actionUnits.Count; i++)
        {

            // 유닛 행동
            actionUnits[i].Action();
        }

    }

    /// <summary>
    /// 리스트에 해당 유닛 등록
    /// </summary>
    /// <param name="_unit"></param>
    public void AddUnit(Unit _unit)
    {

        if (actionUnits.Count < VariableManager.MAX_CONTROL_UNITS) 
        { 

            actionUnits.Add(_unit);
            if (!usedHitBars.TryPop(out HitBar hitbar))
            {

                var go = Instantiate(unitHitBar, this.transform);
                hitbar = go.GetComponent<HitBar>();
            }

            _unit.MyHitBar = hitbar;

            hitBars.Add(hitbar);
        }
    }

    /// <summary>
    /// 리스트에 해당 유닛 제거
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {

        actionUnits.Remove(_unit);

        HitBar hitbar = _unit.MyHitBar;
        hitBars.Remove(hitbar);
        usedHitBars.Push(hitbar);
        hitbar.Used();
        _unit.MyHitBar = null;
    }

    /// <summary>
    /// 이미 포함되었는지 확인 보통 사망용에 쓸 예정
    /// </summary>
    /// <param name="_unit"></param>
    /// <returns></returns>
    public bool ContainsUnit(Unit _unit)
    {

        return actionUnits.Contains(_unit);
    }

    public void AddBuilding(Building _building)
    {

        if (actionBuildings.Count < VariableManager.MAX_BUILDINGS)
        {

            actionBuildings.Add(_building);
            if (!usedHitBars.TryPop(out HitBar hitbar))
            {

                var go = Instantiate(unitHitBar, this.transform);
                hitbar = go.GetComponent<HitBar>();
            }

            _building.MyHitBar = hitbar;

            hitBars.Add(hitbar);
        }
    }

    public void RemoveBuilding(Building _building)
    {

        actionBuildings.Remove(_building);

        HitBar hitbar = _building.MyHitBar;
        hitBars.Remove(hitbar);
        usedHitBars.Push(hitbar);
        hitbar.Used();
        _building.MyHitBar = null;
    }


    private void SetHitBarPos()
    {

        for (int i = 0; i < hitBars.Count; i++)
        {

            hitBars[i].SetPosition();
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
}