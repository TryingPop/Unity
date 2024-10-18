using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Obsolete("기대했던 성능이 안나온다.\n" +
        "크기를 키웠음에도 가장자리에서 인지를 못한다.", true)]
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

            Debug.Log("isRight 진입");
        }
        else if (isLeft) 
        { 
            
            Debug.Log("isLeft 진입");
        }


        if (isUp) 
        { 
        
            Debug.Log("isUp 진입");
        }
        else if (isDown) 
        {
            
            Debug.Log("isDown 진입");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (isRight)
        {

            Debug.Log("isRight 탈출");
        }
        else if (isLeft) 
        { 
            
            Debug.Log("isLeft 탈출");
        }

        if (isUp)
        {

            Debug.Log("isUp 탈출");
        }
        else if (isDown)
        {
            
            Debug.Log("isDown 탈출");
        }
    }
}