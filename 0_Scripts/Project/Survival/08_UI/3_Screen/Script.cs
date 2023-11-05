using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Script
{

    [SerializeField] private int spriteNum;          // �̹��� ��ȣ
    [SerializeField] private float time = 5f;        // ��� ���� �ð� ���� ������ �ð� �÷����Ѵ�
    [SerializeField] private float nextTime = 2f;    // ���� ��� �ð�

    public Vector2 size;            // ��� ������ ũ��
    [SerializeField, TextArea(0, 3)] private string str;             // ���
    
    public int SpriteNum => spriteNum;
    public float Time => time;
    public float NextTime => nextTime;
    public string Str => str;
}
