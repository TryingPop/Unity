using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Selectable", menuName = "Stat/Selectable")]
public class Stats : ScriptableObject
{

    [SerializeField] protected int maxHp;
    [SerializeField] protected int def;

    [SerializeField] protected TYPE_SIZE mySize;           // ���� ������ ����Ƽ ������ 1�� ����!
    [SerializeField] protected TYPE_SELECTABLE myType;

    [SerializeField] protected ushort selectIdx;            // ������Ʈ ��ȣ
    [SerializeField] protected short myPoolIdx;             // Ǯ �ε���
    [SerializeField] protected Sprite mySprite;             // unitSlot���� ����ϴ� �̹���
    [SerializeField] protected float disableTime = 2f;      // ��� ��� Ȯ�� �ð�

    [SerializeField] protected ushort cost;                 // ���� ���(����)
    [SerializeField] protected short supply;                // ���� supply < 0 �̸� �ִ� �α� ����, supply >= 0 �̸� ���� �α� ����

    public int MaxHp => maxHp;
    public int Def => def;

    public int MySize => (int)mySize;

    public ushort SelectIdx
    {

        get
        {

            return selectIdx;
        }

    }

    public short MyPoolIdx
    {
        get
        {

            if (myPoolIdx == VariableManager.POOLMANAGER_NOTEXIST)
            {


                myPoolIdx = PoolManager.instance.ChkIdx(selectIdx);
            }

            return myPoolIdx;
        }
    }

    public TYPE_SELECTABLE MyType => myType;

    public Sprite MySprite => mySprite;

    public float DisableTime
    {
        get
        {

            if (disableTime < 0.1f) disableTime = 0.1f;
            return disableTime;
        }
    }
    public ushort Cost => cost;
    public short Supply => supply;

    /// <summary>
    /// ��ȯ�� �ı� �� �ڿ� ����
    /// </summary>
    /// <param name="_applyCost">���� ����?</param>
    /// <param name="_applySupply">���� ����?</param>
    /// <param name="_myCost">���� ����?</param>
    /// <param name="_cost">���� �Է� ����</param>
    /// <returns>�ڿ��� �Ǵ��� Ȯ��</returns>
    public bool ApplyResources(bool _isRegen, bool _applyCost = false, bool _applySupply = true, 
        bool _myCost = false, int _cost = 0)
    {

        ResourceManager resourceManager = ResourceManager.instance;

        int chkCost = _myCost ? cost : _cost;
        

        if (_isRegen)
        {

            if (_applyCost && _applySupply
                && resourceManager.ChkResources(chkCost, supply))
            {

                resourceManager.UseResources(chkCost, supply);
                return true;
            }
            else if (_applyCost && !_applySupply
                && resourceManager.ChkResources(TYPE_MANAGEMENT.GOLD, chkCost))
            {

                resourceManager.UseResources(TYPE_MANAGEMENT.GOLD, chkCost);
                return true;
            }
            else if (!_applyCost && _applySupply)
            {

                resourceManager.UseResources(TYPE_MANAGEMENT.SUPPLY, supply);
                return true;
            }

            return false;
        }
        else
        {

            if (_applyCost) resourceManager.AddResources(TYPE_MANAGEMENT.GOLD, chkCost);
            if (_applySupply) resourceManager.AddResources(TYPE_MANAGEMENT.SUPPLY, supply);
            return true;
        }
    }
}
