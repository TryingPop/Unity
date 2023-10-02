using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotation : MonoBehaviour
{

    [SerializeField] private Rigidbody myRigid;
    [SerializeField] private Vector3 powVec;
    [SerializeField] private Vector3 rotVec;

    private void OnEnable()
    {

        // ����� ���� ����... �ʿ�!
        myRigid.AddForce(powVec, ForceMode.Impulse);
        myRigid.AddTorque(rotVec, ForceMode.Impulse);
    }


    private void OnTriggerEnter(Collider other)
    {

        gameObject.SetActive(false);
    }
}