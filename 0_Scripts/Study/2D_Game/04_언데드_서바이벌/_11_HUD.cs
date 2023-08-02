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

                break;

            case InfoType.Level:

                break;

            case InfoType.Time:

                break;

            case InfoType.Kill:

                break;

            case InfoType.Health:

                break;
        }
    }
}