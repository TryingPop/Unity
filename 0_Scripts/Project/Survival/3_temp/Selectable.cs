using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 선택에 기본이 되는 클래스
/// 아군 건물, 유닛 뿐만 아니라 적 유닛도 명령하는 객체를 둘 예정이라 적도 이 클래스를 상속받는다
/// </summary>
public abstract class Selectable : MonoBehaviour,   // 선택되었다는 UI 에서 transform 을 이용할 예정
                                    IDamagable      // 모든 유닛은 피격 가능하다!
{

    [Header("생존 관련 변수"), Tooltip("디버깅용도 내부연산으로 설정된다")]
    [SerializeField] protected int maxHp;           // 최대 체력 - 스크립터블 오브젝트로 받아 올 예정이지만
                                                    // 업그레이드로 증가가능하게 따로 변수 추가했다
    protected int curHp;                            // 현재 체력

    [SerializeField] protected bool isLive;         // 생존 여부

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    protected virtual void Init()
    {

        curHp = maxHp;
        isLive = true;
    }

    /// <summary>
    /// 피격 메서드, 모든 유닛과 건물은 피격 가능하다!
    /// </summary>
    public virtual void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (ChkInvincible()) return;

        curHp -= _dmg;

        if (curHp <= 0)
        {

            Dead();
        }
    }

    /// <summary>
    /// 무적인지 체크
    /// </summary>
    /// <returns>무적 여부</returns>
    protected virtual bool ChkInvincible()
    {

        if (maxHp == IDamagable.INVINCIBLE)
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

        isLive = false;
        curHp = 0;
    }

    #region command
    public abstract void AddCommand(Command _cmd);

    public abstract void SetCommand(Command.TYPE _type, Vector3 _pos, Transform _target);
    #endregion command
}
