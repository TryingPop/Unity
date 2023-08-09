using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgrade : Buildings
{

    public const int MAXNUM = 2;
    public static int CURNUM = 0;


    // 인스펙터 창에서 다룬다!
    public Upgrade[] upgrades;

    protected bool isUpgrade;

    protected override void Awake()
    {

        CURNUM++;
        type = BuildingType.Upgrades;
    }

    protected override void Init()
    {

        base.Init();
        isUpgrade = false;
    }

    public override void Activated(int num)
    {

        if (isUpgrade)
        {

            Debug.Log("현재 업그레이드 중입니다.");
            return;
        }

        if (!upgrades[num].ChkUpgrade())
        {

            Debug.Log("더 이상 업그레이드가 불가능합니다.");
        }

        Debug.Log($"{upgrades[num].name} 업그레이드를 시작합니다.");
        
        isUpgrade = true;
        StartCoroutine(UpgradeCoroutine(num));
    }

    protected IEnumerator UpgradeCoroutine(int num)
    {

        yield return new WaitForSeconds(upgrades[num].time);

        // 최대인지 판별하고 업그레이드 실행!
        if (upgrades[num].ChkUpgrade())
        {

            upgrades[num].Upgraded();
        }
        isUpgrade = false;
    }

    protected override void Dead()
    {
        base.Dead();
        CURNUM--;
    }
}