using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildOpt", menuName = "Stat/Building/BuildOpt")]
public class BuildOpt : ScriptableObject
{

    [SerializeField] protected ushort buildTurn;
    [SerializeField] protected ushort destroyIdx;
    [SerializeField] protected float increaseY;
    [SerializeField] protected float initPosY;
    protected short destroyPoolIdx = -1;

    public ushort BuildTurn => buildTurn;

    public float IncreaseY => increaseY;
    public float InitPosY => initPosY;

    public short DestroyPoolIdx
    {

        get
        {

            if (destroyPoolIdx == -1)
            {

                destroyPoolIdx = PoolManager.instance.ChkIdx(destroyIdx);
            }

            return destroyPoolIdx;
        }
    }
}