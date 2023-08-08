using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResource : Buildings
{

    [SerializeField] protected int amount;              // ���귮      -- �����ϴ� �ڿ��� 1���� ���� ������ �����ϴ�..
    [SerializeField] protected float productionTime;    // ���� ����

    protected WaitForSeconds waitTime;
    public const int MAXNUM = 5;
    public static int CURNUM = 0;

    protected override void Awake()
    {

        CURNUM++;
        type = BuildingType.Resources;
        waitTime = new WaitForSeconds(productionTime);
        Activated(0);
    }


    public override void Activated(int num)
    {

        StartCoroutine(ResourceCoroutine(num));
    }

    /// <summary>
    /// ����Ƚ���� 0 ���ϸ� �ڿ����� ���� �ݺ�!
    /// </summary>
    /// <param name="num">����Ƚ��</param>
    protected IEnumerator ResourceCoroutine(int num)
    {

        if (num <= 0)
        {

            while (true)
            {

                yield return waitTime;

                // amount ��ŭ ����
                Debug.Log($"�ڿ��� {amount}��ŭ �����Ͽ����ϴ�.");
            }
        }
        else
        {

            for (int i = 0; i < num; i++)
            {

                yield return waitTime;

                // �ڿ� ����
            }
        }
    }
}
