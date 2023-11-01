using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWarning : MonoBehaviour
{

    [SerializeField] private Text warningTxt;

    private float onTime;
    private float chkTime;
    public void Init(string _text, ref Color _color, float _chkTime)
    {

        onTime = Time.time;
        chkTime = _chkTime;
        warningTxt.text = _text;
        warningTxt.color = _color;
    }

    public bool SetTime()
    {

        if (Time.time - onTime > chkTime)
        {

            onTime = 1f;
            return true;
        }

        return false;
    }
}
