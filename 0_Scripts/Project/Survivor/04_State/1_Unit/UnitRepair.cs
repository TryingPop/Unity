using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair", menuName = "Action/Unit/Repair")]
public class UnitRepair : IUnitAction
{

    [SerializeField] Attack repair;

    public override void Action(Unit _unit)
    {

        // 수리 대상이 없거나 수리를 못하거나 대상이 파괴된(사망인) 상태면 종료
        if (_unit.Target == null
            || _unit.Target.gameObject.layer == VarianceManager.LAYER_DEAD)
        {

            OnExit(_unit);
            return;
        }

        // 길 찾기 연산 중이면 행동 안함
        if (_unit.MyAgent.pathPending) return;

        float sqrRepairDis = repair.atkRange + (_unit.Target.MyStat.MySize * 0.5f);
        sqrRepairDis = sqrRepairDis * sqrRepairDis;

        if (Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position) < sqrRepairDis)
        {

            // 수리 거리 안에 있는 경우
            MY_STATE.GAMEOBJECT targetState = _unit.Target.MyState;

            //건설 해야하는 상태
            if (targetState == MY_STATE.GAMEOBJECT.BUILDING_UNFINISHED)
            {


#if UNITY_EDITOR

                Debug.Log($"{_unit.MyStat.MyName}이 {_unit.Target.MyStat.MyName} 건설을 시도 합니다.");
#endif

                // 건물 건설 상태 필요.
                _unit.Target.Heal(0);
                return;
            }

            if (_unit.MyTurn == 0)
            {

                // 준비 상태
                _unit.MyTurn++;
                _unit.MyAgent.ResetPath();
                if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                _unit.transform.LookAt(_unit.Target.transform.position);

            }
            else
            {

                // 후연산으로 변경 1을 카운팅 못한다!
                int turn = _unit.MyTurn++;
                if (repair.StartAnimTime(turn))
                {

                    _unit.MyAnimator.SetTrigger("Skill0");
                    _unit.MyStateSong();
                }
                else if (repair.AtkTime(turn) == 1)
                {

                    _unit.MyTurn = 0;
                    if (targetState != MY_STATE.GAMEOBJECT.BUILDING_UNFINISHED) repair.OnAttack(_unit);
                }

                // 수리 다됐으면 상태 탈출
                if (_unit.Target.FullHp) OnExit(_unit);
            }

            return;
        }

        if (!_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = true;

        if (_unit.MyTurn < 5) _unit.MyTurn++;
        else _unit.MyAgent.SetDestination(_unit.Target.transform.position);
    }

    public override void OnEnter(Unit _unit)
    {

#if UNITY_EDITOR

        if (repair == null) 
        { 
            
            Debug.LogWarning("수리에 수리량을 나타내는 공격이 없습니다.");
            return; 
        }
#endif

        _unit.MyTurn = 0;
        if (_unit.Target == null) 
        { 
            
            OnExit(_unit);
            return;
        }
        _unit.MyAgent.SetDestination(_unit.Target.transform.position);
        _unit.MyAnimator.SetFloat("Move", 1f);
    }

    protected override void OnExit(Unit _unit, MY_STATE.GAMEOBJECT _nextState = MY_STATE.GAMEOBJECT.NONE)
    {

        base.OnExit(_unit, _nextState);
    }
}
