using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hidden : MonoBehaviour
{

    [SerializeField] private int boomRatio;     // boomAttacker ���ݷ� �߰� ����
    [SerializeField] private float timeConquerorRatio = 2f; // �ð� ����
    [SerializeField] private float forcePow;    // Ȩ�� �Ŀ�

    [SerializeField] private AudioScript hiddenSnd;

    [SerializeField] private AudioClip homerunSnd;
    [SerializeField] private AudioClip boomSnd;

    [SerializeField] private ParticleSystem boomParticle;
    [SerializeField] private ParticleSystem homerunParticle;

    public enum Ability
    {

        None,
        Immortality,        // ���� �ʴ´�
        HealthMan,          // ���׹̳� ����
        TimeConqueror,      // �ð� ������
        boomAttacker,       // �Ŀ�Ǯ�� ����
        ContinuousAttacker, // ���� ����
        HomeRun             // �˹� ����
    }

    public Ability ability;

    /// <summary>
    /// �����Ƽ ����
    /// </summary>
    /// <param name="num">�����Ƽ ��ȣ</param>
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
