using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���� ������
/// Small : 1(�⺻ ����), Medium : 2(?), Large : 3(������, �ǹ�), XLarge = 4
/// </summary>
public enum STATE_SIZE { SMALL = 1, MEDIUM = 2, LARGE = 3, XLARGE = 4 }

/// <summary>
/// ���ÿ� �⺻�� �Ǵ� Ŭ����
/// �Ʊ� �ǹ�, ���� �Ӹ� �ƴ϶� �� ���ֵ� ����ϴ� ��ü�� �� �����̶� ���� �� Ŭ������ ��ӹ޴´�
/// </summary>
public abstract class Selectable : MonoBehaviour,   // ���õǾ��ٴ� UI ���� transform �� �̿��� ����
                                    IDamagable      // ��� ������ �ǰ� �����ϴ�!
{

    [Header("���� ���� ����")]
    protected int maxHp;                            // �ִ� ü�� - ��ũ���ͺ� ������Ʈ�� �޾� �� ����������
                                                    // ���׷��̵�� ���������ϰ� ���� ���� �߰��ߴ�
    
    protected int curHp;
    protected int def;

    protected HitBar myHitBar;

    protected AllianceInfo myAlliance;
    protected UpgradeInfo myUpgrades;

    [SerializeField] protected Stats myStat;
    
    public Stats MyStat => myStat;

    public virtual void SetStat()
    {
         
        if (myStat.MaxHp == VariableManager.INFINITE) maxHp = myStat.MaxHp;
        else maxHp = myStat.MaxHp;


        def = myStat.Def;

        if (myUpgrades != null) 
        { 

            maxHp += myUpgrades.AddHp;
            def += myUpgrades.AddDef;
        }
    }

    public virtual void Heal(int _atk)
    {

        _atk = _atk < 0 ? 0 : _atk;
        curHp += _atk;
        if (curHp > maxHp) curHp = maxHp;
        myHitBar.SetHp(curHp);
    }

    public bool FullHp { get { return curHp == maxHp; } }

    protected Queue<Command> cmds;

    [SerializeField] protected Selectable target;
    [SerializeField] protected Vector3 targetPos;

    [SerializeField] protected short myTurn;

    public int MyTurn
    {

        get { return myTurn; }
        set { myTurn = (short)value; }
    }

    public Selectable Target
    {

        get { return target; }
        set { target = value; }
    }


    public Vector3 TargetPos
    {

        get { return targetPos; }
        set { targetPos = value; }
    }

    public HitBar MyHitBar
    {

        get { return myHitBar; }
        set 
        {

            if (value != null)
            {

                value.Init(transform, maxHp, myStat.MySize);
                value.SetHp(curHp);
            }
            myHitBar = value; 
        }
    }

    public abstract int MyState { set; get; }

    /// <summary>
    /// 5������ �̻��� ȥ�� ���ð���!
    /// </summary>
    public bool IsOnlySelected
    {

        get
        {

            return myStat.SelectIdx > 50000;
        }
    }

    public AllianceInfo MyAlliance => myAlliance;
    public UpgradeInfo MyUpgrades => myUpgrades;

    /// <summary>
    /// �ʱ�ȭ �޼���
    /// </summary>
    protected virtual void Init()
    {

        curHp = maxHp;
    }

    public abstract void AfterSettingLayer();

    /// <summary>
    /// �ǰ� �޼���, ��� ���ְ� �ǹ��� �ǰ� �����ϴ�!
    /// </summary>
    public virtual void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (ChkInvincible()) return;

        curHp -= _dmg - def < 0 ? 0 : _dmg - def;

        myHitBar.SetHp(curHp);

        if (curHp <= 0)
        {

            Dead();
        }
    }

    public abstract void GiveButtonInfo(ButtonInfo[] _buttons);
    public abstract void ChkButtons(ButtonInfo[] _buttons);

    /// <summary>
    /// �������� üũ
    /// </summary>
    /// <returns>���� ����</returns>
    protected bool ChkInvincible()
    {

        if (maxHp == VariableManager.INFINITE)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// ��� ó�� �޼���
    /// </summary>
    public virtual void Dead()
    {

        curHp = 0;
        
        // ��ü ���̾�� ����
        gameObject.layer = VariableManager.LAYER_DEAD;
        if (InputManager.instance.curGroup.IsContains(this)) 
        { 
            
            InputManager.instance.curGroup.DeSelect(this);
            InputManager.instance.ChkSelected();
        }
        StartCoroutine(Disabled());
    }

    protected IEnumerator Disabled()
    {

        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }

    #region command
    public abstract void GetCommand(Command _cmd, bool _add = false);

    // public abstract void ReadCommand();
    #endregion command
}
