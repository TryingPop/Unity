using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField] protected Status status;             // 능력치 스크립터블 오브젝트
    [SerializeField] protected WeaponController myWC;     // 공격할 콜라이더를 담고 있다

    [HideInInspector] public Rigidbody myRd;

    protected WaitForSeconds atkWaitTime;    // 캐싱용 // 세분화 필요!
    protected int nowHp;      // 현재 Hp

    protected bool atkBool;    // 공격 여부
    protected bool deadBool;   // 사망 여부

    /// <summary>
    /// 자신 컴포넌트 갖기
    /// </summary>
    protected virtual void GetComp()
    {

        if (myRd == null) myRd = GetComponent<Rigidbody>();
        if (myWC == null) myWC = GetComponentInChildren<WeaponController>();

        if (status.AtkInterval <= 0) { atkWaitTime = null; }
        else { atkWaitTime = new WaitForSeconds(status.AtkInterval); }
    }


    /// <summary>
    /// hp 초기화 이외 다른거 초기화할꺼 있으면 여기에 넣기
    /// </summary>
    public virtual void Init()
    {

        SetHp();
    }

    /// <summary>
    /// 체력과 사망상태 초기화
    /// </summary>
    protected void SetHp()
    {

        nowHp = status.Hp;
        deadBool = false;
    }

    /// <summary>
    /// 공격 다른 쪽에서 이벤트로 쓸 때 사용할 것
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="other"></param>
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
    /// 피격
    /// </summary>
    /// <param name="atk">공격력</param>
    public virtual void OnDamaged(int atk)
    {

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

    /// <summary>
    /// 사망 상태
    /// </summary>
    protected virtual void Dead()
    {

        deadBool = true;
    }
}