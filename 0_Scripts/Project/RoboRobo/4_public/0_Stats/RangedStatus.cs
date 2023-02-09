using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedStatus", menuName = "Scriptable Object/RangedStatus", order = int.MaxValue)]
public class RangedStatus : ScriptableObject
{
    [SerializeField] private float rangeRadius;
    public float RangeRadius { get { return rangeRadius; } }

    [SerializeField] private float rangeAngle;
    public float RangeAngle { get { return rangeAngle; } }
}
