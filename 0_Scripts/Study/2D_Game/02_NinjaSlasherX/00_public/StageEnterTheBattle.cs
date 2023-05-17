using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnterTheBattle : MonoBehaviour
{

    [SerializeField] GameObject[] activeObjs;   // ��Ʋ ���� �� Ȱ��ȭ ��ų ������Ʈ��
    [SerializeField] bool once = true;
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        // �÷��̾����׸� ����
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