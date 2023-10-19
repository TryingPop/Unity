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

            // ���� �Ÿ� �ȿ� ������ �̵� �ʱ�ȭ
            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            // �� Ȯ��
            if (_unit.Target.MyStat.ApplyResources(true, true, false, true))
            {

                var go = PoolManager.instance.GetSamePrefabs(_unit.Target, _unit.gameObject.layer, _unit.TargetPos);

                if (go)
                {

                    go.transform.position = _unit.TargetPos;
                    var _target = go.GetComponent<Selectable>();

                    _unit.Target = _target;
                    _unit.Target.AfterSettingLayer();
                    _unit.Target.TargetPos = _unit.Target.transform.position;
                    OnExit(_unit, STATE_SELECTABLE.UNIT_REPAIR);
                }
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
