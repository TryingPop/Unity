using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuilding : IUnitAction
{

    [SerializeField] protected PrepareBuilding building;

    public override void Action(Unit _unit)
    {

        if (_unit.MyAgent.remainingDistance < building.MySize * 0.5f)
        {

            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            GameObject go = building.Build();
            if (go)
            {

                go.transform.position = _unit.TargetPos;
                Building building = go.GetComponent<Building>();
                ActionManager.instance.AddBuilding(building);
                _unit.Target = building;
            }

            OnExit(_unit, STATE_UNIT.ATTACK);
        }
    }

    public override void OnEnter(Unit _unit)
    {

        // ������ �ϴ� �ൿ�� ���߰��Ѵ�
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);

        // �Ǽ��� �ǹ� ���� ����
        // ��... ? �̰� �ܺο��� Ȯ���� �� �ִ� �����...?
        // InputManager.instance.MyState = VariableManager.BUILD;
        // InputManager.instance.building = building;
        // InputManager.instance.worker = _unit;
        // building.gameObject.SetActive(true);
    }
}
