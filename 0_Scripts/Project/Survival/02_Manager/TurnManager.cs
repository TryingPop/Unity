using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public static TurnManager instance;

    [SerializeField] private int turnSecond = 10;
    private int turnTime;
    private int turn;
    private int gold;

    [SerializeField] private int turnGold = 0;
    [SerializeField] private int cntGoldBuilding = 0;

    [SerializeField] private TeamInfo teamInfo;

    public int AddTurnGold 
    {
    
        set 
        { 
            
            turnGold += value;
            CalcGold();
        } 
    }
    public int AddCntGoldBuilding 
    { 
        
        set 
        { 
            
            cntGoldBuilding += value;
            CalcGold();
        } 
    }
    public TeamInfo TeamInfo { set { TeamInfo = value; } }

    private void Awake()
    {

        if (instance != null) Destroy(gameObject);
        instance = this;
        
    }

    private void Start()
    {

        turnTime = turnSecond * (int)(1.0/ Time.fixedDeltaTime);
        teamInfo = TeamManager.instance.PlayerTeamInfo;
        CalcGold();
    }

    
    public void CalcGold()
    {

        gold = cntGoldBuilding * teamInfo.lvlGetTurnGold + turnGold;
    }

    private void GetGold()
    {

        if (gold <= 0) return;
        teamInfo.AddGold(gold);
        UIManager.instance.SetChat($"ÅÏ °ñµå +{gold}");
    }

    public void FixedUpdate()
    {

        turn++;
        while (turnTime <= turn)
        {

            turn -= turnTime;
            GetGold();
        }
    }
}