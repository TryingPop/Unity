using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public static Menu instance;

    public GameObject go;
    public AudioManager theAudio;

    public string call_sound;
    public string cancel_sound;

    public OrderManager theOrder;

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
    /// ���� ����
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

                theOrder.Move();            // �ٷ� Ǯ���ָ� �ȵǴ� ������ ����Ѵ�!
                go.SetActive(false);
                theAudio.Play(cancel_sound);
            }
        }
    }
}
