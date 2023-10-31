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

        
        goldTxt.text = $"{teams.Gold:N0}";                             // 문자열 보간 세 자리마다 , 표현
        supplyTxt.text = $"{teams.CurSupply} / {teams.MaxSupply}";     // 띄어쓰기 보간은 안된다
    }
}
