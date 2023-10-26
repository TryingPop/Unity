using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Attack : ScriptableObject
{

    public int atk;                             // 공격력

    // 물리 연산 주기 0.02초를 turn에 곱하면 시간이 된다
    [SerializeField] protected ushort startAnimTime;                   // 애니메이션 시작 턴
    [SerializeField] protected ushort atkTime;                         // 데미지 연산 시작 턴

    public float atkRange;                      // 공격 범위
    public float chaseRange;

    [SerializeField] protected ushort chkTime;

    protected static RaycastHit[] hits = new RaycastHit[25];            // 타겟 찾기에서 공통으로 쓴다

    /// <summary>
    /// 혹시 몰라 세팅용
    /// </summary>
    protected virtual void Init(int _atkTime, int _animTime, float _atkRange, float _chaseRange)
    {

        AtkTime = (ushort)_atkTime;
        StartAnimTime = (ushort)_animTime;

        atkRange = _atkRange;
        chaseRange = _chaseRange;
    }

    /// <summary>
    /// 공격 타이밍
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
    /// 애니메이션 시작 타이밍
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
    /// 공격! - 데미지 주거나 미사일 생성
    /// </summary>
    public abstract void OnAttack(Unit _unit);

    /// <summary>
    /// 범위안 타겟 찾기
    /// </summary>
    /// <param name="_isChase">true면 추적 범위, false면 공격 범위</param>
    public virtual void FindTarget(Unit _unit, bool _isChase, bool _isAlly = false)
    {

        // 검사하는 유닛이 박스 콜라이더를 갖고 있어 hits는 최소 크기 1이 보장된다
        _unit.MyTurn++;
        if (_unit.MyTurn < chkTime) return;      // 일정 턴수마다 확인한다!
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

            // 가장 가까운 적 공격!
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