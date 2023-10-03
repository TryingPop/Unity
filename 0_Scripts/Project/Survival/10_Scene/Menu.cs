using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject panelMenu;
    public GameObject panelOpt;

    public void NextScene(string _sceneName)
    {

        SceneManager.LoadScene(_sceneName);
    }

    public void QuitGame()
    {

        Application.Quit();
    }

    public void ActiveOption(bool _active)
    {

        panelOpt.SetActive(_active);
        panelMenu.SetActive(!_active);
    }

    // option������ �ൿ! ���� ���� <<< �������, ȿ������
    // �׸��� Save �� �� ���� ���� �ϴ°�
}

