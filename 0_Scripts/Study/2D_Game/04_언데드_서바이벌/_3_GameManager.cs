using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _3_GameManager : MonoBehaviour
{

    // 장면이 하나라서 싱글톤으로는 안만든다
    public static _3_GameManager instance;

    [Header("# Game Object")]
    public _1_Player player;
    public _5_PoolManager pool;

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 280, 360, 450, 600 };

    private void Awake()
    {
        
        instance = this;
    }

    private void Update()
    {

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {

            gameTime = maxGameTime;
        }
    }

    public void GetExp(int add = 1)
    {

        exp = add;

        if (exp >= nextExp[level])
        {

            level++;
            exp = 0;
        }
    }
}