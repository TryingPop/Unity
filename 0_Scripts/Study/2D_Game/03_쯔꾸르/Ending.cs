using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{

    public GameObject go;   // panel ĵ����
                            // ���⿡�� �Ʒ����� ���� �ö󰡴� �ִϸ��̼ǰ�
                            // ���� ������ ��� �ִ� Text�� �ִ�
                            // �����ӵ� ���ƾ��ϴµ� �����ϰ� �������

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (Input.GetKeyDown(KeyCode.Z))
        {

            go.SetActive(true);
        }
    }
}
