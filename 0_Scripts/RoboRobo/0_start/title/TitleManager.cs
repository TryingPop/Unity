using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public GameObject gameMode;

    public void OnStart()
    {

        // Start ��ư Ŭ��
        Debug.Log("��ŸƮ ��ư�� �����̾��");
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
