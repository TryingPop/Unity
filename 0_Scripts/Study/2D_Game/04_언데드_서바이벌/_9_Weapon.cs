using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _9_Weapon : MonoBehaviour
{

    public int id;
    public int prefabId;
    public float damage;
    public int count;           // ����

    public float speed;         // ȸ�� �ӵ�

    public readonly int Infinity = -1;

    private void Start()
    {

        Init();
    }

    public void Init()
    {

        switch (id)
        {

            case 0:

                speed = -150;
                Batch();

                break;

            default:

                break;
        }
    }

    private void Batch()
    {

        for (int index = 0; index < count; index++)
        {

            Transform bullet;

            if (index < transform.childCount)
            {

                bullet = transform.GetChild(index);
            }
            else
            {

                bullet = _3_GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;
            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);                              // ������ �ҷ��� ȸ��
            bullet.Translate(bullet.up * 1.5f, Space.World);      // �ҷ��� �� �������� world��ǥ�� �̵�

            bullet.GetComponent<_8_Bullet>().Init(damage, Infinity);
        }
    }

    private void Update()
    {

        switch (id)
        {

            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);

                break;

            default:

                break;
        }

#if true
        // Test Code
        if (Input.GetButtonDown("Jump"))
        {

            LevelUp(20, 5);
        }
#endif
    }


    public void LevelUp(float damage, int count)
    {

        this.damage = damage;
        this.count += count;

        if (id == 0)
        {

            Batch();
        }
    }
}