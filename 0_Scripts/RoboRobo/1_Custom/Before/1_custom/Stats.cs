using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // 인스펙터에서 받아 올 컴포넌트나 게임오브젝트
#region Variable 
    [Header("컴포넌트 or 오브젝트")]
    [SerializeField] private    ParticleSystem      damagedParticle;    // 피격 시 생기는 파티클
    [SerializeField] protected  GameObject          damagedText;        // 데미지 수치 UI
    
    [SerializeField] protected  Status              status;             // 능력치 스크립터블 오브젝트
    [SerializeField] protected  AudioClip           damagedSnd;         // 피격 사운드
    [SerializeField] protected  ParticleSystem      atkParticle;        // 공격 시 생기는 파티클 
    [SerializeField] protected  WeaponController    myWC;               // 공격할 콜라이더를 담고 있다
    [SerializeField] protected  AudioScript         myAS;              // 소리 컨트롤러        

    [HideInInspector] public      Rigidbody       myRd;

    protected   WaitForSeconds  atkWaitTime;    // 캐싱용
    protected   Stats           targetStats;
    protected   Vector3         moveDir;        // 방향

    protected   int     nowHp;      // 현재 Hp
    protected   bool    atkBool;    // 공격 여부
    protected   bool    deadBool;   // 사망 여부
#endregion Variable

    /// <summary>
    /// 초기화 함수 Hp 초기화
    /// 추후에 GameManager LoadScene에서 쓸꺼
    /// </summary>
    protected virtual void Init()
    {

        SetHp();
    }

    /// <summary>
    /// 자신 컴포넌트 갖기
    /// </summary>
    protected virtual void GetComp()
    {

        if (myRd == null) myRd = GetComponent<Rigidbody>();
        if (myWC == null) myWC = GetComponentInChildren<WeaponController>();
        if (myAS == null) myAS = GetComponent<AudioScript>();


        if (status.AtkInterval <= 0) 
        { 
            
            atkWaitTime = null; 
        }
        else
        { 
            atkWaitTime = new WaitForSeconds(status.AtkInterval); 
        }
    }

    /// <summary>
    /// 체력과 사망상태 초기화
    /// </summary>
    protected void SetHp() 
    {

        nowHp = status.Hp;
        deadBool = false;
    }

    protected Vector3 SetDirXZ(Vector3 dir)
    {
       
        dir.y = 0;
        dir = dir.normalized;

        return dir;
    }

    /// <summary>
    /// 이동
    /// </summary>
    /// <param name="Spd">속도</param>
    protected virtual void Move(float Spd)
    {

        myRd.MovePosition(transform.position + moveDir * Spd);
    }

    protected virtual void Attack(object sender, Collider other)
    {

        myWC.AtkColActive(false);
    }

    /// <summary>
    /// 공격
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Attack()
    {
        
        atkBool = true;
        myWC.AtkColActive(true);

        yield return atkWaitTime;

        atkBool = false;
        myWC.AtkColActive(false);
    }

    /// <summary>
    /// 생존 시에만 피격 최소 데미지 1 보정, 최소 hp 0 보정
    /// </summary>
    /// <param name="atk">공격력</param>
    public virtual void OnDamaged(int atk) 
    {
        myAS.SetSnd(damagedSnd);
        myAS.GetSnd(false);

        if (!deadBool) 
        {
            
            atk -= status.Def;

            if (atk < 1)
            {

                atk = 1; 
            }

            nowHp -= atk; 

            if (nowHp < 0)
            {

                nowHp = 0;
            }
        }

        // 데미지 텍스트 생성 및 수치 확인
        if (damagedText != null)
        {
            
            GameObject obj = Instantiate(damagedText, transform);
            obj.GetComponent<DamageScript>()?.SetTxt(atk.ToString());
        }

        ChkDead();
    }

    /// <summary>
    /// hp < 0 이하이고 생존 상태일 때만 Dead 실행
    /// </summary>
    protected virtual void ChkDead()
    {

        if (nowHp <= 0 && !deadBool) 
        {

            Dead();
        }
    }


    protected virtual void Dead() 
    {
        
        deadBool = true;
    }

    /// <summary>
    /// Stats 스크립트 받아오는 메소드
    /// </summary>
    /// <param name="gameObject">대상</param>
    protected virtual void SetTargetStats(GameObject gameObject)
    {

        targetStats = gameObject.GetComponent<Stats>();
    }

    /// <summary>
    /// 공격 파티클 생성
    /// </summary>
    protected virtual void SetAtkParticle()
    {

        Instantiate(atkParticle, targetStats.transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 현재 체력 %
    /// Enemy 쪽에서만 쓸꺼
    /// </summary>
    /// <returns>현재 체력 %</returns>
    public float GetHpBar()
    {

        return (float)nowHp / status.Hp;
    }
}
