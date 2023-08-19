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
    /// ���� �ൿ ����
    /// </summary>
    private void Action()
    {

        for (int i = 0; i < actionUnits.Count; i++)
        {

            // ���� �ൿ
            actionUnits[i].Action();
        }
    }

    /// <summary>
    /// ����Ʈ�� �ش� ���� ���
    /// </summary>
    /// <param name="_unit"></param>
    public void AddUnit(Unit _unit)
    {

        if (actionUnits.Count < MAX_CONTROL_UNITS) actionUnits.Add(_unit);
    }

    /// <summary>
    /// ����Ʈ�� �ش� ���� ����
    /// </summary>
    /// <param name="_unit"></param>
    public void RemoveUnit(Unit _unit)
    {

        actionUnits.Remove(_unit);
    }

    /// <summary>
    /// �̹� ���ԵǾ����� Ȯ�� ���� ����뿡 �� ����
    /// </summary>
    /// <param name="_unit"></param>
    /// <returns></returns>
    public bool ContainsUnit(Unit _unit)
    {

        return actionUnits.Contains(_unit);
    }
}