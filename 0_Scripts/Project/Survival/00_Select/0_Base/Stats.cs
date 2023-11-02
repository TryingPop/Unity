using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Selectable", menuName = "Stat/Selectable")]
public class Stats : ScriptableObject
{

    [SerializeField] protected int maxHp;
    [SerializeField] protected int def;

    [SerializeField] protected TYPE_SIZE mySize;            // 유닛 사이즈 유니티 사이즈 1과 같다!
    [SerializeField] protected TYPE_SELECTABLE myType;

    [SerializeField] protected int selectIdx;               // 오브젝트 번호
    [SerializeField] protected int myPoolIdx;               // 풀 인덱스
    [SerializeField] protected Sprite mySprite;             // unitSlot에서 사용하는 이미지
    [SerializeField] protected float disableTime = 2f;      // 사망 모션 확인 시간

    [SerializeField] protected int cost;                    // 생성 비용(정가)
    [SerializeField] protected int supply;                  // 만약 supply < 0 이면 최대 인구 증가, supply >= 0 이면 현재 인구 증가

    [SerializeField] protected string myName;
    [SerializeField] protected int hitBarPos;

    public int MaxHp => maxHp;
    public int Def => def;

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
