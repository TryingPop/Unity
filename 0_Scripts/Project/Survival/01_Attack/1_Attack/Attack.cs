using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Attack : ScriptableObject
{

    public int atk;                             // ���ݷ�

    // ���� ���� �ֱ� 0.02�ʸ� turn�� ���ϸ� �ð��� �ȴ�
    [SerializeField] protected ushort startAnimTime;                   // �ִϸ��̼� ���� ��
    [SerializeField] protected ushort atkTime;                         // ������ ���� ���� ��

    public float atkRange;                      // ���� ����
    public float chaseRange;

    [SerializeField] protected ushort chkTime;

    protected static RaycastHit[] hits = new RaycastHit[25];            // Ÿ�� ã�⿡�� �������� ����

    /// <summary>
    /// Ȥ�� ���� ���ÿ�
    /// </summary>
    protected virtual void Init(int _atkTime, int _animTime, float _atkRange, float _chaseRange)
    {

        AtkTime = (ushort)_atkTime;
        StartAnimTime = (ushort)_animTime;

        atkRange = _atkRange;
        chaseRange = _chaseRange;
    }

    /// <summary>
    /// ���� Ÿ�̹�
    /// </summary>
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
            atkTime = (ushort)value;
        }
    }

    /// <summary>
    /// �ִϸ��̼� ���� Ÿ�̹�
    /// </summary>
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
            startAnimTime = value > atkTime ? atkTime : (ushort)value;
        }
    }

    /// <summary>
    /// ����! - ������ �ְų� �̻��� ����
    /// </summary>
    public abstract void OnAttack(Unit _unit);

    /// <summary>
    /// ������ Ÿ�� ã��
    /// </summary>
    /// <param name="_isChase">true�� ���� ����, false�� ���� ����</param>
    public virtual void FindTarget(Unit _unit, bool _isChase, bool _isAlly = false)
    {

        // �˻��ϴ� ������ �ڽ� �ݶ��̴��� ���� �־� hits�� �ּ� ũ�� 1�� ����ȴ�
        _unit.MyTurn++;
        if (_unit.MyTurn < chkTime) return;      // ���� �ϼ����� Ȯ���Ѵ�!
        _unit.MyTurn = 0;

        int len = Physics.SphereCastNonAlloc(_unit.transform.position, _isChase ? 
            chaseRange : atkRange, _unit.transform.forward, hits, 0f, _isAlly ? _unit.MyTeam.AllyLayer : _unit.MyTeam.EnemyLayer);
        float minDis = _isChase ? chaseRange * chaseRange + 1f : atkRange * atkRange + 1f;
        _unit.Target = null;

        Transform target = null;

        for (int i = 0; i < len; i++)
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