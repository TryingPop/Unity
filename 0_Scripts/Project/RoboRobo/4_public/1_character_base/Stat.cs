using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected Status status;             // �ɷ�ġ ��ũ���ͺ� ������Ʈ
    [SerializeField] protected WeaponController myWC;     // ������ �ݶ��̴��� ��� �ִ�

    [HideInInspector] public Rigidbody myRd;

    protected WaitForSeconds atkWaitTime;    // ĳ�̿� // ����ȭ �ʿ�!
    protected int nowHp;      // ���� Hp

    protected bool atkBool;    // ���� ����
    protected bool deadBool;   // ��� ����

    /// <summary>
    /// �ڽ� ������Ʈ ����
    /// </summary>
    protected virtual void GetComp()
    {

        if (myRd == null) myRd = GetComponent<Rigidbody>();
        if (myWC == null) myWC = GetComponentInChildren<WeaponController>();

        if (status.AtkInterval <= 0) { atkWaitTime = null; }
        else { atkWaitTime = new WaitForSeconds(status.AtkInterval); }
    }


    /// <summary>
    /// hp �ʱ�ȭ �̿� �ٸ��� �ʱ�ȭ�Ҳ� ������ ���⿡ �ֱ�
    /// </summary>
    public virtual void Init()
    {

        SetHp();
    }

    /// <summary>
    /// ü�°� ������� �ʱ�ȭ
    /// </summary>
    protected void SetHp()
    {

        nowHp = status.Hp;
        deadBool = false;
    }

    /// <summary>
    /// ���� �ٸ� �ʿ��� �̺�Ʈ�� �� �� ����� ��
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
    protected virtual void Attack(object sender, Collider other)
    {

        myWC.AtkColActive(false);
    }

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Attack()
    {

        atkBool = true;
        myWC.AtkColActive(true);

        yield return atkWaitTime;

        atkBool = false;
        myWC.AtkColActive(false);
    }


    /// <summary>
    /// �ǰ�
    /// </summary>
    /// <param name="atk">���ݷ�</param>
    public virtual void OnDamaged(int atk)
    {

        if (!deadBool)
        {

            atk -= status.Def;

            if (atk < 1)
            {

                atk = 1;
            }

            nowHp -= atk;

            if (nowHp < 0)
            {

                nowHp = 0;
            }
        }
    }


    /// <summary>
    /// hp < 0 �����̰� ���� ������ ���� Dead ����
    /// </summary>
    protected virtual void ChkDead()
    {

        if (nowHp <= 0 && !deadBool)
        {

            Dead();
        }
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    protected virtual void Dead()
    {

        deadBool = true;
    }
}