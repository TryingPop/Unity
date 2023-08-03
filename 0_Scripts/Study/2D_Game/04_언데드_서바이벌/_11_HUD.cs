using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _11_HUD : MonoBehaviour
{

    public enum InfoType
    {

        Exp,
        Level,
        Kill,
        Time,
        Health,
    }

    public InfoType type;

    private Text myText;
    private Slider mySlider;

    private void Awake()
    {

        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {

        switch (type)
        {

            case InfoType.Exp:

                if (_3_GameManager.instance.level >= _3_GameManager.instance.nextExp.Length)
                {

                    enabled = false;
                    break;
                }

                float curExp = _3_GameManager.instance.exp;
                float maxExp = _3_GameManager.instance.nextExp[_3_GameManager.instance.level];
                mySlider.value = curExp / maxExp;

                break;

            case InfoType.Level:
                
                myText.text = string.Format("Lv.{0:F0}",_3_GameManager.instance.level);
                break;

            case InfoType.Kill:

                myText.text = string.Format("{0:F0}", _3_GameManager.instance.kill);
                break;

            case InfoType.Time:

                float remainTime = _3_GameManager.instance.maxGameTime - _3_GameManager.instance.gameTime;
                remainTime = remainTime < 0 ? 0 : remainTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);

                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;

            case InfoType.Health:

                float curHealth = _3_GameManager.instance.health;
                float maxHealth = _3_GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;

                break;
        }
    }
}