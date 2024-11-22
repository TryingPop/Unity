using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Selectable", menuName = "Stat/Selectable")]
public class Stats : ScriptableObject
{

    [SerializeField] protected int maxHp;
    [SerializeField] protected int def;
    [SerializeField] protected int evade;
    [SerializeField] protected int sight;

    [SerializeField] protected TYPE_SIZE mySize;            // ���� ������ ����Ƽ ������ 1�� ����!
    [SerializeField] protected TYPE_SELECTABLE myType;

    [SerializeField] protected int selectIdx;               // ������Ʈ ��ȣ
    [SerializeField] protected int myPoolIdx;               // Ǯ �ε���
    [SerializeField] protected Sprite mySprite;             // unitSlot���� ����ϴ� �̹���
    [SerializeField] protected float disableTime = 2f;      // ��� ��� Ȯ�� �ð�

    [SerializeField] protected int cost;                    // ���� ���(����)
    [SerializeField] protected int supply;                  // ���� supply < 0 �̸� �ִ� �α� ����, supply >= 0 �̸� ���� �α� ����

    [SerializeField] protected string myName;
    [SerializeField] protected int hitBarPos;               // ü�¹� y offset

    [SerializeField] protected int addedHp;
    [SerializeField] protected int addedDef;
    [SerializeField] protected int addedEvade;


    public int MaxHp => maxHp;

    public int Def => def;

    public int Evade => evade;

    public int Sight => sight;

    public int GetMaxHp(int _upgrade)
    {

        return maxHp + _upgrade * addedHp;
    }

    public int GetDef(int _upgrade)
    {

        return def + _upgrade * addedDef;
    }

    public int GetAddHp(int _upgrade)
    {

        return addedHp * _upgrade;
    }

    public int GetAddDef(int _upgrade)
    {

        return addedDef * _upgrade;
    }

    public int GetAddEvade(int _upgrade)
    {

        return addedEvade * _upgrade;
    }

    public int MySize => (int)mySize;

    public int SelectIdx
    {

        get
        {

            return selectIdx;
        }

    }

    public int MyPoolIdx
    {
        get
        {

            if (myPoolIdx == VarianceManager.POOLMANAGER_NOTEXIST)
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
    public int Cost => cost;
    public int Supply => supply;

    public string MyName => myName;

    public int HitBarPos => hitBarPos;
}
