using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ÿ� �⺻�� �Ǵ� Ŭ����
/// �Ʊ� �ǹ�, ���� �Ӹ� �ƴ϶� �� ���ֵ� ����ϴ� ��ü�� �� �����̶� ���� �� Ŭ������ ��ӹ޴´�
/// </summary>
public abstract class GameEntity : Commandable,         // ��� ���� ������Ʈ -> ���ð� �ൿ����!
                                    IDamagable          // ��� ������ �ǰ� �����ϴ�!
                                          
{

    protected int curHp;                                // ���� Hp
    protected int maxHp;

    protected int atk;
    protected int def;
    protected int evade;

    [Header("����")]
    [SerializeField] protected int myTurn;              // ���� �� �� <<< �ֺ��� �� Ž��, �ǹ� �ൿ���� ���δ�

    [SerializeField] protected bool isStarting = false; // ���� ��ġ�� ���� ���� Ȯ��

    protected HitBar myHitBar;                          // ü�¹�

    [SerializeField] protected Stats myStat;            // ����
    [SerializeField] protected SightMesh myMinimap;
    public Stats MyStat => myStat;                      

    [SerializeField] protected GameEntity target;       // ��ǥ��
    [SerializeField] protected Vector3 targetPos;       // ��ǥ ��ǥ

    [SerializeField] protected SoundGroup mySound;
    [SerializeField] protected AudioSource myAudio;

    /// <summary>
    /// ���� ���� �޾ƿ���
    /// </summary>
    public abstract void GetStat();

    /// <summary>
    /// ü�� ȸ��
    /// </summary>
    public virtual void Heal(int _atk)
    {

        _atk = _atk < 0 ? 0 : _atk;
        curHp += _atk;
        if (curHp > MaxHp) curHp = MaxHp;
        myHitBar.SetHp();

        // ���� ���� ���̸� �����Ѵ�!
        if (PlayerManager.instance.curGroup.Contains(this))
        {

            UIManager.instance.UpdateHp = true;
        }
    }

    /// <summary>
    /// Ǯ Hp ���� Ȯ��
    /// </summary>
    public bool FullHp => curHp == maxHp;

    public virtual int MaxHp => maxHp;
    public int CurHp => curHp;
    public int Def => def;

    /// <summary>
    /// ��� ��ư Ȱ��ȭ�� TYPE������ ������ �� ��� ���ֵ鿡�� Ȱ��ȭ �ؾ��ϴ��� ���´�
    /// </summary>
    public virtual bool IsCancelBtn => false;

    public int MyTurn
    {

        get { return myTurn; }
        set 
        {

#if UNITY_EDITOR

            if (value < 0) Debug.LogError("�� �Է°��� 0�Դϴ�.");
#endif
            myTurn = value; 
        }
    }

    public GameEntity Target
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

                value.Init(this, MaxHp, myStat.HitBarPos);
                value.SetHp();
            }
            myHitBar = value; 
        }
    }

    public void ChkSupply(bool _isDead = false)
    {

        if (myTeam == null) 
        {

#if UNITY_EDITOR

            Debug.LogError($"{this.name}�� �����ε� �� ������ �����ϴ�.");
#endif
            return; 
        }

        int supply = myStat.Supply;
        
        if (supply < 0)
        {

            // �̿ϼ��� ���� Ż��!
            if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED) return;

            if (_isDead)
                // ��� �� �̹Ƿ� �ִ� �α� ���
                myTeam.AddMaxSupply(supply);
            else
                // ��ȯ �� �ִ� �α� ����
                myTeam.AddMaxSupply(-supply);
        }
        else
        {

            if (_isDead) myTeam.AddCurSupply(-supply);
            else myTeam.AddCurSupply(supply);
        }
    }

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
    public virtual void OnDamaged(int _dmg, bool _pure = false, bool _evade = true, Transform _trans = null)
    {

        if (ChkInvincible() || (_evade && ChkEvade())) return;

        if (_pure) curHp -= _dmg < VarianceManager.MIN_DAMAGE ? VarianceManager.MIN_DAMAGE : _dmg;
        else curHp -= _dmg - Def < VarianceManager.MIN_DAMAGE ? VarianceManager.MIN_DAMAGE : _dmg - Def;

        if (curHp < 0) curHp = 0;
        myHitBar.SetHp();
        
        if (curHp == 0) Dead();
        else if (PlayerManager.instance.curGroup.Contains(this)) UIManager.instance.UpdateHp = true;
    }

    /// <summary>
    /// �������� üũ ���� ü�� or ��� ������ ��츸 true!
    /// </summary>
    /// <returns>���� ����</returns>
    protected bool ChkInvincible()
    {

        if (MaxHp == VarianceManager.INFINITE
            || myState == STATE_SELECTABLE.DEAD)
        {

            return true;
        }

        return false;
    }

    protected bool ChkEvade()
    {

        int random = Random.Range(0, 101);
        int evade = myStat.Evade;

        return random < evade;
    }

    /// <summary>
    /// ��� ó�� �޼���, hp�� ǥ��, ���� ���õǸ� ����, �׸��� �α�����
    /// </summary>
    public virtual void Dead(bool _immediately = false)
    {

        myState = STATE_SELECTABLE.DEAD;
        // ��ü ���̾�� ����
        gameObject.layer = VarianceManager.LAYER_DEAD;

        if (_immediately) PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
        else StartCoroutine(Disabled());
    }

    protected void CurGroupDeSelect()
    {

        if (PlayerManager.instance.curGroup.Contains(this))
        {

            PlayerManager.instance.curGroup.DeSelect(this);
            PlayerManager.instance.ChkUIs();
            // slot�� ��ư ����!
            UIManager.instance.ExitInfo(TYPE_INFO.ALL);
        }
    }

    protected void SavedGroupDeSelect()
    {

        if (PlayerManager.instance.curGroup.ContainsSavedGroup(this))
        {

            PlayerManager.instance.curGroup.DeselectSavedGroup(this);
        }
    }

    /// <summary>
    /// ���õǾ� ������ ����
    /// </summary>
    public virtual void ResetTeam()
    {

        CurGroupDeSelect();

        SavedGroupDeSelect();
    }

    /// <summary>
    /// ��� �� ��� ��� �󸶳� ������ ����
    /// </summary>
    protected IEnumerator Disabled()
    {

        if (myStat.DisableTime <= 0) yield return null;
        else if (myStat.DisableTime == 2) yield return VarianceManager.BASE_WAITFORSECONDS;
        else yield return new WaitForSeconds(myStat.DisableTime);

        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    #region Sound
    public void MyStateSong()
    {

        if (mySound == null) return;

        AudioClip sound = mySound.GetSound(myState);
        if (sound == null) return;
        myAudio.clip = sound;
        myAudio.Play();
    }

    #endregion

    public override void SetTitle(Text _titleTxt)
    {

        _titleTxt.text = myStat.name;
    }
}
