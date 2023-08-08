using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour, IDamagable
{

    public int maxHp;
    public int curHp;

    public enum BuildingType
    {

        None,                   // 기능 없는 건물 - 벽
        Units,                  // 유닛 생산 건물
        Resources,              // 자원 획득 건물
        Upgrades,               // 업그레이드 건물
    }

    protected BuildingType type;


    protected virtual void Awake()
    {
        
        type = BuildingType.None;
    }

    protected void OnEnable()
    {

        Init();
    }

    protected virtual void Init()
    {

        curHp = maxHp;
    }


    public void OnDamaged(int _dmg)
    {

        curHp -= _dmg;
        if (curHp < 0)
        {

            curHp = 0;
            gameObject.SetActive(false);        // 최대 갯수를 줘서
                                                // 재활용 할 예정
        }
    }

    /// <summary>
    /// 활성화
    /// </summary>
    /// <param name="num">번호</param>
    public virtual void Activated(int num) { }
}
