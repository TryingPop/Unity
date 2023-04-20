using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name == "Tank_Shell")
        {

            Debug.Log(">>>>>>>>>>>>>>>>> Hit!");
            // transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);   // Ŭ���� ������ ũ�Ⱑ 1�� ���� 3�� ���� �Ǿ��� �����Ƿ�
                                                                        // �����ϱ� ���ٴ� �ִ°� ����
            transform.localScale += Vector3.one;
            // rigidbody2D.AddForce(new Vector2(1000.0f, -1000.0f));    
            GetComponent<Rigidbody2D>().AddForce(new Vector2(1000.0f, -1000.0f));   // �տ����� GetComponent�� �� �����Ӹ��� �ҷ��ͼ� ���� ���� ������ rigidbody2D ������ �����ؼ� ��µ�
                                                                                    // �� �����Ӹ��� üũ �ϴ°� �ƴ� 1���� üũ�ؼ� GetComponent �̿�
        }
    }
}
