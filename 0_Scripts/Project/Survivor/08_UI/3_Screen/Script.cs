using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script
{

    [SerializeField] private int spriteNum;          // 이미지 번호
    [SerializeField] private float time = 5f;        // 대사 유지 시간 글자 많으면 시간 늘려야한다
    [SerializeField] private float nextTime = 2f;    // 다음 대사 시간

    public Vector2 size;            // 대사 슬롯의 크기
    [SerializeField, TextArea(0, 3)] private string str;             // 대사
    
    public int SpriteNum => spriteNum;
    public float Time => time;
    public float NextTime => nextTime;
    public string Str => str;
}
