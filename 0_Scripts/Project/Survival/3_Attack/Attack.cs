using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public abstract class Attack : MonoBehaviour
{

    protected bool isAtk = false;
    protected short coolTime;                   // ������ ���� ��Ÿ��

    public int atk;                             // ���ݷ�

    // ���� ���� �ֱ� 0.02�ʸ� turn�� ���ϸ� �ð��� �ȴ�
    [SerializeField] protected short startAnimTime;                   // �ִϸ��̼� ���� ��
    [SerializeField] protected short atkTime;                         // ������ ���� ���� ��

    public float atkRange;                      // ���� ����
    public float chaseRange;

    public LayerMask atkLayers;                 // ���!

    protected Selectable target;                // ���Ŀ� ����� ����! << player�� target�� selectable ������ �ٲ㼭 �� ���̴�!

    protected virtual void Init(int _atkTime, int _animTime, float _atkRange, float _chaseRange)
    {

        atkTime = (short)_atkTime;
        startAnimTime = (short)_animTime;

        atkRange = _atkRange;
        chaseRange = _chaseRange;
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