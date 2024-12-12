using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ÿ� �⺻�� �Ǵ� Ŭ����
/// �Ʊ� �ǹ�, ���� �Ӹ� �ƴ϶� �� ���ֵ� ����ϴ� ��ü�� �� �����̶� ���� �� Ŭ������ ��ӹ޴´�
/// </summary>
public abstract class BaseObj : Commandable,         // ��� ���� ������Ʈ -> ���ð� �ൿ����!
                                    IDamagable          // ��� ������ �ǰ� �����ϴ�!
                                          
{

    protected int curHp;                                // ���� Hp

    [Header("����")]
    [SerializeField] protected int myTurn;              // ���� �� �� <<< �ֺ��� �� Ž��, �ǹ� �ൿ���� ���δ�

    protected HitBar myHitBar;                          // ü�¹�

    [SerializeField] protected Stats myStat;            // ����
    [SerializeField] protected SightMesh myMinimap;
    public Stats MyStat => myStat;                      

    [SerializeField] protected BaseObj target;          // ��ǥ��
    [SerializeField] protected Vector3 targetPos;       // ��ǥ ��ǥ

    [SerializeField] protected SoundGroup mySound;
    [SerializeField] protected AudioSource myAudio;

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
    public abstract bool FullHp { get; }

    public abstract int MaxHp { get; }
    public int CurHp => curHp;
    public abstract int Def { get; }

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

    public BaseObj Target
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

            if (value == null)
            {

#if UNITY_EDITOR

                Debug.LogWarning($"{name} ������Ʈ�� hitbar�� null�Դϴ�.");
#endif
                return;
            }

            value.Init(this, myStat.HitBarPos);
            value.SetMaxHp();
            value.SetHp();
            myHitBar = value;
        }
    }

    protected void ChkSupply(bool _isDead = false)
    {

        if (myTeam == null) 
        {

#if UNITY_EDITOR

            Debug.Log($"{name}�� �����ε� �� ������ �����ϴ�.");
#endif
            return; 
        }

        int supply = myStat.Supply;
        
        if (supply < 0)
        {

            // �̿ϼ��� ���� Ż��!
            if (myState == MY_STATE.GAMEOBJECT.BUILDING_UNFINISHED) return;

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
    public abstract void ChkTeamStat();

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
            || myState == MY_STATE.GAMEOBJECT.DEAD)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// ȸ�� ����
    /// </summary>
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

        myState = MY_STATE.GAMEOBJECT.DEAD;
        // ��ü ���̾�� ����
        gameObject.layer = VarianceManager.LAYER_DEAD;

        if (_immediately) PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
        else StartCoroutine(Disabled());
    }

    /// <summary>
    /// ���� �׷� ����
    /// </summary>
    protected void CurGroupDeSelect()
    {

        if (PlayerManager.instance.curGroup.Contains(this))
        {

            PlayerManager.instance.curGroup.DeSelect(this);
            PlayerManager.instance.ChkUIs();
            // slot�� ��ư ����!
            UIManager.instance.ExitInfo(MY_TYPE.UI.ALL);
        }
    }

    /// <summary>
    /// ����� �׷� ����
    /// </summary>
    protected void SavedGroupDeSelect()
    {

        if (PlayerManager.instance.curGroup.ContainsSavedGroup(this))
            PlayerManager.instance.curGroup.DeselectSavedGroup(this);
    }

    /// <summary>
    /// ���õǾ� ������ ����
    /// </summary>
    public virtual void ResetTeamStat()
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

    // public abstract void SetStat();
}
