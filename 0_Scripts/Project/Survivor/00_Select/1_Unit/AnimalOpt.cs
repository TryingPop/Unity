using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalOpt", menuName = "Stat/AnimalOpt")]
public class AnimalOpt : ScriptableObject
{

    [SerializeField] protected ushort idleWeight;               // 대기 가중치
    [SerializeField] protected ushort moveWeight;               // 이동 가중치

    protected ushort sumWeight;                                 // 총합 가중치 - 연산용
    [SerializeField]protected ushort moveRange;                 // 이동 범위

    [SerializeField] protected ushort[] killGolds;              // 여기서 랜덤으로 준다
    [SerializeField] protected float[] actionTimer;             // 여기서 랜덤 행동 시간 설정

    protected WaitForSeconds[] waitTimes;                       // 캐싱용

    [SerializeField] protected ushort deadEffectIdx;            // 사망 이펙트의 번호
    protected short prefabIdx = -1;

    public ushort KillGold
    {

        get
        {

            if (killGolds == null)
            {

                killGolds = new ushort[1] { 1 };
            }

            return killGolds[Random.Range(0, killGolds.Length)];
        }
    }

    public WaitForSeconds WaitTime
    {

        get
        {

            if (waitTimes == null)
            {

                if (actionTimer == null)
                {

                    actionTimer = new float[1] { 5f };
                }

                waitTimes = new WaitForSeconds[actionTimer.Length];

                for (int i = 0; i < actionTimer.Length; i++)
                {

                    if (actionTimer[i] <= 0f)
                    {

                        actionTimer[i] = 1f;
                    }

                    waitTimes[i] = new WaitForSeconds(actionTimer[i]);
                }
            }

            return waitTimes[Random.Range(0, waitTimes.Length)];
        }
    }
    public int RandActions
    {

        get
        {

            if (sumWeight == 0) sumWeight = (ushort)(idleWeight + moveWeight);

            int num = Random.Range(0, sumWeight);
            if (num < idleWeight)
            {

                return 0;
            }

            return 1;
        }
    }

    public int MoveRange => moveRange;

    public short PrefabIdx
    {

        get
        {

            if (prefabIdx == -1)
            {

                prefabIdx = PoolManager.instance.ChkIdx(deadEffectIdx);
            }

            return prefabIdx;
        }
    }
}
