using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 유닛 사이즈
/// Small : 1(기본 유닛), Medium : 2(?), Large : 3(보스몹, 건물), XLarge = 4
/// </summary>
public enum STATE_SIZE { SMALL = 1, MEDIUM = 2, LARGE = 3, XLARGE = 4 }

/// <summary>
/// 선택에 기본이 되는 클래스
/// 아군 건물, 유닛 뿐만 아니라 적 유닛도 명령하는 객체를 둘 예정이라 적도 이 클래스를 상속받는다
/// </summary>
[RequireComponent(typeof(Stats)),
    RequireComponent(typeof(SightMesh))]
public abstract class Selectable : MonoBehaviour,       // 선택되었다는 UI 에서 transform 을 이용할 예정
                                    IDamagable          // 모든 유닛은 피격 가능하다!
{

    [Header("생존 관련 변수")]
    protected int maxHp;                                // 최대 체력 - 스크립터블 오브젝트로 받아 올 예정이지만
                                                        // 업그레이드로 증가가능하게 따로 변수 추가했다
    
    protected int curHp;                                // 현재 Hp
    protected int def;                                  // 방어력

    protected HitBar myHitBar;                          // 체력바

    protected AllianceInfo myAlliance;                  // 팀 정보
    protected UpgradeInfo myUpgrades;                   // 업그레이드 정보

    [SerializeField] protected Stats myStat;            // 스텟
    [SerializeField] protected SightMesh myMinimap;
    public Stats MyStat => myStat;                      

    [SerializeField] protected Selectable target;       // 목표물
    [SerializeField] protected Vector3 targetPos;       // 목표 좌표

    [SerializeField] protected ushort myTurn;           // 진행 턴 수 <<< 주변에 적 탐색, 건물 행동에서 쓰인다

    [SerializeField] protected bool isStarting = false; // 씬에 배치된 몬스터 인지 확인

    /// <summary>
    /// 스텟 설정
    /// </summary>
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

    /// <summary>
    /// 체력 회복
    /// </summary>
    public virtual void Heal(int _atk)
    {

        _atk = _atk < 0 ? 0 : _atk;
        curHp += _atk;
        if (curHp > maxHp) curHp = maxHp;
        myHitBar.SetHp(curHp);
    }

    /// <summary>
    /// 풀 Hp 인지 확인
    /// </summary>
    public bool FullHp { get { return curHp == maxHp; } }

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

                value.Init(transform, maxHp, myStat.MySize);
                value.SetHp(curHp);
            }
            myHitBar = value; 
        }
    }


    public abstract int MyState { set; get; }

    /// <summary>
    /// 5만번대 이상은 혼자 선택가능!
    /// 스타 1에서 건물 여러 개 선택 안되는 걸 따왔다
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
    /// 초기화 메서드
    /// </summary>
    protected virtual void Init()
    {

        curHp = maxHp;
    }

    /// <summary>
    /// 유닛 생성하고, 레이어 바꾼뒤 실행할 기능들 모아둔 메서드
    /// </summary>
    public abstract void AfterSettingLayer();

    /// <summary>
    /// 피격 메서드, 모든 유닛과 건물은 피격 가능하다!
    /// </summary>
    public virtual void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (ChkInvincible()) return;

        curHp -= _dmg - def < VariableManager.MIN_DAMAGE ? VariableManager.MIN_DAMAGE : _dmg - def;

        if (curHp <= 0) Dead();
        else myHitBar.SetHp(curHp);
    }

    /// <summary>
    /// 무적인지 체크
    /// </summary>
    /// <returns>무적 여부</returns>
    protected bool ChkInvincible()
    {

        if (maxHp == VariableManager.INFINITE)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// 사망 처리 메서드
    /// </summary>
    public virtual void Dead()
    {

        curHp = 0;
        myHitBar.SetHp(curHp);

        // 시체 레이어로 변경
        gameObject.layer = VariableManager.LAYER_DEAD;
        if (InputManager.instance.curGroup.IsContains(this)) 
        { 
            
            InputManager.instance.curGroup.DeSelect(this);
            InputManager.instance.ChkUIs();
        }

        StartCoroutine(Disabled());
    }

    /// <summary>
    /// 사망 시 사망 모션 얼마나 볼건지 설정
    /// </summary>
    protected IEnumerator Disabled()
    {

        yield return new WaitForSeconds(myStat.DisableTime);

        PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
    }

    /// <summary>
    /// 유닛 슬롯 UI에 현재 정보 넘길 때 사용하는 메서드
    /// </summary>
    public abstract void SetInfo(Text _txt);

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
