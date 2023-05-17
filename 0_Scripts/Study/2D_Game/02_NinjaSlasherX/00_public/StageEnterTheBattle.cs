using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnterTheBattle : MonoBehaviour
{

    [SerializeField] GameObject[] activeObjs;   // 배틀 진입 시 활성화 시킬 오브젝트들
    [SerializeField] bool once = true;
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        // 플레이어한테만 반응
        if (collision.tag == "PlayerBody")
        {

            for (int i = 0; i < activeObjs.Length; i++)
            {

                activeObjs[i].SetActive(true);
            }

            if (once)
            {

                Destroy(gameObject, 1f);
            }
        }
    }
}