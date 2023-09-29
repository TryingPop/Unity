using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMissileRotation : MonoBehaviour
{

    Rigidbody myRigid;

    [SerializeField] bool isMove;

    [SerializeField] Transform target;


    private void Awake()
    {

        myRigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.Space))
        {

            isMove = !isMove;
        }

        if (Input.GetKey(KeyCode.Z) )
        {

            transform.position = new Vector3(0f, 2f, -5f);
            // transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            myRigid.velocity = Vector3.zero;
        }

        if (isMove)
        {

            // �ش� ��ǥ�� �ٶ󺸰�, ���� ���� ���� �״�� �Ѵ�
            myRigid.rotation = Quaternion.LookRotation(target.position - transform.position, transform.up);
            // ���Ŀ� z������ ȸ��
            myRigid.rotation *= Quaternion.Euler(0f, 0f, 10f);

            // ������ ���� �� �ٷ� ����� �� �ִ�
            // myRigid.MoveRotation(Quaternion.LookRotation(target.position - transform.position, transform.up) * Quaternion.Euler(0f, 0f, 10f));
        }

        if (Input.GetKey(KeyCode.X))
        {

            /*
            // ���� ����
            if (Quaternion.Euler(1f, 1f, 1f) == Quaternion.Euler(1f, 1f, 0f) * Quaternion.Euler(0f, 0f, 1f))
            {

                Debug.Log("���� ���ƿ�");
            }
            else
            {

                Debug.Log("���� �޶�");
            }
            */

        }
    }
}
