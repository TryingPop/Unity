using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Attack : ScriptableObject
{

    public int atk;                             // ���ݷ�

    // ���� ���� �ֱ� 0.02�ʸ� turn�� ���ϸ� �ð��� �ȴ�
    [SerializeField] protected short startAnimTime;                   // �ִϸ��̼� ���� ��
    [SerializeField] protected short atkTime;                         // ������ ���� ���� ��

    public float atkRange;                      // ���� ����
    public float chaseRange;

    // public LayerMask atkLayers;                 

    protected static RaycastHit[] hits;

    protected virtual void Init(int _atkTime, int _animTime, float _atkRange, float _chaseRange)
    {

        atkTime = (short)_atkTime;
        startAnimTime = (short)_animTime;

        atkRange = _atkRange;
        chaseRange = _chaseRange;

        if (hits == null)
        {

            hits = new RaycastHit[25];
        }
    }

    public int AtkTime
    {

        get 
        {

            if (atkTime < 1) atkTime = 1;
            return atkTime; 
        }
        set
        {

            value = value <= 1 ? 1 : value;
            atkTime = (short)value;
        }
    }

    public int StartAnimTime
    {

        get 
        {

            if (startAnimTime < 1) startAnimTime = 1;
            return startAnimTime; 
        }
        set
        {

            value = value <= 1 ? 1 : value;
            StartAnimTime = value > atkTime ? atkTime : value;
        }
    }

    public abstract void OnAttack(Unit _unit);

    /// <summary>
    /// ������ Ÿ�� ã��
    /// </summary>
    /// <param name="_isChase">true�� ���� ����, false�� ���� ����</param>
    public virtual void FindTarget(Unit _unit, bool _isChase, bool _isAlly = false)
    {

        // �˻��ϴ� ������ �ڽ� �ݶ��̴��� ���� �־� hits�� �ּ� ũ�� 1�� ����ȴ�
        if (hits == null) hits = new RaycastHit[25];
        
        int cnt = Physics.SphereCastNonAlloc(_unit.transform.position, _isChase ? 
            chaseRange : atkRange, _unit.transform.forward, hits, 0f, _unit.MyTeam.GetLayer(_isAlly));
        float minDis = _isChase ? chaseRange * chaseRange + 1f : atkRange * atkRange + 1f;
        _unit.Target = null;

        Transform target = null;

        for (int i = 0; i < cnt; i++)
        {

            if (hits[i].transform == _unit.transform)
            {

                continue;
            }

            // ���� ����� �� ����!
            float targetDis = Vector3.SqrMagnitude(_unit.transform.position - hits[i].transform.position);
            if (minDis > targetDis)
            {

                minDis = targetDis;
                target = hits[i].transform;
            }
        }

        if (target != null) _unit.Target = target.GetComponent<Selectable>();
    }
}