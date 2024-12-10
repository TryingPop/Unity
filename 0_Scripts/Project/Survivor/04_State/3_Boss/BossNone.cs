using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossNone", menuName = "Action/Unit/BossNone")]
public class BossNone : UnitAtkNone
{

    [SerializeField] protected MY_STATE.GAMEOBJECT[] types;
    [SerializeField] protected int[] per;

    protected int len = 0;
    protected int weight = -1;
    protected int Weight
    {

        get
        {

            if (weight <= 0)
            {

                len = Mathf.Min(per.Length, types.Length);
                weight = 0;
                for (int i = 0; i < len; i++)
                {

                    weight += per[i];
                }
            }

            return weight;
        }
    }

    public override void Action(Unit _unit)
    {
        
        // 타겟이 없는 경우 타겟을 찾는다!
        if (_unit.Target == null)
        {

            base.Action(_unit);
            return;
        }
        
        // 타겟이 있으므로 타겟의 좌표를 목표 지점으로 한다!
        // 그리고 타겟을 해제!
        _unit.TargetPos = _unit.Target.transform.position;
        _unit.Target = null;
        // 다음 행동 설정 확률적으로 한다!
        int rand = Random.Range(0, Weight);
        for (int i = 0; i < len; i++)
        {

            rand -= per[i];
            if (rand < 0) 
            { 
                
                OnExit(_unit, types[i]);
                return;
            }
        }
    }
}
