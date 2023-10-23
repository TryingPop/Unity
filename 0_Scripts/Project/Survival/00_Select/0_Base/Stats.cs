using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Selectable", menuName = "Stat/Selectable")]
public class Stats : ScriptableObject
{

    [SerializeField] protected int maxHp;
    [SerializeField] protected int def;

    [SerializeField] protected TYPE_SIZE mySize;           // 유닛 사이즈 유니티 사이즈 1과 같다!
    [SerializeField] protected TYPE_SELECTABLE myType;

    [SerializeField] protected ushort selectIdx;            // 오브젝트 번호
    [SerializeField] protected short myPoolIdx;             // 풀 인덱스
    [SerializeField] protected Sprite mySprite;             // unitSlot에서 사용하는 이미지
    [SerializeField] protected float disableTime = 2f;      // 사망 모션 확인 시간

    [SerializeField] protected ushort cost;                 // 생성 비용(정가)
    [SerializeField] protected short supply;                // 만약 supply < 0 이면 최대 인구 증가, supply >= 0 이면 현재 인구 증가

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
    /// 소환과 파괴 시 자원 적용
    /// </summary>
    /// <param name="_applyCost">가격 적용?</param>
    /// <param name="_applySupply">보급 적용?</param>
    /// <param name="_myCost">스텟 가격?</param>
    /// <param name="_cost">유저 입력 가격</param>
    /// <returns>자원이 되는지 확인</returns>
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
