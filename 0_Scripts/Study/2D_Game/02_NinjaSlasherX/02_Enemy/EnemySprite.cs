using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{

    EnemyMain enemyMain;

    void Awake()
    {

        // EnemyMain을 검색
        enemyMain = GetComponentInParent<EnemyMain>();
    }

    void OnWillRenderObject()
    {
        
        if (Camera.current.tag == "MainCamera")
        {

            // 처리
            enemyMain.cameraEnabled = true;
        }
    }
}
