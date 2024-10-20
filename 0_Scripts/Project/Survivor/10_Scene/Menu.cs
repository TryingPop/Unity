using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 설정 누르면 활성화할꺼
/// </summary>
public class Menu : MonoBehaviour
{

    public GameObject[] panels;

    public void NextScene(string _sceneName)
    {

        Time.timeScale = 1.0f;
        // SceneManager.LoadScene(_sceneName);
        LoadingScene.NextScene(_sceneName);
    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public void ActivePanel(int _idx)
    {

        panels[_idx].SetActive(true);
    }

    public void InActivePanel(int _idx)
    {

        panels[_idx].SetActive(false);
    }
}

