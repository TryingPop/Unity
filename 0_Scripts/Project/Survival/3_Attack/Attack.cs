using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{

    protected bool isAtk = false;

    // ���� ���� �ֱ� 0.02�ʸ� turn�� ���ϸ� �ð��� �ȴ�
    protected int atkAnimTime = 20;         // ���� ���� ���� �ִϸ��̼� ��������
    protected int coolTime;                 // ������ ���� ��Ÿ��
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