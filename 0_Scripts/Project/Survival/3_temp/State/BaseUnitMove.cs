using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitMove : BaseUnitState
{

    public BaseUnitMove(BaseUnit _baseUnit) : base(_baseUnit) { }


    // �̵��� �����Ѵ�
    public override void Execute()
    {

        if (baseUnit.Target != null)
        {

            // Ÿ���� ������� ��� Ÿ�ٸ� �Ѵ´�
            if (baseUnit.Target.gameObject.activeSelf) baseUnit.MyAgent.destination = baseUnit.Target.position;
            else
            {

                // Ÿ���� ���� ���
                baseUnit.MyAgent.ResetPath();
            }
        }

        if (baseUnit.MyAgent.remainingDistance < 0.1f)
        {

            baseUnit.DoneState();
        }
    }
}