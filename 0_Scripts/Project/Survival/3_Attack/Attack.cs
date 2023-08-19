using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{

    protected bool isAtk = false;

    // 물리 연산 주기 0.02초를 turn에 곱하면 시간이 된다
    protected int atkAnimTime = 20;         // 공격 몇턴 전에 애니메이션 시작할지
    protected int coolTime;                 // 데미지 적용 쿨타임
    protected Selectable target;

    public bool IsAtk
    {

        get { return isAtk; }
        set 
        {

            coolTime = 0;
            isAtk = value; 
        }
    }

    public int AtkAnimTime
    {

        get { return atkAnimTime; }
        set { atkAnimTime = value; }
    }

    public Selectable Target
    {

        get { return target; }
        set { target = value; }
    }



    public int CoolTime => coolTime;

    public abstract void OnAttack(Unit _unit);

    public virtual void ChkCoolTime(Unit _unit)
    {

        coolTime++;
        int maxTurn = Mathf.FloorToInt(_unit.AtkTime * 50);

        if (coolTime == Mathf.Max(maxTurn - atkAnimTime, 1))
        {

            _unit.MyAnimator.SetTrigger("Attack");
        }
        else if (coolTime > maxTurn)
        {

            coolTime = 0;
            OnAttack(_unit);
        }
    }
}