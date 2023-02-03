using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public GameObject gameMode;

    private void Awake()
    {

        // 게임화면에서 타이틀로 올 때 DontDestroyObject이므로 파괴안되어서 직접 파괴
        if (DontDestroyObj.instance != null) Destroy(DontDestroyObj.instance.gameObject);
        Time.timeScale = 1.0f;
    }

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
        // Debug.Log(SceneName);
    }

}
