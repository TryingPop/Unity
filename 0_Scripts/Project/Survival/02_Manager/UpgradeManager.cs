using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeManager
{

    [SerializeField] public int addGold;
    [SerializeField] private int addSupply;

    public int AddGold => addGold;
    public int AddSupply => addSupply;
}
