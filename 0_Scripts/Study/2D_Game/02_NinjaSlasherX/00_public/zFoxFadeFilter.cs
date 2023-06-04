using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public enum FOXFADE_STATE 
{ 

    NON,
    IN,
    OUT,
}



public class zFoxFadeFilter : MonoBehaviour
{

    public static zFoxFadeFilter instance = null;

    // 외부 파라미터 (Inspector 표시)
    public GameObject fadeFilterObject = null;
    public string attacheObject = "FadeFilterPoint";

    // 외부 파라미터
    [HideInInspector] public FOXFADE_STATE fadeState;

    // 내부 파라미터
    private float startTime;
    private float fadeTime;
    private Color fadeColor;

    private new Renderer renderer;

    // 코드 (MonoBehaviour 기본 기능 구현)
    void Awake()
    {

        instance = this;

        fadeState = FOXFADE_STATE.NON;

        renderer = fadeFilterObject.GetComponent<Renderer>();
    }

    void SetFadeAction(FOXFADE_STATE state, Color color, float time) 
    {

        fadeState = state;
        startTime = Time.time ;
        fadeTime = time;
        fadeColor = color;
    }

    public void FadeIn(Color color, float time)
    {

        SetFadeAction(FOXFADE_STATE.IN, color, time);
    }

    public void FadeOut(Color color, float time)
    {

        SetFadeAction(FOXFADE_STATE.OUT, color, time);
    }

    void SetFadeFilterColor(bool enabled, Color color)
    {

        if (fadeFilterObject)
        {

            renderer.enabled = enabled;
            renderer.material.color = color;
            SpriteRenderer sprite = fadeFilterObject.GetComponent<SpriteRenderer>();

            if (sprite)
            {

                sprite.enabled = enabled;
                sprite.color = color;
                fadeFilterObject.SetActive(enabled);
            }
        }
    }

    void Update()
    {
        
        // 페이드 필터를 적용한다
        if (attacheObject != null)
        {

            GameObject go = GameObject.Find(attacheObject);
            fadeFilterObject.transform.position = go.transform.position;
        }

        // 페이드 처리
        switch (fadeState)
        {

            case FOXFADE_STATE.NON:
                break;

            case FOXFADE_STATE.IN:
                fadeColor.a = 1.0f - ((Time.time - startTime) / fadeTime);
                if (fadeColor.a > 1.0f || fadeColor.a < 0.0f)
                {

                    fadeColor.a = 0.0f;
                    fadeState = FOXFADE_STATE.NON;
                    SetFadeFilterColor(false, fadeColor);
                    break;
                }
                SetFadeFilterColor(true, fadeColor);
                break;

            case FOXFADE_STATE.OUT:
                fadeColor.a = (Time.time - startTime) / fadeTime;
                if (fadeColor.a > 1.0f || fadeColor.a < 0.0f)
                {

                    fadeColor.a = 1.0f;
                    fadeState = FOXFADE_STATE.NON;
                }

                SetFadeFilterColor(true, fadeColor);
                break;
        }
    }
}
