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

            camMove.AddHorizontal = 1f;
            Debug.Log("isRight ����");
        }
        else if (isLeft) 
        { 
            
            camMove.AddHorizontal = -1f;
            Debug.Log("isLeft ����");
        }


        if (isUp) 
        { 
        
            camMove.AddVertical = 1f;
            Debug.Log("isUp ����");
        }
        else if (isDown) 
        {
            
            camMove.AddVertical = -1f;
            Debug.Log("isDown ����");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (isRight)
        {

            camMove.AddHorizontal = 0f;
            Debug.Log("isRight Ż��");
        }
        else if (isLeft) 
        { 
            
            camMove.AddHorizontal = 0f;
            Debug.Log("isLeft Ż��");
        }

        if (isUp)
        {

            camMove.AddVertical = 0f;
            Debug.Log("isUp Ż��");
        }
        else if (isDown)
        {
            
            camMove.AddVertical = 0f;
            Debug.Log("isDown Ż��");
        }
    }
}