using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Script", menuName ="Script/Basic")]
public sealed class ScriptGroup : ScriptableObject
{

    [SerializeField] private Script[] scripts;

    public Script[] Scripts => scripts;
}