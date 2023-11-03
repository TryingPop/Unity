using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWarning : MonoBehaviour
{

    [SerializeField] private Text warningTxt;

    private float startTime;
    private float endTime;
    public void Init(string _text, ref Color _color, float _chkTime)
    {

        startTime = Time.time;
        endTime = _chkTime;
        warningTxt.text = _text;
        _color.a = 0.7f;
        warningTxt.color = _color;
    }

    public bool ChkTime()
    {

        if (Time.time - startTime > endTime)
        {

            startTime = -endTime;
            return true;
        }

        return false;
    }
}
