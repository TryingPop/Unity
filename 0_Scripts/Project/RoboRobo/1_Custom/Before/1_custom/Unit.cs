using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Stat
{

    // �ν����Ϳ��� �޾� �� ������Ʈ�� ���ӿ�����Ʈ
#region Variable 
    [Header("������Ʈ or ������Ʈ")]
    [SerializeField] private    ParticleSystem      damagedParticle;    // �ǰ� �� ����� ��ƼŬ
    [SerializeField] protected  GameObject          damagedText;        // ������ ��ġ UI
    
    
    [SerializeField] protected  AudioClip           damagedSnd;         // �ǰ� ����
    [SerializeField] protected  ParticleSystem      atkParticle;        // ���� �� ����� ��ƼŬ 
    
    [SerializeField] protected  AudioScript         myAS;              // �Ҹ� ��Ʈ�ѷ�        
    
    protected   Stat            targetStats;     // ����ȭ �ʿ�!
    protected   Vector3         moveDir;        // ����

    
#endregion Variable

    /// <summary>
    /// �ڽ� ������Ʈ ����
    /// </summary>
    protected override void GetComp()
    {

        if (myAS == null) myAS = GetComponent<AudioScript>();

        base.GetComp();
    }


    /// <summary>
    /// �ٶ� ���� ����
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
    /// �̵�
    /// </summary>
    /// <param name="Spd">�ӵ�</param>
    protected virtual void Move(float Spd)
    {

        myRd.MovePosition(transform.position + moveDir * Spd);
    }



    /// <summary>
    /// ���� �ÿ��� �ǰ� �ּ� ������ 1 ����, �ּ� hp 0 ����
    /// </summary>
    /// <param name="atk">���ݷ�</param>
    public override void OnDamaged(int atk) 
    {
        myAS.SetSnd(damagedSnd);
        myAS.GetSnd(false);

        base.OnDamaged(atk);

        // ������ �ؽ�Ʈ ���� �� ��ġ Ȯ��
        if (damagedText != null)
        {
            
            GameObject obj = Instantiate(damagedText, transform);
            obj.GetComponent<DamageScript>()?.SetTxt(atk.ToString());
        }

        ChkDead();
    }

    /// <summary>
    /// Stats ��ũ��Ʈ �޾ƿ��� �޼ҵ�
    /// </summary>
    /// <param name="gameObject">���</param>
    protected virtual void SetTargetStats(GameObject gameObject)
    {

        targetStats = gameObject.GetComponent<Stat>();
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
