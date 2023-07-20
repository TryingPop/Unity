using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public static Menu instance;

    public GameObject go;
    public AudioManager theAudio;

    public string call_sound;
    public string cancel_sound;

    public OrderManager theOrder;

    public GameObject[] gos;

    private bool activated;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void Exit()
    {

        Application.Quit();
    }

    public void Continue()
    {

        theOrder.NotMove();
        activated = false;
        go.SetActive(false);
        theAudio.Play(cancel_sound);
    }
    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            activated = !activated;

            if (activated)
            {

                theOrder.NotMove();
                go.SetActive(true);
                theAudio.Play(call_sound);
            }
            else
            {

                theOrder.Move();            // 바로 풀어주면 안되니 조건을 줘야한다!
                go.SetActive(false);
                theAudio.Play(cancel_sound);
            }
        }
    }

    public void GoToTitle()
    {

        // 타이틀 씬에서 옮겨지는 것들을 담아야한다
        for (int i = 0; i < gos.Length; i++)
        {

            Destroy(gos[i]);
        }
        go.SetActive(false);
        activated = false;

        // AudioListener를 AudioManager로 이동시킨다!
        SceneManager.LoadScene("title");
    }
}
