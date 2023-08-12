using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitMove : BaseUnitState
{

    public BaseUnitMove(BaseUnit _baseUnit) : base(_baseUnit) { }


    // 이동을 실행한다
    public override void Execute()
    {

        if (baseUnit.Target != null)
        {

            // 타겟이 살아있을 경우 타겟만 쫓는다
            if (baseUnit.Target.gameObject.activeSelf) baseUnit.MyAgent.destination = baseUnit.Target.position;
            else
            {

                // 타겟이 죽은 경우
                baseUnit.MyAgent.ResetPath();
            }
        }

        if (baseUnit.MyAgent.remainingDistance < 0.1f)
        {

            baseUnit.DoneState();
        }
    }
}