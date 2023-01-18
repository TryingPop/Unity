using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;


[Obsolete("PlayerInfo 참조하기!")]
[Serializable] // 객체끼리 연결시켜주는 직렬화 어트리뷰트
public class User // 유저 객체
{
    // 추후에 필요하면 속성과 인스턴스 변수 추가
    public string name;
    public int win; 
    public int lose; 
    public int score; 

    // 생성자
    public User(string name = "", int win = 0, int lose = 0, int score = 100)
    {
        this.name = name;
        this.win = win;
        this.lose = lose;
        this.score = score;
    }

    
    public double GetWinRate()
    {
        return Math.Round(((double)win / (double)(win + lose))*100f, 1);
    }
}
