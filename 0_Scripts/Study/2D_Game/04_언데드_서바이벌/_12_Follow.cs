using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _12_Follow : MonoBehaviour
{

    RectTransform rect;

    private void Awake()
    {

        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {

        rect.position = Camera.main.WorldToScreenPoint(_3_GameManager.instance.player.transform.position);  // ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
    }
}