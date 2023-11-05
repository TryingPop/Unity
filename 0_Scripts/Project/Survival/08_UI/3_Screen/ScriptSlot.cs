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

    // �ʱ� ��ġ �̵� �� �ʱ� ������
    public void EndPos(ref Vector3 initPos)
    {

        myRectTrans.anchoredPosition = initPos;
        myRectTrans.sizeDelta = new Vector2(100f, 60f);
    }

    // ���� ��ġ
    public void SetNext(float _posY)
    {

        destination.y -= _posY;
    }

    // ��ġ ����
    private void SetPos()
    {

        myRectTrans.anchoredPosition = Vector3.Lerp(myRectTrans.anchoredPosition, destination, 0.1f);
    }

    // �ؾ��ϴ��� �Ǻ�
    public bool ChkTime()
    {

        if (Time.time - startTime > endTime)
        {

            startTime = -endTime;
            scriptTxt.text = null;
            return true;
        }

        // ��ġ ����
        SetPos();
        return false;
    }
}