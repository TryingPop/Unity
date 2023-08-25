using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public abstract class Attack : MonoBehaviour
{

    protected bool isAtk = false;
    protected short coolTime;                   // 데미지 적용 쿨타임

    public int atk;                             // 공격력

    // 물리 연산 주기 0.02초를 turn에 곱하면 시간이 된다
    [SerializeField] protected short startAnimTime;                   // 애니메이션 시작 턴
    [SerializeField] protected short atkTime;                         // 데미지 연산 시작 턴

    public float atkRange;                      // 공격 범위
    public float chaseRange;

    public LayerMask atkLayers;                 // 대상!

    protected Selectable target;                // 추후에 사라질 변수! << player에 target을 selectable 형으로 바꿔서 쓸 것이다!

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

            if (atkTime <= 0) atkTime = 1;
            return atkTime; 
        }
        set
        {

            value = value < 1 ? 1 : value;
            atkTime = (short)value;
            
        }
    }

    public int StartAnimTime
    {

        get { return startAnimTime; }
        set
        {

            value = value < 1 ? 1 : value;
            StartAnimTime = value > atkTime ? atkTime : value;
        }
    }



    public bool IsAtk
    {

        get { return isAtk; }
        set 
        {

            coolTime = 0;
            isAtk = value; 
        }
    }

    public Selectable Target
    {

        get { return target; }
        set { target = value; }
    }



    public int CoolTime
    {

        get { return coolTime; }
        set { coolTime = (short)value; }
    }


    public abstract void OnAttack(Unit _unit);


    /// <summary>
    /// 범위안 타겟 찾기
    /// </summary>
    /// <param name="isChase">true면 추적 범위, false면 공격 범위</param>
    public virtual void FindTarget(Unit _unit, bool isChase)
    {
        // 검사하는 유닛이 박스 콜라이더를 갖고 있어 hits는 최소 크기 1이 보장된다
        // RaycastHit[] hits = Physics.SphereCastAll(transform.position,
        //          isChase ? chaseRange : atkRange, transform.forward, 0f, atkLayers);

        if (hits == null) hits = new RaycastHit[25];
        
        int cnt = Physics.SphereCastNonAlloc(transform.position, isChase ? chaseRange : atkRange, transform.forward, hits, 0f, atkLayers);
        float minDis = isChase ? chaseRange * chaseRange + 1f : atkRange * atkRange + 1f;
        _unit.Target = null;

        for (int i = 0; i < cnt; i++)
        // for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform == transform)
            {

                continue;
            }

            // 가장 가까운 적 공격!
            float targetDis = Vector3.SqrMagnitude(transform.position - hits[i].transform.position);
            if (minDis > targetDis)
            {

                minDis = targetDis;
                _unit.Target = hits[i].transform;
            }
        }
        
    }
}