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

        // 유닛이 하던 행동을 멈추게한다
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);

        // 건설할 건물 정보 전달
        // 음... ? 이걸 외부에서 확인할 수 있는 방법이...?
        // InputManager.instance.MyState = VariableManager.BUILD;
        // InputManager.instance.building = building;
        // InputManager.instance.worker = _unit;
        // building.gameObject.SetActive(true);
    }
}
