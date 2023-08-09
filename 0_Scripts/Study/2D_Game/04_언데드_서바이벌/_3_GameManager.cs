using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _3_GameManager : MonoBehaviour
{

    // 장면이 하나라서 싱글톤으로는 안만든다
    public static _3_GameManager instance;

    [Header("# Game Object")]
    public _1_Player player;
    public _5_PoolManager pool;
    public _17_LevelUp uiLevelUp;
    public _18_Result uiResult;
    public GameObject enemyCleaner;

    public Transform uiJoy;

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
        Application.targetFrameRate = 60;           // 60FPS 로 
    }

    public void GameStart(int id)
    {

        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);

        // 기본 무기 지급
        uiLevelUp.Select(playerId % 2);
        Resume();

        _21_AudioManager.instance.PlaySfx(_21_AudioManager.Sfx.Select);
        _21_AudioManager.instance.PlayBgm(true);
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

        _21_AudioManager.instance.PlaySfx(_21_AudioManager.Sfx.Lose);
        _21_AudioManager.instance.PlayBgm(false);
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

        _21_AudioManager.instance.PlaySfx(_21_AudioManager.Sfx.Win);
        _21_AudioManager.instance.PlayBgm(false);
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
        uiJoy.localScale = Vector3.zero;
    }

    public void Resume()
    {

        isLive = true;
        Time.timeScale = 1f;
        uiJoy.localScale = Vector3.one;
    }

    public void GameQuit()
    {

        Application.Quit();
    }
}