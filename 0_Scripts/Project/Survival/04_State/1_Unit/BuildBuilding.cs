using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǹ� ������ �Ǽ��� ���
/// </summary>
[CreateAssetMenu(fileName = "BuildBuilding", menuName = "Action/Unit/BuildBuilding")]
public class BuildBuilding : IUnitAction
{
    public override void Action(Unit _unit)
    {

        if (_unit.Target == null)
        {
         
            OnExit(_unit, STATE_SELECTABLE.NONE);
            return;
        }

        if (_unit.MyAgent.remainingDistance < _unit.Target.MyStat.MySize * 0.5f)
        {

            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            // ���� �Ǽ� �������� �˻�
            // ���� �˻� > ��ġ �˻� ������ �ϸ� �ȴ�!

            var go = PoolManager.instance.GetSamePrefabs(_unit.Target, _unit.gameObject.layer, _unit.TargetPos);
            if (go)
            {

                go.transform.position = _unit.TargetPos;
                _unit.Target = go.GetComponent<Selectable>();
                _unit.Target.AfterSettingLayer();
                _unit.Target.TargetPos = _unit.Target.transform.position;
                // ���� ��ȣ�� �����ϱ⿡ repair�� Attack�� ���� �ȵȴ�!
                OnExit(_unit, STATE_SELECTABLE.UNIT_REPAIR);
            }
            else
            {

                // �Ǽ� ���� ���� �Ϲ� ���·� Ż��
                OnExit(_unit, STATE_SELECTABLE.NONE);
            }
        }

    }

    public override void OnEnter(Unit _unit)
    {

        // Ÿ���� ���ų� �ǹ��� �ƴ� ��� Ż��!
        if (!_unit.Target) return;

        // Ÿ���� ��ҷ� �̵�!
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 1f);
    }
}
