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

    private float timer;

    public readonly int Infinity = -1;

    _1_Player player;

    private void Awake()
    {

        // player = GetComponentInParent<_1_Player>();
        player = _3_GameManager.instance.player;
    }

    public void Init(_13_ItemData data)
    {

        // Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage * _19_Character.Damage;
        count = data.baseCount + _19_Character.Count;

        for (int index = 0; index < _3_GameManager.instance.pool.prefabs.Length; index++)
        {

            if (data.projectile == _3_GameManager.instance.pool.prefabs[index])
            {

                prefabId = index;
                break;
            }
        }

        switch (id)
        {

            case 0:

                speed = 150 * _19_Character.WeaponSpeed;
                Batch();

                break;

            case 1:

                speed = speed > 0 ? speed : 0.5f;
                speed *= _19_Character.WeaponRate;
                break;
            default:

                break;
        }

        // Hand Set
        _16_Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // �ڱ� �ڽŰ� �ڽ� ��ο��� ApplyGear �̸��� �޼��� ����
        // �� ��° �Ű������� ���� ��� ApplyGear�޼��带 ���� ���ϸ� ���� �޽����� �ߴµ�,
        // �� ��° �Ű������� ������ ���� �޽����� �ȶ��
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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

            bullet.GetComponent<_8_Bullet>().Init(damage, Infinity, Vector2.zero);
        }
    }

    private void Update()
    {

        if (!_3_GameManager.instance.isLive) return;

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

#if false
        // Test Code
        if (Input.GetButtonDown("Jump"))
        {

            LevelUp(1, 1);
        }
#endif
    }


    public void LevelUp(float damage, int count)
    {

        this.damage += damage * _19_Character.Damage;
        this.count += count + _19_Character.Count;

        if (id == 0)
        {

            Batch();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    private void Fire()
    {

        // ��� ���� �Ǻ�
        if (!player.scanner.nearestTarget) return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = _3_GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bullet.GetComponent<_8_Bullet>().Init(damage, count, dir);

        _21_AudioManager.instance.PlaySfx(_21_AudioManager.Sfx.Range);
    }
}