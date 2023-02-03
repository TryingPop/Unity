using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public GameObject gameMode;

    private void Awake()
    {

        // ����ȭ�鿡�� Ÿ��Ʋ�� �� �� DontDestroyObject�̹Ƿ� �ı��ȵǾ ���� �ı�
        if (DontDestroyObj.instance != null) Destroy(DontDestroyObj.instance.gameObject);
        Time.timeScale = 1.0f;
    }

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
        // Debug.Log(SceneName);
    }

}
