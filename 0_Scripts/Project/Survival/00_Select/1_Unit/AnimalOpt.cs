using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalOpt", menuName = "Stat/AnimalOpt")]
public class AnimalOpt : ScriptableObject
{



    [SerializeField] protected ushort idleWeight;
    [SerializeField] protected ushort moveWeight;

    protected ushort sumWeight;
    [SerializeField]protected ushort moveRange;

    [SerializeField] protected ushort[] killGolds;
    [SerializeField] protected float[] actionTimer;

    protected WaitForSeconds[] waitTimes;

    [SerializeField] protected ushort deadEffectIdx;
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
