using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_HiScore : MonoBehaviour
{

    void Start()
    {

        zFoxFadeFilter.instance.FadeIn(Color.black, 0.5f);
        for (int i = 0; i <= 10; i++)
        {

            TextMesh tm = GameObject.Find("Rank" + i).GetComponent<TextMesh>();
            if ( i == SaveData.newRecord)
            {

                tm.color = Color.red;
            }

            tm.text = string.Format("{0, 2} {1, 10}", i, SaveData.HiScore[(i - 1)]);    // 뒤에 2 와 10은 자리수
        }
    }

    void Button_Prev(MenuObject_Button button)
    {

        zFoxFadeFilter.instance.FadeOut(Color.black, 0.5f);
        Invoke("SceneJump", 0.7f);
        AppSound.instance.SE_MENU_CANCEL.Play();
    }

    void SceneJump()
    {

        SceneManager.LoadScene("Menu_Title");
    }
}
