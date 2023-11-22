using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Action/Unit/Move")]
public class UnitMove : IUnitAction
{

    public override void Action(Unit _unit)
    {

        // �� ���� �߿��� �н� �ش� ���� ������ ����� �̵��� ���Ѵ�!
        if (_unit.MyAgent.pathPending) return;

        if (_unit.Target != null)
        {

            // ����� ����ִ��� Ȯ��
            if (_unit.Target.gameObject.activeSelf
                && _unit.Target.gameObject.layer != VarianceManager.LAYER_DEAD)
            {

                if (_unit.MyTurn < 5)
                {

                    _unit.MyTurn++;
                }
                else
                {
                    // ������� �̵�
                    _unit.TargetPos = _unit.Target.transform.position;
                    // �̰� �����ؾ��Ѵ�
                    _unit.MyAgent.SetDestination(_unit.TargetPos);
                    _unit.MyTurn = 0;
                }
            }
            else _unit.Target = null;
        }
        else
        {

            if (_unit.MyAgent.remainingDistance < 0.1f)
            {

                OnExit(_unit);
            }
        }
    }

    public override void OnEnter(Unit _unit)
    {

        _unit.MyAgent.SetDestination(_unit.TargetPos);
        _unit.MyAnimator.SetFloat("Move", 1f);
        _unit.MyTurn = 0;
    }
}
