using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeManager
{

    [SerializeField] public int addTurnGold;
    [SerializeField] private int addSupply;

    public int AddTurnGold => addTurnGold;
    public int AddSupply => addSupply;
}
