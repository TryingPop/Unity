using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    [SerializeField] private Text scriptTxt;
    [SerializeField] private RectTransform myRectTrans;
    [SerializeField] private Vector3 initPos;
    [SerializeField] private Vector3 destPos;
    private Vector3 destination;
    private float startTime;
    private float endTime;

    public void Init(string _text, ref Vector3 _destination, float _time = 5.0f)
    {

        scriptTxt.text = _text;
        destination = _destination;
        startTime = Time.time;
        endTime = _time;
    }

    // 위치 조절
    public void SetPos()
    {

        if (Time.time - startTime < endTime)
        {

            myRectTrans.anchoredPosition = Vector3.Lerp(myRectTrans.anchoredPosition, destination, 0.1f);
        }
        else
        {

            myRectTrans.anchoredPosition = initPos;
        }
    }

    // Test 용도
    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {

            
            Init("내용 글", ref destPos);
        }

        SetPos();
    }
}