using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public abstract class Attack : MonoBehaviour
{

    protected bool isAtk = false;
    protected int coolTime;                     // ������ ���� ��Ÿ��

    public int atk;                             // ���ݷ�

    // ���� ���� �ֱ� 0.02�ʸ� turn�� ���ϸ� �ð��� �ȴ�
    public int startAnimTime;                   // �ִϸ��̼� ���� ��
    public int atkTime;                         // ������ ���� ���� ��

    public float atkRange;                      // ���� ����
    public float chaseRange;

    public LayerMask atkLayers;                 // ���!

    protected Selectable target;                // ���Ŀ� ����� ����! << player�� target�� selectable ������ �ٲ㼭 �� ���̴�!

    protected virtual void Init(float _atkTime, float _animTime, float _atkRange, float _chaseRange)
    {

        atkTime = Mathf.FloorToInt(_atkTime * 50);
        StartAnimTime = _animTime;

        atkRange = _atkRange;
        chaseRange = _chaseRange;
    }

    public float AtkTime
    {

        get { return atkTime * 0.02f; }
        set
        {

            atkTime = Mathf.FloorToInt(value * 50);
        }
    }

    [SerializeField]
    public float StartAnimTime
    {

        get { return startAnimTime * 0.02f; }
        set
        {

            int temp = Mathf.FloorToInt(value * 50);
            startAnimTime = temp > atkTime ? atkTime : temp;
            startAnimTime = startAnimTime < 1 ? 1 : startAnimTime;
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



    public int CoolTime => coolTime;

    public abstract void OnAttack(Unit _unit);

    public virtual void ActionAttack(Unit _unit)
    {

        coolTime++;

        if (coolTime == startAnimTime) _unit.MyAnimator.SetTrigger("Attack");
        else if (coolTime > atkTime)
        {

            coolTime = 0;
            OnAttack(_unit);
        }
    }

    /// <summary>
    /// ������ Ÿ�� ã��
    /// </summary>
    /// <param name="isChase">true�� ���� ����, false�� ���� ����</param>
    public virtual void FindTarget(Unit _unit, bool isChase)
    {

        // �˻��ϴ� ������ �ڽ� �ݶ��̴��� ���� �־� hits�� �ּ� ũ�� 1�� ����ȴ�
        RaycastHit[] hits = Physics.SphereCastAll(transform.position,
                   isChase ? chaseRange : atkRange, transform.forward, 0f, atkLayers);


        float minDis = isChase ? chaseRange * chaseRange + 1f : atkRange * atkRange + 1f;
        _unit.Target = null;

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].transform == transform)
            {

                continue;
            }

            // ���� ����� �� ����!
            float targetDis = Vector3.SqrMagnitude(transform.position - hits[i].transform.position);
            if (minDis > targetDis)
            {

                minDis = targetDis;
                _unit.Target = hits[i].transform;
            }
        }
    }
}