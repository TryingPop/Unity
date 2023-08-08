using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResource : Buildings
{

    [SerializeField] protected int amount;              // 생산량      -- 생산하는 자원은 1개로 하자 많으면 복잡하다..
    [SerializeField] protected float productionTime;    // 생산 간격

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
    /// 시행횟수가 0 이하면 자원생산 무한 반복!
    /// </summary>
    /// <param name="num">시행횟수</param>
    protected IEnumerator ResourceCoroutine(int num)
    {

        if (num <= 0)
        {

            while (true)
            {

                yield return waitTime;

                // amount 만큼 생산
                Debug.Log($"자원을 {amount}만큼 생성하였습니다.");
            }
        }
        else
        {

            for (int i = 0; i < num; i++)
            {

                yield return waitTime;

                // 자원 생산
            }
        }
    }
}
