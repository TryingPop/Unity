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
    /// 공격력 반환
    /// </summary>
    public abstract int GetAtk(int _lvlInfo);

    // 물리 연산 주기 0.02초를 turn에 곱하면 시간이 된다
    [SerializeField] protected int startAnimTime;                   // 애니메이션 시작 턴
    [SerializeField] protected int atkTime;                         // 데미지 연산 시작 턴

    public float atkRange;                                          // 공격 범위
    public float chaseRange;

    [SerializeField] protected int chkTime;                         // 정찰 타임 간격

    /// <summary>
    /// 혹시 몰라 세팅용
    /// </summary>
    protected virtual void Init(int _atkTime, int _animTime, float _atkRange, float _chaseRange)
    {

        atkTime = _atkTime < 1 ? 1 : _atkTime;
        startAnimTime = _animTime < 1 ? 1 : _animTime;

        atkRange = _atkRange;
        chaseRange = _chaseRange;
    }

    /// <summary>
    /// 공격 타이밍 판단 메서드,
    /// 반환 값이 1 : 공격할 타이밍, 0 : 공격할 타이밍인데 값 초기화 X, -1 : 아직 공격할 타이밍 X
    /// </summary>
    public virtual int AtkTime(int _chk)
    {

        if (atkTime <= _chk) return 1;
        else return -1;
    }

    /// <summary>
    /// 애니메이션 시작 타이밍
    /// </summary>
    public bool StartAnimTime(int _chk)
    {

        if (startAnimTime == _chk) return true;
        else return false;
    }

    /// <summary>
    /// 공격! - 데미지 주거나 미사일 생성
    /// </summary>
    public abstract void OnAttack(BaseObj _unit);

    /// <summary>
    /// 범위안 타겟 찾기
    /// </summary>
    /// <param name="_isChase">true면 추적 범위, false면 공격 범위</param>
    public virtual void FindTarget(BaseObj _unit, bool _isChase, bool _isAlly = false)
    {

        // 검사하는 유닛이 박스 콜라이더를 갖고 있어 hits는 최소 크기 1이 보장된다
        _unit.MyTurn++;
        if (_unit.MyTurn < chkTime) return;      // 일정 턴수마다 확인한다!
        _unit.MyTurn = 0;

        int len = Physics.SphereCastNonAlloc(_unit.transform.position, _isChase ? 
            chaseRange : atkRange, _unit.transform.forward, VarianceManager.hits, 0f, _isAlly ? _unit.MyTeam.AllyLayer : _unit.MyTeam.EnemyLayer);
        float minDis = _isChase ? chaseRange * chaseRange + 1f : atkRange * atkRange + 1f;
        _unit.Target = null;

        Transform target = null;

        for (int i = 0; i < len; i++)
        {

            if (VarianceManager.hits[i].transform == _unit.transform) continue;

            // 가장 가까운 적 공격!
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