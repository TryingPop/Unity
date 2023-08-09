using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour, IDamagable
{

    public int maxHp;
    public int curHp;

    public enum BuildingType
    {

        None,                   // ��� ���� �ǹ� - ��
        Units,                  // ���� ���� �ǹ�
        Resources,              // �ڿ� ȹ�� �ǹ�
        Upgrades,               // ���׷��̵� �ǹ�
    }

    protected BuildingType type;


    protected virtual void Awake()
    {
        
        type = BuildingType.None;
    }

    protected void OnEnable()
    {

        Init();
    }

    protected virtual void Init()
    {

        curHp = maxHp;
    }


    public void OnDamaged(int _dmg)
    {

        curHp -= _dmg;
        if (curHp < 0)
        {

            Dead();
        }
    }

    protected virtual void Dead()
    {

        curHp = 0;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Ȱ��ȭ
    /// </summary>
    /// <param name="num">��ȣ</param>
    public virtual void Activated(int num) { }
}
