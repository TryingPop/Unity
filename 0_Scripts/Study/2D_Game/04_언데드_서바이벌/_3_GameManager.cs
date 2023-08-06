using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _3_GameManager : MonoBehaviour
{

    // ����� �ϳ��� �̱������δ� �ȸ����
    public static _3_GameManager instance;

    [Header("# Game Object")]
    public _1_Player player;
    public _5_PoolManager pool;
    public _17_LevelUp uiLevelUp;
    public _18_Result uiResult;
    public GameObject enemyCleaner;

    [Header("# Game Control")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public bool isLive = true;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100f;
    
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 280, 360, 450, 600 };


    private void Awake()
    {
        
        instance = this;
    }

    public void GameStart(int id)
    {

        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);

        // �⺻ ���� ����
        uiLevelUp.Select(playerId % 2);
        Resume();
    }

    public void GameOver()
    {

        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {

        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }
    public void GameVictory()
    {

        StartCoroutine(GameVictoryRoutine());
    }

    private IEnumerator GameVictoryRoutine()
    {

        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        
        if (!isLive) return;

        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {

            gameTime = maxGameTime;
            GameVictory();
        }
    }

    public void GetExp(int add = 1)
    {

        if (!isLive) return;

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