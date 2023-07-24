using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public enum Type
    {

        Ammo,
        Coin,
        Grenade,
        Heart,
        Weapon,
    };

    public Type type;
    public int value;
    public float rotateSpeed;

    private Rigidbody rigid;
    SphereCollider sphereCollider;

    private void Awake()
    {
        
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();        // 제일 위에꺼만 가져온다
    }

    private void Update()
    {

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Floor")
        {

            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
