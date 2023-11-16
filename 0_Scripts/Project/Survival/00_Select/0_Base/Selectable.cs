using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 선택에 기본이 되는 클래스
/// 아군 건물, 유닛 뿐만 아니라 적 유닛도 명령하는 객체를 둘 예정이라 적도 이 클래스를 상속받는다
/// </summary>
// [RequireComponent(typeof(Stats)),
//     RequireComponent(typeof(SightMesh))]
public abstract class Selectable : MonoBehaviour,       // 선택되었다는 UI 에서 transform 을 이용할 예정
                                    IDamagable,         // 모든 유닛은 피격 가능하다!
                                    IInfoTxt            // 인포 메시지 
{

    [Header("생존 관련 변수")]
    protected int curHp;                                // 현재 Hp

    protected HitBar myHitBar;                          // 체력바

    protected TeamInfo myTeam;                          // 팀 정보

    [SerializeField] protected Stats myStat;            // 스텟
    [SerializeField] protected SightMesh myMinimap;
    public Stats MyStat => myStat;                      

    [SerializeField] protected Selectable target;       // 목표물
    [SerializeField] protected Vector3 targetPos;       // 목표 좌표

    [SerializeField] protected ushort myTurn;           // 진행 턴 수 <<< 주변에 적 탐색, 건물 행동에서 쓰인다

    [SerializeField] protected bool isStarting = false; // 씬에 배치된 몬스터 인지 확인

    [SerializeField] protected STATE_SELECTABLE myState;

    protected string stateName = "";

    public string StateName { set { stateName = value; } }
    /// <summary>
    /// 스텟 설정
    /// </summary>
    // public abstract void SetStat();

    /// <summary>
    /// 체력 회복
    /// </summary>
    public virtual void Heal(int _atk)
    {

        _atk = _atk < 0 ? 0 : _atk;
        curHp += _atk;
        if (curHp > MaxHp) curHp = MaxHp;
        myHitBar.SetHp(curHp);

        // 현재 선택 중이면 해제한다!
        if (InputManager.instance.curGroup.Contains(this))
        {

            UIManager.instance.UpdateHp = true;
        }
    }

    /// <summary>
    /// 풀 Hp 인지 확인
    /// </summary>
    public bool FullHp { get { return curHp == MaxHp; } }

    public int MaxHp { get { return myStat.MaxHp + myTeam.AddedHp; } }
    public int CurHp => curHp;
    public int Def { get { return myStat.Def + myTeam.AddedDef; } }

    /// <summary>
    /// 취소 버튼 활성화는 TYPE만으로 결정할 수 없어서 유닛들에게 활성화 해야하는지 묻는다
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

                value.Init(transform, MaxHp, myStat.HitBarPos);
                value.SetHp(curHp);
            }
            myHitBar = value; 
        }
    }

    public virtual int MyState
    {

        get { return (int)myState; }
        set { myState = (STATE_SELECTABLE)value; }
    }

    public TeamInfo MyTeam => myTeam;

    public void ChkSupply(bool _isDead = false)
    {

        if (myTeam == null) return;

        int supply = myStat.Supply;
        
        if (supply < 0)
        {

            // 미완성인 경우는 탈출!
            if (myState == STATE_SELECTABLE.BUILDING_UNFINISHED) return;

            if (_isDead)
            {

                // 사망 시 이므로 최대 인구 깎기
                myTeam.AddMaxSupply(supply);
            }
            else
            {

                // 소환 시 최대 인구 증가
                myTeam.AddMaxSupply(-supply);
            }
        }
        else
        {

            if (_isDead)
            {

                myTeam.AddCurSupply(-supply);
            }
            else
            {

                myTeam.AddCurSupply(supply);
            }
        }
    }

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// 유닛 생성하고, 레이어 바꾼뒤 실행할 기능들 모아둔 메서드
    /// </summary>
    public abstract void AfterSettingLayer();

    /// <summary>
    /// 피격 메서드, 모든 유닛과 건물은 피격 가능하다!
    /// </summary>
    public virtual void OnDamaged(int _dmg, Transform _trans = null, bool _ignoreDef = false)
    {

        if (ChkInvincible()) return;

        if (_ignoreDef) curHp -= _dmg - Def < VarianceManager.MIN_DAMAGE ? VarianceManager.MIN_DAMAGE : _dmg - Def;
        else curHp -= _dmg < VarianceManager.MIN_DAMAGE ? VarianceManager.MIN_DAMAGE : _dmg;

        if (curHp < 0) curHp = 0;
        myHitBar.SetHp(curHp);

        if (curHp == 0) Dead();
        else if (InputManager.instance.curGroup.Contains(this)) UIManager.instance.UpdateHp = true;
    }

    /// <summary>
    /// 무적인지 체크 무적 체력 or 사망 상태인 경우만 true!
    /// </summary>
    /// <returns>무적 여부</returns>
    protected bool ChkInvincible()
    {

        if (MaxHp == VarianceManager.INFINITE
            || myState == STATE_SELECTABLE.DEAD)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// 사망 처리 메서드, hp바 표시, 현재 선택되면 해제, 그리고 인구조절
    /// </summary>
    public virtual void Dead()
    {

        myState = STATE_SELECTABLE.DEAD;
        // 시체 레이어로 변경
        gameObject.layer = VarianceManager.LAYER_DEAD;

        // 현재 선택 중이면 해제한다
        if (InputManager.instance.curGroup.Contains(this))
        {

            InputManager.instance.curGroup.DeSelect(this);
            InputManager.instance.ChkUIs();
            // slot과 버튼 종료!
            UIManager.instance.ExitInfo(TYPE_INFO.ALL);
        }
        
        if (InputManager.instance.curGroup.ContainsSavedGroup(this))
        {

            InputManager.instance.curGroup.DeselectSavedGroup(this);
        }

        StartCoroutine(Disabled());
    }

    public abstract void ResetTeam();

    /// <summary>
    /// 사망 시 사망 모션 얼마나 볼건지 설정
    /// </summary>
    protected IEnumerator Disabled()
    {

        if (myStat.DisableTime == 2) yield return VarianceManager.BASE_WAITFORSECONDS;
        else if (myStat.DisableTime > 0) yield return new WaitForSeconds(myStat.DisableTime);
        else yield return null;

        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    #region Info
    public void SetTitle(Text _titleTxt)
    {

        _titleTxt.text = myStat.MyName;
    }

    public abstract void SetRectTrans(RectTransform _rectTrans);

    public abstract void SetInfo(Text _descTxt);

    #endregion info

    #region command
    /// <summary>
    /// 명령을 받는다
    /// </summary>
    public abstract void GetCommand(Command _cmd, bool _add = false);

    /// <summary>
    /// 명령을 수행할 수 있는 상태인지 혹은 명령을 수행할 수 잇는지 확인
    /// </summary>
    protected abstract bool ChkCommand(Command _cmd);

    /// <summary>
    /// 예약된 명령을 받을지 확인하고 예약된 명령 시작
    /// </summary>
    protected abstract void ReadCommand(Command _cmd);
    #endregion command
}
