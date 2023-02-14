using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour
{

    [SerializeField] private int boomRatio;     // boomAttacker 공격력 추가 비율
    [SerializeField] private float timeConquerorRatio = 2f; // 시간 비율
    [SerializeField] private float forcePow;    // 홈런 파워

    [SerializeField] private AudioScript hiddenSnd;

    [SerializeField] private AudioClip homerunSnd;
    [SerializeField] private AudioClip boomSnd;

    [SerializeField] private ParticleSystem boomParticle;
    [SerializeField] private ParticleSystem homerunParticle;

    public enum Ability
    {

        None,
        Immortality,        // 죽지 않는다
        HealthMan,          // 스테미나 무한
        TimeConqueror,      // 시간 지배자
        boomAttacker,       // 파워풀한 공격
        ContinuousAttacker, // 연속 공격
        HomeRun             // 넉백 공격
    }

    public Ability ability;

    /// <summary>
    /// 어빌리티 설정
    /// </summary>
    /// <param name="num">어빌리티 번호</param>
    public void SetAbility(int num)
    {

        ability = (Ability)num;
    }

    public bool ChkAbility(Ability chkAbility)
    {

        if( ability == chkAbility)
        {

            return true;
        }
        else
        {

            return false;
        }
    }

    public float ChkTime()
    {

        if (ChkAbility(Ability.TimeConqueror))
        {

            return Time.deltaTime * timeConquerorRatio;
        }
        else
        {

            return Time.deltaTime;
        }
    }

    public int ChkAtk(int atk)
    {
        if (ChkAbility(Ability.boomAttacker))
        {

            return atk * boomRatio;
        }
        else 
        {

            return atk;
        }
    }

    public ParticleSystem ChkParticle(ParticleSystem particle)
    {

        if (ChkAbility(Ability.boomAttacker))
        {

            return boomParticle;
        }
        else if (ChkAbility(Ability.HomeRun))
        {

            return homerunParticle;
        }
        else
        {

            return particle;
        }
    }

    public void ChkAtkSnd()
    {

        if (ChkAbility(Ability.boomAttacker))
        {

            hiddenSnd.SetSnd(boomSnd);
            hiddenSnd.GetSnd(false);
        }
        else if (ChkAbility(Ability.HomeRun))
        {

            hiddenSnd.SetSnd(homerunSnd);
            hiddenSnd.GetSnd(true);
        }
    }

    public void ChkKnockBack(Rigidbody rd)
    {
        if (ChkAbility(Ability.HomeRun))
        {

            Vector3 _forceDir = ((rd.transform.position - transform.position).normalized + transform.up);
            rd.AddForce(_forceDir * forcePow, ForceMode.Impulse);
        }
    }
}
