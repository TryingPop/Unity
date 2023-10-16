using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : ScriptableObject
{

    [SerializeField] protected bool isPlayer;
    public abstract bool IsSucess { get; }
    public bool IsPlayer => isPlayer;

    public abstract void Init(GameManager _gameManager);

    public abstract void Chk(Unit _unit, Building _building);
}
