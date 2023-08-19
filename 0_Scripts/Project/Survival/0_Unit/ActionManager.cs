using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{

    public static ActionManager instance;

    private List<Unit> actionUnits;

    public readonly int MAX_CONTROL_UNITS = 200;

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

        actionUnits = new List<Unit>(MAX_CONTROL_UNITS);
    }

    private void FixedUpdate()
    {

        Action();
    }

    /// <summary>
    /// 유닛 행동 실행
    /// </summary>
    private void Action()
    {

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

        if (actionUnits.Count < MAX_CONTROL_UNITS) actionUnits.Add(_unit);
    }

    /// <summary>
    /// 리스트에 해당 유닛 제거
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {

        actionUnits.Remove(_unit);
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
}