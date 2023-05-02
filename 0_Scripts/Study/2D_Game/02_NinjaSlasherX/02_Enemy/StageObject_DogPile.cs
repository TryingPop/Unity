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

        // ��ϵǾ� �ִ� �� ����Ʈ�� ���� ���� �������� Ȯ��
        // (1�ʿ� �� ���� �ص� �ȴ�)
        bool flag = true;
        foreach(GameObject enemy in enemyList)
        {

            if (enemy != null)
            {

                flag = false;
            }
        }

        // ��� ���� �������°�?
        if (flag)
        {

            // ������ ���� ������Ʈ ����Ʈ�� ���Ե� ������Ʈ�� �����Ѵ�
            foreach(GameObject destroyGameObject in destroyObjectList)
            {

                Destroy(destroyGameObject, 1.0f);
            }

            CancelInvoke("CheckEnemy");
        }
    }
}
