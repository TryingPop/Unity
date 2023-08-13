using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnitPatrol : IUnitState<BaseUnit>
{

    private static BaseUnitPatrol instance;
    public static BaseUnitPatrol Instance
    {

        get
        {

            if (instance == null)
            {

                instance = new BaseUnitPatrol();
            }

            return instance;
        }
    }

    /// <summary>
    /// µÎ ÁÂÇ¥¸¦ ¿Ô´Ù°¬´Ù ÇÏ±â!
    /// </summary>
    public void Execute(BaseUnit _baseUnit)
    {

        if (_baseUnit.MyAgent.remainingDistance < 0.1f)
        {

            Vector3 temp = _baseUnit.patrolPos;
            _baseUnit.patrolPos = _baseUnit.MyAgent.destination;
            _baseUnit.MyAgent.destination = temp;
        }
    }

    public void Reset(BaseUnit _baseUnit)
    {

        _baseUnit.MyAgent.destination = _baseUnit.TargetPos;
        _baseUnit.MyAnimator.SetFloat("Move", 0.5f);
        _baseUnit.patrolPos = _baseUnit.transform.position;
    }
}
