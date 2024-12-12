using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 선택에 기본이 되는 클래스
/// 아군 건물, 유닛 뿐만 아니라 적 유닛도 명령하는 객체를 둘 예정이라 적도 이 클래스를 상속받는다
/// </summary>
public abstract class BaseObj : Commandable,         // 명령 가능 오브젝트 -> 선택과 행동가능!
                                    IDamagable          // 모든 유닛은 피격 가능하다!
                                          
{

    protected int curHp;                                // 현재 Hp

    [Header("스텟")]
    [SerializeField] protected int myTurn;              // 진행 턴 수 <<< 주변에 적 탐색, 건물 행동에서 쓰인다

    protected HitBar myHitBar;                          // 체력바

    [SerializeField] protected Stats myStat;            // 스텟
    [SerializeField] protected SightMesh myMinimap;
    public Stats MyStat => myStat;                      

    [SerializeField] protected BaseObj target;          // 목표물
    [SerializeField] protected Vector3 targetPos;       // 목표 좌표

    [SerializeField] protected SoundGroup mySound;
    [SerializeField] protected AudioSource myAudio;

    /// <summary>
    /// 체력 회복
    /// </summary>
    public virtual void Heal(int _atk)
    {

        _atk = _atk < 0 ? 0 : _atk;
        curHp += _atk;
        if (curHp > MaxHp) curHp = MaxHp;
        myHitBar.SetHp();

        // 현재 선택 중이면 해제한다!
        if (PlayerManager.instance.curGroup.Contains(this))
        {

            UIManager.instance.UpdateHp = true;
        }
    }

    /// <summary>
    /// 풀 Hp 인지 확인
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

            if (value < 0) Debug.LogError("턴 입력값이 0입니다.");
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

                Debug.LogWarning($"{name} 오브젝트의 hitbar가 null입니다.");
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

            Debug.Log($"{name}은 유닛인데 팀 정보가 없습니다.");
#endif
            return; 
        }

        int supply = myStat.Supply;
        
        if (supply < 0)
        {

            // 미완성인 경우는 탈출!
            if (myState == MY_STATE.GAMEOBJECT.BUILDING_UNFINISHED) return;

            if (_isDead)
                // 사망 시 이므로 최대 인구 깎기
                myTeam.AddMaxSupply(supply);
            else
                // 소환 시 최대 인구 증가
                myTeam.AddMaxSupply(-supply);
        }
        else
        {

            if (_isDead) myTeam.AddCurSupply(-supply);
            else myTeam.AddCurSupply(supply);
        }
    }

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    protected abstract void Init();

    /// <summary>
    /// 유닛 생성하고, 레이어 바꾼뒤 실행할 기능들 모아둔 메서드
    /// </summary>
    public abstract void ChkTeamStat();

    /// <summary>
    /// 피격 메서드, 모든 유닛과 건물은 피격 가능하다!
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
    /// 무적인지 체크 무적 체력 or 사망 상태인 경우만 true!
    /// </summary>
    /// <returns>무적 여부</returns>
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
    /// 회피 여부
    /// </summary>
    protected bool ChkEvade()
    {

        int random = Random.Range(0, 101);
        int evade = myStat.Evade;

        return random < evade;
    }

    /// <summary>
    /// 사망 처리 메서드, hp바 표시, 현재 선택되면 해제, 그리고 인구조절
    /// </summary>
    public virtual void Dead(bool _immediately = false)
    {

        myState = MY_STATE.GAMEOBJECT.DEAD;
        // 시체 레이어로 변경
        gameObject.layer = VarianceManager.LAYER_DEAD;

        if (_immediately) PoolManager.instance.UsedPrefab(gameObject, MyStat.MyPoolIdx);
        else StartCoroutine(Disabled());
    }

    /// <summary>
    /// 현재 그룹 해제
    /// </summary>
    protected void CurGroupDeSelect()
    {

        if (PlayerManager.instance.curGroup.Contains(this))
        {

            PlayerManager.instance.curGroup.DeSelect(this);
            PlayerManager.instance.ChkUIs();
            // slot과 버튼 종료!
            UIManager.instance.ExitInfo(MY_TYPE.UI.ALL);
        }
    }

    /// <summary>
    /// 저장된 그룹 해제
    /// </summary>
    protected void SavedGroupDeSelect()
    {

        if (PlayerManager.instance.curGroup.ContainsSavedGroup(this))
            PlayerManager.instance.curGroup.DeselectSavedGroup(this);
    }

    /// <summary>
    /// 선택되어 있으면 해제
    /// </summary>
    public virtual void ResetTeamStat()
    {

        CurGroupDeSelect();

        SavedGroupDeSelect();
    }

    /// <summary>
    /// 사망 시 사망 모션 얼마나 볼건지 설정
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
