using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptSlot : MonoBehaviour
{

    [SerializeField] private Text scriptTxt;
    [SerializeField] private RectTransform myRectTrans;
    [SerializeField] private Image talker;
    // [SerializeField] private Vector3 destPos;
    private Vector3 destination;
    private float startTime;
    private float endTime;

    public void Init(Sprite _img, string _text, ref Vector2 scriptSize, float _time = 5.0f)
    {

        talker.sprite = _img;
        scriptTxt.text = _text;
        destination = new Vector2(0f, -35f);
        startTime = Time.time;
        endTime = _time;
        myRectTrans.sizeDelta = scriptSize;
    }

    // 초기 위치 이동 및 초기 사이즈
    public void EndPos(ref Vector3 initPos)
    {

        myRectTrans.anchoredPosition = initPos;
        myRectTrans.sizeDelta = new Vector2(100f, 60f);
    }

    // 다음 위치
    public void SetNext(float _posY)
    {

        destination.y -= _posY;
    }

    // 위치 조절
    private void SetPos()
    {

        myRectTrans.anchoredPosition = Vector3.Lerp(myRectTrans.anchoredPosition, destination, 0.1f);
    }

    // 해야하는지 판별
    public bool ChkTime()
    {

        if (Time.time - startTime > endTime)
        {

            startTime = -endTime;
            scriptTxt.text = null;
            return true;
        }

        // 위치 조절
        SetPos();
        return false;
    }
}