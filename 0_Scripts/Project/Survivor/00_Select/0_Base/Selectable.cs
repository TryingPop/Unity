using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
/// ���ÿ� �⺻�� �Ǵ� Ŭ����
/// �Ʊ� �ǹ�, ���� �Ӹ� �ƴ϶� �� ���ֵ� ����ϴ� ��ü�� �� �����̶� ���� �� Ŭ������ ��ӹ޴´�
/// </summary>
// [RequireComponent(typeof(Stats)),
//     RequireComponent(typeof(SightMesh))]
public abstract class Selectable : MonoBehaviour,       // ���õǾ��ٴ� UI ���� transform �� �̿��� ����
                                    IDamagable          // ��� ������ �ǰ� �����ϴ�!
{

    [Header("���� ���� ����")]
    protected int curHp;                                // ���� Hp

    protected HitBar myHitBar;                          // ü�¹�

    protected TeamInfo myTeam;                          // �� ����

    [SerializeField] protected Stats myStat;            // ����
    [SerializeField] protected SightMesh myMinimap;
    public Stats MyStat => myStat;                      

    [SerializeField] protected Selectable target;       // ��ǥ��
    [SerializeField] protected Vector3 targetPos;       // ��ǥ ��ǥ

    [SerializeField] protected ushort myTurn;           // ���� �� �� <<< �ֺ��� �� Ž��, �ǹ� �ൿ���� ���δ�

    [SerializeField] protected bool isStarting = false; // ���� ��ġ�� ���� ���� Ȯ��

    /// <summary>
    /// ���� ����
    /// </summary>
    // public abstract void SetStat();

    /// <summary>
    /// ü�� ȸ��
    /// </summary>
    public virtual void Heal(int _atk)
    {

        _atk = _atk < 0 ? 0 : _atk;
        curHp += _atk;
        if (curHp > MaxHp) curHp = MaxHp;
        myHitBar.SetHp(curHp);
    }

    /// <summary>
    /// Ǯ Hp ���� Ȯ��
    /// </summary>
    public bool FullHp { get { return curHp == MaxHp; } }

    public int MaxHp { get { return myStat.MaxHp + myTeam.AddedHp; } }
    public int Def { get { return myStat.Def + myTeam.AddedDef; } }

    /// <summary>
    /// ��� ��ư Ȱ��ȭ�� TYPE������ ������ �� ��� ���ֵ鿡�� Ȱ��ȭ �ؾ��ϴ��� ���´�
    /// </summary>
    public virtual bool IsCancelBtn => false;

    public virtual int MyTurn
    {

        get { return myTurn; }
        set 
        {

            if (value > ushort.MaxValue) value = ushort.MaxValue;
            else if (value < ushort.MinValue) value = ushort.MinValue;

            myTurn = (ushort)value; 
        }
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

                value.Init(transform, MaxHp, myStat.MySize);
                value.SetHp(curHp);
            }
            myHitBar = value; 
        }
    }


    public abstract int MyState { set; get; }

    /// <summary>
    /// 5������ �̻��� ȥ�� ���ð���!
    /// ��Ÿ 1���� �ǹ� ���� �� ���� �ȵǴ� �� ���Դ�
    /// </summary>
    public bool IsOnlySelected
    {

        get
        {

            return myStat.SelectIdx > 50000;
        }
    }

    public TeamInfo MyTeam => myTeam;

    /// <summary>
    /// �ʱ�ȭ �޼���
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// ���� �����ϰ�, ���̾� �ٲ۵� ������ ��ɵ� ��Ƶ� �޼���
    /// </summary>
    public abstract void AfterSettingLayer();

    /// <summary>
    /// �ǰ� �޼���, ��� ���ְ� �ǹ��� �ǰ� �����ϴ�!
    /// </summary>
    public virtual void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (ChkInvincible()) return;

        curHp -= _dmg - Def < VariableManager.MIN_DAMAGE ? VariableManager.MIN_DAMAGE : _dmg - Def;

        if (curHp <= 0) Dead();
        else myHitBar.SetHp(curHp);
    }

    /// <summary>
    /// �������� üũ
    /// </summary>
    /// <returns>���� ����</returns>
    protected bool ChkInvincible()
    {

        if (MaxHp == VariableManager.INFINITE)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// ��� ó�� �޼���, hp�� ǥ��, ���� ���õǸ� ����, �׸��� �α�����
    /// </summary>
    public virtual void Dead()
    {

        curHp = 0;
        myHitBar.SetHp(curHp);

        // ��ü ���̾�� ����
        gameObject.layer = VariableManager.LAYER_DEAD;

        // �α� ��´�
        int supply = myStat.Supply;
        if (supply < 0) myTeam.AddMaxSupply(supply);
        else myTeam.AddCurSupply(-supply);

        // ���� ���� ���̸� �����Ѵ�!
        if (InputManager.instance.curGroup.IsContains(this)) 
        { 
            
            InputManager.instance.curGroup.DeSelect(this);
            InputManager.instance.ChkUIs();
        }

        StartCoroutine(Disabled());
    }

    /// <summary>
    /// ��� �� ��� ��� �󸶳� ������ ����
    /// </summary>
    protected IEnumerator Disabled()
    {

        yield return new WaitForSeconds(myStat.DisableTime);

        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    /// <summary>
    /// ���� ���� UI�� ���� ���� �ѱ� �� ����ϴ� �޼���
    /// </summary>
    public abstract void SetInfo(Text _txt);

    #region command
    /// <summary>
    /// ����� �޴´�
    /// </summary>
    public abstract void GetCommand(Command _cmd, bool _add = false);

    /// <summary>
    /// ����� ������ �� �ִ� �������� Ȥ�� ����� ������ �� �մ��� Ȯ��
    /// </summary>
    protected abstract bool ChkCommand(Command _cmd);

    /// <summary>
    /// ����� ����� ������ Ȯ���ϰ� ����� ��� ����
    /// </summary>
    protected abstract void ReadCommand(Command _cmd);
    #endregion command
}
