using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // 인스펙터에서 받아 올 컴포넌트나 게임오브젝트
    #region Component or GameObject
    [Header("컴포넌트 or 오브젝트")]
    [Tooltip("맞을 때 생성되는 파티클")] [SerializeField]
    private ParticleSystem attParticle; 
    
    [Tooltip("데미지 수치")] [SerializeField]
    protected GameObject damageEffect;

    [Tooltip("데미지 줄 박스 콜라이더")] [SerializeField]
    protected BoxCollider dmgCol; 
    #endregion Component or GameObject


    // 인스펙터에서 조절 가능한 변수
    #region Convertible Variable 
    [Header("스텟")]
    [Tooltip("최대 체력")] [SerializeField]
    protected int maxHp; 

    [Tooltip("공격력")] [SerializeField]
    public int atk;

    [Tooltip("방어력")] [SerializeField]
    protected int def;

    [Tooltip("히든 스텟")] [SerializeField]
    public Hidden hidden;

    [Tooltip("파워 데미지")] [SerializeField]
    public int nuclearAtk;
    #endregion Convertible Variable

    /// <summary>
    /// 히든 스텟 
    /// </summary>
    public enum Hidden { None,
                            Immortality, // 죽지 않는다
                            HealthMan, // 스테미나 무한
                            TimeConqueror, // 시간 지배자
                            NuclearAttacker, // 파워풀한 공격
                            ContinuousAttacker, // 연속 공격
                            HomeRun // 넉백 공격
                          }

    // 현재 체력
    protected int nowHp;

    // 생존 변수
    protected bool deadBool;

    // 초기 hp 설정 함수 
    protected void SetHp() 
    {

        nowHp = maxHp;
    }

    public virtual void Damaged(int _damage) // 데미지 메서드
    {

        if (!deadBool) // 사망 상태시에만 공격
        {
            
            // 데미지 계산 식 max(공격력 - 방어력, 1) 
            _damage = _damage - def;

            if (_damage < 1)
            {

                _damage = 1; // 데미지 최소값 1 보정
            }

            nowHp -= _damage; // 데미지 적용

            if (nowHp <= 0) // 체력이 음수면
            {

                nowHp = 0;

                if (hidden != Hidden.Immortality)
                {

                    Dead();
                }
            }
        }

        if (damageEffect != null)
        {
            GameObject obj = Instantiate(damageEffect, transform);
            obj.GetComponent<DamageScript>().SetTxt(_damage.ToString());
        }
    }

    protected virtual void Dead() // 사망
    {
        deadBool = true;
    }
}
