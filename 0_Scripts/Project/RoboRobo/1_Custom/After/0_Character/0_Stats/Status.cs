using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Status", menuName = "Scriptable Object/Status", order = int.MaxValue)]
public class Status : ScriptableObject
{

    [SerializeField] private int atk;
    public int Atk { get { return atk; } }

    [SerializeField] private int def;
    public int Def { get { return def; } }

    [SerializeField] private int hp;
    public int Hp { get { return hp; } }


    [SerializeField] private float moveSpd;
    public float MoveSpd { get { return moveSpd; } }

    [SerializeField] private float atkInterval;
    public float AtkInterval { get { return atkInterval; } }
}

