using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class AttackZone : MonoBehaviour
{

    private float size = 0f;                                // 사이즈 크기 변수
    WaitForSeconds waitTime = new WaitForSeconds(0.02f);    // 0.02초 간격으로 사이즈 증가

    [SerializeField] private GameObject readyZone;          // 준비 존 타이밍과 범위 알려주기 위한 오브젝트

    private void OnEnable()
    {

        // 준비 존 활성화
        readyZone.SetActive(true);

        // 크기 증가 0.02초 간격으로 0.02씩 증가
        StartCoroutine(ExpandZone());
    }

    private void OnDisable()
    {
        size = 0f;
        transform.localScale = Vector3.zero;

        // 1초 뒤에 비활성화했지만 먼저 꺼질 수도 있어서 확실하게 비활성화 체크
        readyZone.SetActive(false);
    }


    /// <summary>
    /// 0.02초 간격으로 2%씩 크기 증가
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpandZone()
    {

        // 사이즈가 1인경우 탈출
        while (size < 1f)
        {

            SizeUp();
            
            yield return waitTime;
        }
        
        // 준비 존은 비활성화
        readyZone.SetActive(false);
    }
    
    /// <summary>
    /// 게임 오브젝트의 사이즈가 2%씩 증가
    /// </summary>
    private void SizeUp()
    {

        size += 0.02f;
        if (size > 1f)
        {

            size = 1f;
        }

        // 사이즈 적용
        transform.localScale = Vector3.one * size;
    }
}
