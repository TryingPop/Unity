using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{

    EnemyMain enemyMain;

    void Awake()
    {

        // EnemyMain�� �˻�
        enemyMain = GetComponentInParent<EnemyMain>();
    }

    void OnWillRenderObject()
    {
        
        if (Camera.current.tag == "MainCamera")
        {

            // ó��
            enemyMain.cameraEnabled = true;
        }
    }
}
