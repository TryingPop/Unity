using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Obsolete("����ߴ� ������ �ȳ��´�.\n" +
        "ũ�⸦ Ű�������� �����ڸ����� ������ ���Ѵ�.", true)]
public class UICam : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private CameraMovement camMove;

    [SerializeField] private bool isLeft;
    [SerializeField] private bool isRight;
    [SerializeField] private bool isUp;
    [SerializeField] private bool isDown;

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (isRight)
        {

            Debug.Log("isRight ����");
        }
        else if (isLeft) 
        { 
            
            Debug.Log("isLeft ����");
        }


        if (isUp) 
        { 
        
            Debug.Log("isUp ����");
        }
        else if (isDown) 
        {
            
            Debug.Log("isDown ����");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (isRight)
        {

            Debug.Log("isRight Ż��");
        }
        else if (isLeft) 
        { 
            
            Debug.Log("isLeft Ż��");
        }

        if (isUp)
        {

            Debug.Log("isUp Ż��");
        }
        else if (isDown)
        {
            
            Debug.Log("isDown Ż��");
        }
    }
}