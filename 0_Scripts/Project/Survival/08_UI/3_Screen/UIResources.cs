using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResources : MonoBehaviour
{

    [SerializeField] private Text goldTxt;
    [SerializeField] private Text supplyTxt;

    private TeamInfo teams;
    public TeamInfo Teams { set { teams = value; } }
    

    public void UpdateText()
    {

        goldTxt.text = $"{teams.Gold}";
        supplyTxt.text = $"{teams.CurSupply} / {teams.MaxSupply}";
    }
}
