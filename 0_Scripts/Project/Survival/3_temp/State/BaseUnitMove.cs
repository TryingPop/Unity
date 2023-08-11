using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitMove : BaseUnitNone
{

    public BaseUnitMove(NavMeshAgent _nav) : base(_nav) { }


    // �̵��� �����Ѵ�
    public override void Execute(Vector3 _vec, Transform _target)
    {

        if (_target != null)
        {

            // Ÿ���� ������� ��� Ÿ�ٸ� �Ѵ´�
            if (_target.gameObject.activeSelf) nav.destination = _target.position;
            else
            {

                // Ÿ���� ���� ���
                nav.destination = nav.transform.position;
            }
        }

        if (nav.remainingDistance < 0.1f)
        {

            isDone = true;
        }
        else
        {

            isDone = false;
        }
    }
}