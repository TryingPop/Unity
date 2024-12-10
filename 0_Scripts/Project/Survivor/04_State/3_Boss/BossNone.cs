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
        
        // Ÿ���� ���� ��� Ÿ���� ã�´�!
        if (_unit.Target == null)
        {

            base.Action(_unit);
            return;
        }
        
        // Ÿ���� �����Ƿ� Ÿ���� ��ǥ�� ��ǥ �������� �Ѵ�!
        // �׸��� Ÿ���� ����!
        _unit.TargetPos = _unit.Target.transform.position;
        _unit.Target = null;
        // ���� �ൿ ���� Ȯ�������� �Ѵ�!
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
