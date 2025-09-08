using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Repair", menuName = "Action/Unit/Repair")]
public class UnitRepair : IUnitAction
{

    [SerializeField] Attack repair;

    public override void Action(Unit _unit)
    {

        // ���� ����� ���ų� ������ ���ϰų� ����� �ı���(�����) ���¸� ����
        if (_unit.Target == null
            || _unit.Target.gameObject.layer == VarianceManager.LAYER_DEAD)
        {

            OnExit(_unit);
            return;
        }

        // �� ã�� ���� ���̸� �ൿ ����
        if (_unit.MyAgent.pathPending) return;

        float sqrRepairDis = repair.atkRange + (_unit.Target.MyStat.MySize * 0.5f);
        sqrRepairDis = sqrRepairDis * sqrRepairDis;

        if (Vector3.SqrMagnitude(_unit.transform.position - _unit.Target.transform.position) < sqrRepairDis)
        {

            // ���� �Ÿ� �ȿ� �ִ� ���
            MY_STATE.GAMEOBJECT targetState = _unit.Target.MyState;

            //�Ǽ� �ؾ��ϴ� ����
            if (targetState == MY_STATE.GAMEOBJECT.BUILDING_UNFINISHED)
            {


#if UNITY_EDITOR

                Debug.Log($"{_unit.MyStat.MyName}�� {_unit.Target.MyStat.MyName} �Ǽ��� �õ� �մϴ�.");
#endif

                // �ǹ� �Ǽ� ���� �ʿ�.
                _unit.Target.Heal(0);
                return;
            }

            if (_unit.MyTurn == 0)
            {

                // �غ� ����
                _unit.MyTurn++;
                _unit.MyAgent.ResetPath();
                if (_unit.MyAgent.updateRotation) _unit.MyAgent.updateRotation = false;
                _unit.transform.LookAt(_unit.Target.transform.position);

            }
            else
            {

                // �Ŀ������� ���� 1�� ī���� ���Ѵ�!
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

                // ���� �ٵ����� ���� Ż��
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
            
            Debug.LogWarning("������ �������� ��Ÿ���� ������ �����ϴ�.");
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
