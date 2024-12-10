using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Attack : ScriptableObject
{

    [SerializeField] protected bool isPure;
    [SerializeField] protected bool isEvade;

    public bool IsPure => isPure;
    public bool IsEvade => isEvade;

    /// <summary>
    /// ���ݷ� ��ȯ
    /// </summary>
    public abstract int GetAtk(int _lvlInfo);

    // ���� ���� �ֱ� 0.02�ʸ� turn�� ���ϸ� �ð��� �ȴ�
    [SerializeField] protected int startAnimTime;                   // �ִϸ��̼� ���� ��
    [SerializeField] protected int atkTime;                         // ������ ���� ���� ��

    public float atkRange;                                          // ���� ����
    public float chaseRange;

    [SerializeField] protected int chkTime;                         // ���� Ÿ�� ����

    /// <summary>
    /// Ȥ�� ���� ���ÿ�
    /// </summary>
    protected virtual void Init(int _atkTime, int _animTime, float _atkRange, float _chaseRange)
    {

        atkTime = _atkTime < 1 ? 1 : _atkTime;
        startAnimTime = _animTime < 1 ? 1 : _animTime;

        atkRange = _atkRange;
        chaseRange = _chaseRange;
    }

    /// <summary>
    /// ���� Ÿ�̹� �Ǵ� �޼���,
    /// ��ȯ ���� 1 : ������ Ÿ�̹�, 0 : ������ Ÿ�̹��ε� �� �ʱ�ȭ X, -1 : ���� ������ Ÿ�̹� X
    /// </summary>
    public virtual int AtkTime(int _chk)
    {

        if (atkTime <= _chk) return 1;
        else return -1;
    }

    /// <summary>
    /// �ִϸ��̼� ���� Ÿ�̹�
    /// </summary>
    public bool StartAnimTime(int _chk)
    {

        if (startAnimTime == _chk) return true;
        else return false;
    }

    /// <summary>
    /// ����! - ������ �ְų� �̻��� ����
    /// </summary>
    public abstract void OnAttack(BaseObj _unit);

    /// <summary>
    /// ������ Ÿ�� ã��
    /// </summary>
    /// <param name="_isChase">true�� ���� ����, false�� ���� ����</param>
    public virtual void FindTarget(BaseObj _unit, bool _isChase, bool _isAlly = false)
    {

        // �˻��ϴ� ������ �ڽ� �ݶ��̴��� ���� �־� hits�� �ּ� ũ�� 1�� ����ȴ�
        _unit.MyTurn++;
        if (_unit.MyTurn < chkTime) return;      // ���� �ϼ����� Ȯ���Ѵ�!
        _unit.MyTurn = 0;

        int len = Physics.SphereCastNonAlloc(_unit.transform.position, _isChase ? 
            chaseRange : atkRange, _unit.transform.forward, VarianceManager.hits, 0f, _isAlly ? _unit.MyTeam.AllyLayer : _unit.MyTeam.EnemyLayer);
        float minDis = _isChase ? chaseRange * chaseRange + 1f : atkRange * atkRange + 1f;
        _unit.Target = null;

        Transform target = null;

        for (int i = 0; i < len; i++)
        {

            if (VarianceManager.hits[i].transform == _unit.transform) continue;

            // ���� ����� �� ����!
            float targetDis = Vector3.SqrMagnitude(_unit.transform.position - VarianceManager.hits[i].transform.position);
            if (minDis > targetDis)
            {

                minDis = targetDis;
                target = VarianceManager.hits[i].transform;
            }
        }

        if (target != null) _unit.Target = target.GetComponent<BaseObj>();
    }

    public int CalcUnitAtk(TeamInfo _team)
    {

        int lvl = _team == null ? 0 : _team.GetLvl(MY_TYPE.UPGRADE.UNIT_ATK);
        return GetAtk(lvl);
    }
}