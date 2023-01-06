using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public GameObject gameMode;

    public void OnStart()
    {

        // Start 버튼 클릭
        Debug.Log("스타트 버튼을 누르셨어요");
    }

    public void SelectMode(bool isActive)
    {

        gameMode.SetActive(isActive);
    }

    public void OnQuit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SelectScene(string SceneName)
    {

        SceneManager.LoadScene(SceneName);
    }

}
