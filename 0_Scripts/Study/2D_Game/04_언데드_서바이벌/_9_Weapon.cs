using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _9_Weapon : MonoBehaviour
{

    public int id;
    public int prefabId;
    public float damage;
    public int count;           // 개수

    public float speed;         // 회전 속도

    private float timer;

    public readonly int Infinity = -1;

    _1_Player player;

    private void Awake()
    {
        
        player = GetComponentInParent<_1_Player>();
    }

    private void Start()
    {

        Init();
    }

    public void Init()
    {

        switch (id)
        {

            case 0:

                speed = 150;
                Batch();

                break;

            case 1:

                speed = speed > 0 ? speed : 0.3f;
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
            bullet.Rotate(rotVec);                              // 생성한 불렛을 회전
            bullet.Translate(bullet.up * 1.5f, Space.World);      // 불렛의 윗 방향으로 world좌표로 이동

            bullet.GetComponent<_8_Bullet>().Init(damage, Infinity, Vector2.zero);
        }
    }

    private void Update()
    {

        switch (id)
        {

            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;

            case 1:
                timer += Time.deltaTime;

                if (timer > speed)
                {

                    timer = 0;
                    Fire();
                }

                break;

            default:

                break;
        }

#if true
        // Test Code
        if (Input.GetButtonDown("Jump"))
        {

            LevelUp(1, 1);
        }
#endif
    }


    public void LevelUp(float damage, int count)
    {

        this.damage += damage;
        this.count += count;

        if (id == 0)
        {

            Batch();
        }
    }

    private void Fire()
    {

        // 대상 유무 판별
        if (!player.scanner.nearestTarget) return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = _3_GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<_8_Bullet>().Init(damage, count, dir);
    }
}