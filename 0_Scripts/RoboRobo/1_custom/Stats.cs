using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    // �ν����Ϳ��� �޾� �� ������Ʈ�� ���ӿ�����Ʈ
    #region Component or GameObject
    [Header("������Ʈ or ������Ʈ")]
    [Tooltip("���� �� �����Ǵ� ��ƼŬ")] [SerializeField]
    private ParticleSystem attParticle; 
    
    [Tooltip("������ ��ġ")] [SerializeField]
    protected GameObject damageEffect;

    [Tooltip("������ �� �ڽ� �ݶ��̴�")] [SerializeField]
    protected BoxCollider dmgCol; 
    #endregion Component or GameObject


    // �ν����Ϳ��� ���� ������ ����
    #region Convertible Variable 
    [Header("����")]
    [Tooltip("�ִ� ü��")] [SerializeField]
    protected int maxHp; 

    [Tooltip("���ݷ�")] [SerializeField]
    public int atk;

    [Tooltip("����")] [SerializeField]
    protected int def;

    [Tooltip("���� ����")] [SerializeField]
    public Hidden hidden;

    [Tooltip("�Ŀ� ������")] [SerializeField]
    public int nuclearAtk;
    #endregion Convertible Variable

    /// <summary>
    /// ���� ���� 
    /// </summary>
    public enum Hidden { None,
                            Immortality, // ���� �ʴ´�
                            HealthMan, // ���׹̳� ����
                            TimeConqueror, // �ð� ������
                            NuclearAttacker, // �Ŀ�Ǯ�� ����
                            ContinuousAttacker, // ���� ����
                            HomeRun // �˹� ����
                          }

    // ���� ü��
    protected int nowHp;

    // ���� ����
    protected bool deadBool;

    // �ʱ� hp ���� �Լ� 
    protected void SetHp() 
    {

        nowHp = maxHp;
    }

    public virtual void Damaged(int _damage) // ������ �޼���
    {

        if (!deadBool) // ��� ���½ÿ��� ����
        {
            
            // ������ ��� �� max(���ݷ� - ����, 1) 
            _damage = _damage - def;

            if (_damage < 1)
            {

                _damage = 1; // ������ �ּҰ� 1 ����
            }

            nowHp -= _damage; // ������ ����

            if (nowHp <= 0) // ü���� ������
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

    protected virtual void Dead() // ���
    {
        deadBool = true;
    }
}
