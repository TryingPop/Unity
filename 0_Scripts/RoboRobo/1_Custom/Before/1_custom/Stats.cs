using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // �ν����Ϳ��� �޾� �� ������Ʈ�� ���ӿ�����Ʈ
#region Variable 
    [Header("������Ʈ or ������Ʈ")]
    [SerializeField] private    ParticleSystem      damagedParticle;    // �ǰ� �� ����� ��ƼŬ
    [SerializeField] protected  GameObject          damagedText;        // ������ ��ġ UI
    
    [SerializeField] protected  Status              status;             // �ɷ�ġ ��ũ���ͺ� ������Ʈ
    [SerializeField] protected  AudioClip           damagedSnd;         // �ǰ� ����
    [SerializeField] protected  ParticleSystem      atkParticle;        // ���� �� ����� ��ƼŬ 
    [SerializeField] protected  WeaponController    myWC;               // ������ �ݶ��̴��� ��� �ִ�
    [SerializeField] protected  AudioScript         myAS;              // �Ҹ� ��Ʈ�ѷ�        

    [HideInInspector] public      Rigidbody       myRd;

    protected   WaitForSeconds  atkWaitTime;    // ĳ�̿�
    protected   Stats           targetStats;
    protected   Vector3         moveDir;        // ����

    protected   int     nowHp;      // ���� Hp
    protected   bool    atkBool;    // ���� ����
    protected   bool    deadBool;   // ��� ����
#endregion Variable

    /// <summary>
    /// �ʱ�ȭ �Լ� Hp �ʱ�ȭ
    /// ���Ŀ� GameManager LoadScene���� ����
    /// </summary>
    protected virtual void Init()
    {

        SetHp();
    }

    /// <summary>
    /// �ڽ� ������Ʈ ����
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
    /// ü�°� ������� �ʱ�ȭ
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
    /// �̵�
    /// </summary>
    /// <param name="Spd">�ӵ�</param>
    protected virtual void Move(float Spd)
    {

        myRd.MovePosition(transform.position + moveDir * Spd);
    }

    protected virtual void Attack(object sender, Collider other)
    {

        myWC.AtkColActive(false);
    }

    /// <summary>
    /// ����
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
    /// ���� �ÿ��� �ǰ� �ּ� ������ 1 ����, �ּ� hp 0 ����
    /// </summary>
    /// <param name="atk">���ݷ�</param>
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

        // ������ �ؽ�Ʈ ���� �� ��ġ Ȯ��
        if (damagedText != null)
        {
            
            GameObject obj = Instantiate(damagedText, transform);
            obj.GetComponent<DamageScript>()?.SetTxt(atk.ToString());
        }

        ChkDead();
    }

    /// <summary>
    /// hp < 0 �����̰� ���� ������ ���� Dead ����
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
    /// Stats ��ũ��Ʈ �޾ƿ��� �޼ҵ�
    /// </summary>
    /// <param name="gameObject">���</param>
    protected virtual void SetTargetStats(GameObject gameObject)
    {

        targetStats = gameObject.GetComponent<Stats>();
    }

    /// <summary>
    /// ���� ��ƼŬ ����
    /// </summary>
    protected virtual void SetAtkParticle()
    {

        Instantiate(atkParticle, targetStats.transform.position, Quaternion.identity);
    }

    /// <summary>
    /// ���� ü�� %
    /// Enemy �ʿ����� ����
    /// </summary>
    /// <returns>���� ü�� %</returns>
    public float GetHpBar()
    {

        return (float)nowHp / status.Hp;
    }
}
