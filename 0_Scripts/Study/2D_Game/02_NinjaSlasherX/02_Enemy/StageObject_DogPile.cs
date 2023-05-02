using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject_DogPile : MonoBehaviour
{

    public GameObject[] enemyList;
    public GameObject[] destroyObjectList;

    private void Start() 
    {

        InvokeRepeating("CheckEnemy", 0.0f, 1.0f);
    }

    void CheckEnemy()
    {

        // 등록되어 있는 적 리스트로 적이 생존 상태인지 확인
        // (1초에 한 번만 해도 된다)
        bool flag = true;
        foreach(GameObject enemy in enemyList)
        {

            if (enemy != null)
            {

                flag = false;
            }
        }

        // 모든 적이 쓰러졌는가?
        if (flag)
        {

            // 제거할 게임 오브젝트 리스트에 포함된 오브젝트를 삭제한다
            foreach(GameObject destroyGameObject in destroyObjectList)
            {

                Destroy(destroyGameObject, 1.0f);
            }

            CancelInvoke("CheckEnemy");
        }
    }
}
