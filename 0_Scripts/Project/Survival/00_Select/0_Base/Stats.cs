using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Stat", menuName = "Status/Stat")]
public class Stats : ScriptableObject
{

    [SerializeField] protected int maxHp;

    [SerializeField] protected int def;

    [SerializeField] protected STATE_SIZE mySize;
    [SerializeField] protected TYPE_SELECTABLE myType;

    [SerializeField] protected ushort selectIdx;
    [SerializeField] protected short myPoolIdx;

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
}
