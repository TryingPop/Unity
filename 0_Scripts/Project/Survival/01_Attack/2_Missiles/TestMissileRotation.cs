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

            // 해당 좌표를 바라보고, 위는 현재 위를 그대로 한다
            myRigid.rotation = Quaternion.LookRotation(target.position - transform.position, transform.up);
            // 이후에 z축으로 회전
            myRigid.rotation *= Quaternion.Euler(0f, 0f, 10f);

            // 다음과 같이 한 줄로 요약할 수 있다
            // myRigid.MoveRotation(Quaternion.LookRotation(target.position - transform.position, transform.up) * Quaternion.Euler(0f, 0f, 10f));
        }

        if (Input.GetKey(KeyCode.X))
        {

            /*
            // 둘이 같다
            if (Quaternion.Euler(1f, 1f, 1f) == Quaternion.Euler(1f, 1f, 0f) * Quaternion.Euler(0f, 0f, 1f))
            {

                Debug.Log("둘이 같아영");
            }
            else
            {

                Debug.Log("둘이 달라영");
            }
            */

        }
    }
}
