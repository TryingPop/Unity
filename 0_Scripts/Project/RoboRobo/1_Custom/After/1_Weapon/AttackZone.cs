using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class AttackZone : MonoBehaviour
{

    private float size = 0f;                                // ������ ũ�� ����
    WaitForSeconds waitTime = new WaitForSeconds(0.02f);    // 0.02�� �������� ������ ����

    [SerializeField] private GameObject readyZone;          // �غ� �� Ÿ�ְ̹� ���� �˷��ֱ� ���� ������Ʈ

    private void OnEnable()
    {

        // �غ� �� Ȱ��ȭ
        readyZone.SetActive(true);

        // ũ�� ���� 0.02�� �������� 0.02�� ����
        StartCoroutine(ExpandZone());
    }

    private void OnDisable()
    {
        size = 0f;
        transform.localScale = Vector3.zero;

        // 1�� �ڿ� ��Ȱ��ȭ������ ���� ���� ���� �־ Ȯ���ϰ� ��Ȱ��ȭ üũ
        readyZone.SetActive(false);
    }


    /// <summary>
    /// 0.02�� �������� 2%�� ũ�� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpandZone()
    {

        // ����� 1�ΰ�� Ż��
        while (size < 1f)
        {

            SizeUp();
            
            yield return waitTime;
        }
        
        // �غ� ���� ��Ȱ��ȭ
        readyZone.SetActive(false);
    }
    
    /// <summary>
    /// ���� ������Ʈ�� ����� 2%�� ����
    /// </summary>
    private void SizeUp()
    {

        size += 0.02f;
        if (size > 1f)
        {

            size = 1f;
        }

        // ������ ����
        transform.localScale = Vector3.one * size;
    }
}
