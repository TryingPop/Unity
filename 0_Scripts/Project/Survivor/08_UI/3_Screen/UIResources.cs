using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResources : MonoBehaviour
{

    [SerializeField] private Text goldTxt;
    [SerializeField] private Text supplyTxt;

    private bool isSupplyRed;

    private TeamInfo teams;
    public TeamInfo Teams { set { teams = value; } }
    

    public void UpdateText()
    {
        
        goldTxt.text = $"{teams.Gold:N0}";                             // 문자열 보간 세 자리마다 , 표현

        if (teams.CurSupply > teams.MaxSupply)
        {

            if (!isSupplyRed)
            {

                isSupplyRed = true;
                supplyTxt.color = Color.red;
            }
        }
        else 
        {

            if (isSupplyRed)
            {

                isSupplyRed = false;
                supplyTxt.color = Color.white;
            }
        }

        supplyTxt.text = $"{teams.CurSupply} / {teams.MaxSupply}";     // 띄어쓰기 보간은 안된다
    }
}