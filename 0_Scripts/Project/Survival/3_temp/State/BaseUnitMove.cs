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

    // 이동을 실행한다
    public void Execute(BaseUnit _baseUnit)
    {

        if (_baseUnit.Target != null)
        {

            // 타겟이 살아있을 경우 타겟만 쫓는다
            if (_baseUnit.Target.gameObject.activeSelf) _baseUnit.MyAgent.destination = _baseUnit.Target.position;
            else
            {

                // 타겟이 죽은 경우
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