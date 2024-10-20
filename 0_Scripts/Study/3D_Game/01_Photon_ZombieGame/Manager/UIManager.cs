using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// 필요한 UI에 즉시 접근하고 변경할 수 있도록 허용하는 UI 매니저
public class UIManager : MonoBehaviour
{

    // 싱글톤 접근용 프로퍼티
    public static UIManager instance
    {

        get
        {

            if (m_instance == null)
            {

                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance;

    public Text ammoText;           // 탄알 표시용 텍스트
    public Text scoreText;          // 점수 표시용 텍스트
    public Text waveText;           // 적 웨이브 표시용 텍스트
    public GameObject gameoverUI;   // 게임오버 시 활성화할 UI

    // 탄알 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {

        ammoText.text = magAmmo + "/" + remainAmmo;
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {

        scoreText.text = "Score : " + newScore;
    }

    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {

        waveText.text = "Wave : " + waves + "\nEnemy Left : " + count;
    }

    // 게임오버 UI 활성화
    public void SetActiveGameoverUI(bool active)
    {

        gameoverUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}