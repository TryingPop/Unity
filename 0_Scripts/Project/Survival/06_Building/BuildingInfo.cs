using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingInfo
{

    public enum GENERATOR_TYPE { NONE, RESOURCE, INFANTRY, UPGRADE }

    public GENERATOR_TYPE myType;

    [Tooltip("Generator Type")]
    public short amount;
    public short time;
}
