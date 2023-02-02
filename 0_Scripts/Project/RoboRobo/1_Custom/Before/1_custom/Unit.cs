using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Stat
{

    // 인스펙터에서 받아 올 컴포넌트나 게임오브젝트
#region Variable 
    [Header("컴포넌트 or 오브젝트")]
    [SerializeField] private    ParticleSystem      damagedParticle;    // 피격 시 생기는 파티클
    [SerializeField] protected  GameObject          damagedText;        // 데미지 수치 UI
    
    
    [SerializeField] protected  AudioClip           damagedSnd;         // 피격 사운드
    [SerializeField] protected  ParticleSystem      atkParticle;        // 공격 시 생기는 파티클 
    
    [SerializeField] protected  AudioScript         myAS;              // 소리 컨트롤러        
    
    protected   Stat            targetStats;     // 세분화 필요!
    protected   Vector3         moveDir;        // 방향

    
#endregion Variable

    /// <summary>
    /// 자신 컴포넌트 갖기
    /// </summary>
    protected override void GetComp()
    {

        if (myAS == null) myAS = GetComponent<AudioScript>();

        base.GetComp();
    }


    /// <summary>
    /// 바라볼 방향 설정
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
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



    /// <summary>
    /// 생존 시에만 피격 최소 데미지 1 보정, 최소 hp 0 보정
    /// </summary>
    /// <param name="atk">공격력</param>
    public override void OnDamaged(int atk) 
    {
        myAS.SetSnd(damagedSnd);
        myAS.GetSnd(false);

        base.OnDamaged(atk);

        // 데미지 텍스트 생성 및 수치 확인
        if (damagedText != null)
        {
            
            GameObject obj = Instantiate(damagedText, transform);
            obj.GetComponent<DamageScript>()?.SetTxt(atk.ToString());
        }

        ChkDead();
    }

    /// <summary>
    /// Stats 스크립트 받아오는 메소드
    /// </summary>
    /// <param name="gameObject">대상</param>
    protected virtual void SetTargetStats(GameObject gameObject)
    {

        targetStats = gameObject.GetComponent<Stat>();
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
