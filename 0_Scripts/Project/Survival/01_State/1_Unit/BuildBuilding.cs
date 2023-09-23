using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBuilding : IUnitAction
{

    private static BuildBuilding instance;

    public static BuildBuilding Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new BuildBuilding();
            }

            return instance;
        }
    }

    public override void Action(Unit _unit)
    {

        if (_unit.Target == null)
        {
         
            OnExit(_unit, STATE_UNIT.NONE);
            return;
        }

        if (_unit.MyAgent.remainingDistance < _unit.Target.MySize * 0.5f)
        {

            _unit.MyAgent.ResetPath();
            _unit.MyAnimator.SetFloat("Move", 0f);

            // ���� ���� �˻�
            // _unit.Target.gameObject.SetActive(true);
            var go = PoolManager.instance.GetSamePrefabs(_unit.Target, _unit.gameObject.layer);
            if (go)
            {

                go.transform.position = _unit.TargetPos;
                _unit.Target = go.GetComponent<Selectable>();
                // �Ǽ� ������ ��쿡�� �ǹ� ������ ����
                OnExit(_unit, STATE_UNIT.ATTACK);
            }
            else
            {

                // �Ǽ� ���� ���� �Ϲ� ���·� Ż��
                OnExit(_unit, STATE_UNIT.NONE);
            }

        }
    }

    public override void OnEnter(Unit _unit)
    {

        if (_unit.Target == null) return;

        // Ÿ���� ��ҷ� �̵�!
        _unit.MyAgent.destination = _unit.TargetPos;
        _unit.MyAnimator.SetFloat("Move", 0.5f);
    }
}
