using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;

    public GameObject target;       // ī�޶� ���� ���
    public float moveSpeed;         // ī�޶� �󸶳� ���� �ӵ���
    private Vector3 targetPosition; // ����� ��ġ

    private void Start()
    {

        if (instance != null)
        {

            Destroy(this.gameObject);
        }
        else
        {

            DontDestroyOnLoad(this.gameObject);

            instance = this;
        }
    }
    private void Update()
    {
        
        if (target != null)
        {

            targetPosition.Set(
                target.transform.position.x, target.transform.position.y, 
                this.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);


        }
    }
}
