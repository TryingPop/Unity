using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitMove : IUnitState<BaseUnit>
{

    private static BaseUnitMove instance;
    public static BaseUnitMove Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new BaseUnitMove();
            }

            return instance;
        }
    }

    // �̵��� �����Ѵ�
    public void Execute(BaseUnit _baseUnit)
    {

        if (_baseUnit.Target != null)
        {

            // Ÿ���� ������� ��� Ÿ�ٸ� �Ѵ´�
            if (_baseUnit.Target.gameObject.activeSelf) _baseUnit.MyAgent.destination = _baseUnit.Target.position;
            else
            {

                // Ÿ���� ���� ���
                _baseUnit.MyAgent.ResetPath();
            }
        }

        if (_baseUnit.MyAgent.remainingDistance < 0.1f)
        {

            _baseUnit.DoneState();
        }
    }

    public void Reset(BaseUnit _baseUnit)
    {

        _baseUnit.MyAgent.destination = _baseUnit.TargetPos;
        _baseUnit.MyAnimator.SetFloat("Move", 0.5f);
    }
}