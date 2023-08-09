using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgrade : Buildings
{

    public const int MAXNUM = 2;
    public static int CURNUM = 0;


    // �ν����� â���� �ٷ��!
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

            Debug.Log("���� ���׷��̵� ���Դϴ�.");
            return;
        }

        if (!upgrades[num].ChkUpgrade())
        {

            Debug.Log("�� �̻� ���׷��̵尡 �Ұ����մϴ�.");
        }

        Debug.Log($"{upgrades[num].name} ���׷��̵带 �����մϴ�.");
        
        isUpgrade = true;
        StartCoroutine(UpgradeCoroutine(num));
    }

    protected IEnumerator UpgradeCoroutine(int num)
    {

        yield return new WaitForSeconds(upgrades[num].time);

        // �ִ����� �Ǻ��ϰ� ���׷��̵� ����!
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