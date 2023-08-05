using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class _3_GameManager : MonoBehaviour
{

    // ����� �ϳ��� �̱������δ� �ȸ����
    public static _3_GameManager instance;

    [Header("# Game Object")]
    public _1_Player player;
    public _5_PoolManager pool;
    public _17_LevelUp uiLevelUp;

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool isLive = true;

    [Header("# Player Info")]
    public int health;
    public int maxHealth = 100;
    
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 280, 360, 450, 600 };


    private void Awake()
    {
        
        instance = this;
    }

    private void Start()
    {

        health = maxHealth;

        // �ӽ� ��ũ��Ʈ (ù ��° ĳ���� ����)
        uiLevelUp.Select(0);
    }

    private void Update()
    {
        
        if (!isLive) return;

        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {

            gameTime = maxGameTime;
        }
    }

    public void GetExp(int add = 1)
    {

        exp += add;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {

            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {

        isLive = false;
        Time.timeScale = 0f;
    }

    public void Resume()
    {

        isLive = true;
        Time.timeScale = 1f;
    }
}