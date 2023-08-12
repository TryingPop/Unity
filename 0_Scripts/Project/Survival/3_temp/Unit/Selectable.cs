using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ÿ� �⺻�� �Ǵ� Ŭ����
/// �Ʊ� �ǹ�, ���� �Ӹ� �ƴ϶� �� ���ֵ� ����ϴ� ��ü�� �� �����̶� ���� �� Ŭ������ ��ӹ޴´�
/// </summary>
public abstract class Selectable : MonoBehaviour,   // ���õǾ��ٴ� UI ���� transform �� �̿��� ����
                                    IDamagable      // ��� ������ �ǰ� �����ϴ�!
{

    [Header("���� ���� ����"), Tooltip("�����뵵 ���ο������� �����ȴ�")]
    [SerializeField] protected int maxHp;           // �ִ� ü�� - ��ũ���ͺ� ������Ʈ�� �޾� �� ����������
                                                    // ���׷��̵�� ���������ϰ� ���� ���� �߰��ߴ�
    protected int curHp;                            // ���� ü��

    protected int def;

    /// <summary>
    /// �ʱ�ȭ �޼���
    /// </summary>
    protected virtual void Init()
    {

        curHp = maxHp;
    }

    /// <summary>
    /// �ǰ� �޼���, ��� ���ְ� �ǹ��� �ǰ� �����ϴ�!
    /// </summary>
    public virtual void OnDamaged(int _dmg, Transform _trans = null)
    {

        if (ChkInvincible()) return;

        curHp -= _dmg - def < 0 ? 0 : _dmg - def; 
        
        if (curHp <= 0)
        {

            Dead();
        }
    }

    /// <summary>
    /// �������� üũ
    /// </summary>
    /// <returns>���� ����</returns>
    protected virtual bool ChkInvincible()
    {

        if (maxHp == IDamagable.INVINCIBLE)
        {

            return true;
        }

        return false;
    }

    /// <summary>
    /// ��� ó�� �޼���
    /// </summary>
    public virtual void Dead()
    {

        curHp = 0;
        // ��ü ���̾�� ����
        gameObject.layer = 14;
    }

    #region command
    public abstract void DoCommand(Command _cmd, bool _add = false);

    public abstract void ReadCommand();
    #endregion command
}
