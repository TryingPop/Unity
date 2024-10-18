using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public static TurnManager instance;

    [SerializeField] private int turnSecond = 10;           // 턴 간격
    private int turnTime;                                   // 현재 턴
    private int turn;                                       // 턴간격 계산해서 넣어준다
    private int gold;                                       // 턴골드

    [SerializeField] private int turnGold = 0;              // 초기 골드

    [SerializeField] private TeamInfo teamInfo;

    public int AddTurnGold 
    {
    
        set 
        { 
            
            turnGold += value;
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

        gold = turnGold;
    }

    private void GetGold()
    {

        if (gold <= 0) return;
        teamInfo.AddGold(gold);
        UIManager.instance.SetChat($"턴 골드 +{gold}");
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